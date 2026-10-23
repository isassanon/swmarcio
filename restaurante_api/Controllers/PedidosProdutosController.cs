using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using restaurante_api.Data;
using restaurante_api.Models;

namespace restaurante_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PedidosProdutosController : ControllerBase
{
    private readonly RestauranteContext _context;

    public PedidosProdutosController(RestauranteContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PedidoProduto>>>
        GetPedidosProdutos()
    {
        return await _context.PedidosProdutos
            .Include(pp => pp.Pedido)
            .Include(pp => pp.Produto)
            .AsNoTracking()
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PedidoProduto>>
        GetPedidoProduto(int id)
    {
        var pedidoProduto = await _context.PedidosProdutos
            .Include(pp => pp.Pedido)
            .Include(pp => pp.Produto)
            .AsNoTracking()
            .FirstOrDefaultAsync(pp => pp.PedidoProdutoId == id);

        if (pedidoProduto == null)
        {
            return NotFound();
        }

        return pedidoProduto;
    }

    [HttpPost]
    public async Task<ActionResult<PedidoProduto>>
        PostPedidoProduto(PedidoProduto pedidoProduto)
    {
        if (pedidoProduto.PedidoProdutoQuantidade <= 0)
        {
            return BadRequest("A quantidade deve ser maior que zero.");
        }

        var pedidoExiste = await _context.Pedidos
            .AnyAsync(p => p.PedidoId == pedidoProduto.PedidoId);

        if (!pedidoExiste)
        {
            return BadRequest("O pedido informado não existe.");
        }

        var produtoExiste = await _context.Produtos
            .AnyAsync(p => p.ProdutoId == pedidoProduto.ProdutoId);

        if (!produtoExiste)
        {
            return BadRequest("O produto informado não existe.");
        }

        var itemDuplicado = await _context.PedidosProdutos
            .AnyAsync(pp =>
                pp.PedidoId == pedidoProduto.PedidoId &&
                pp.ProdutoId == pedidoProduto.ProdutoId);

        if (itemDuplicado)
        {
            return Conflict(
                "Esse produto já está incluído nesse pedido.");
        }

        _context.PedidosProdutos.Add(pedidoProduto);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetPedidoProduto),
            new { id = pedidoProduto.PedidoProdutoId },
            pedidoProduto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutPedidoProduto(
        int id,
        PedidoProduto pedidoProduto)
    {
        if (id != pedidoProduto.PedidoProdutoId)
        {
            return BadRequest(
                "O ID da URL é diferente do ID do item do pedido.");
        }

        if (pedidoProduto.PedidoProdutoQuantidade <= 0)
        {
            return BadRequest("A quantidade deve ser maior que zero.");
        }

        var pedidoExiste = await _context.Pedidos
            .AnyAsync(p => p.PedidoId == pedidoProduto.PedidoId);

        var produtoExiste = await _context.Produtos
            .AnyAsync(p => p.ProdutoId == pedidoProduto.ProdutoId);

        if (!pedidoExiste || !produtoExiste)
        {
            return BadRequest("O pedido ou produto informado não existe.");
        }

        _context.Entry(pedidoProduto).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PedidoProdutoExists(id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePedidoProduto(int id)
    {
        var pedidoProduto = await _context.PedidosProdutos
            .FindAsync(id);

        if (pedidoProduto == null)
        {
            return NotFound();
        }

        _context.PedidosProdutos.Remove(pedidoProduto);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool PedidoProdutoExists(int id)
    {
        return _context.PedidosProdutos
            .Any(pp => pp.PedidoProdutoId == id);
    }
}