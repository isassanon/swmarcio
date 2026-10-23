using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using restaurante_api.Data;
using restaurante_api.Models;

namespace restaurante_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PedidosController : ControllerBase
{
    private readonly RestauranteContext _context;

    public PedidosController(RestauranteContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidos()
    {
        return await _context.Pedidos
            .Include(p => p.Cliente)
            .Include(p => p.PedidosProdutos)
                .ThenInclude(pp => pp.Produto)
            .AsNoTracking()
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Pedido>> GetPedido(int id)
    {
        var pedido = await _context.Pedidos
            .Include(p => p.Cliente)
            .Include(p => p.PedidosProdutos)
                .ThenInclude(pp => pp.Produto)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.PedidoId == id);

        if (pedido == null)
        {
            return NotFound();
        }

        return pedido;
    }

    [HttpPost]
    public async Task<ActionResult<Pedido>> PostPedido(Pedido pedido)
    {
        var clienteExiste = await _context.Clientes
            .AnyAsync(c => c.ClienteId == pedido.ClienteId);

        if (!clienteExiste)
        {
            return BadRequest("O cliente informado não existe.");
        }

        var statusValidos = new[]
        {
            "aberto",
            "preparando",
            "entregue",
            "cancelado"
        };

        if (!statusValidos.Contains(pedido.PedidoStatus))
        {
            return BadRequest(
                "O status deve ser aberto, preparando, entregue ou cancelado.");
        }

        if (pedido.PedidoData == default)
        {
            pedido.PedidoData = DateTime.Now;
        }

        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetPedido),
            new { id = pedido.PedidoId },
            pedido);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutPedido(int id, Pedido pedido)
    {
        if (id != pedido.PedidoId)
        {
            return BadRequest("O ID da URL é diferente do ID do pedido.");
        }

        var clienteExiste = await _context.Clientes
            .AnyAsync(c => c.ClienteId == pedido.ClienteId);

        if (!clienteExiste)
        {
            return BadRequest("O cliente informado não existe.");
        }

        var statusValidos = new[]
        {
            "aberto",
            "preparando",
            "entregue",
            "cancelado"
        };

        if (!statusValidos.Contains(pedido.PedidoStatus))
        {
            return BadRequest("Status de pedido inválido.");
        }

        _context.Entry(pedido).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PedidoExists(id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePedido(int id)
    {
        var pedido = await _context.Pedidos.FindAsync(id);

        if (pedido == null)
        {
            return NotFound();
        }

        var possuiProdutos = await _context.PedidosProdutos
            .AnyAsync(pp => pp.PedidoId == id);

        if (possuiProdutos)
        {
            return Conflict(
                "Não é possível excluir um pedido que possui produtos.");
        }

        _context.Pedidos.Remove(pedido);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool PedidoExists(int id)
    {
        return _context.Pedidos.Any(p => p.PedidoId == id);
    }
}