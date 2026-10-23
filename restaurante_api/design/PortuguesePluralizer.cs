using Microsoft.EntityFrameworkCore.Design;

namespace restaurante_api.Design;

public class PortuguesePluralizer : IPluralizer
{
    public string Singularize(string identifier)
    {
        var normalized = identifier
            .Replace("_", string.Empty)
            .ToLowerInvariant();

        return normalized switch
        {
            "clientes" => "Cliente",
            "cliente" => "Cliente",

            "categorias" => "Categoria",
            "categoria" => "Categoria",

            "produtos" => "Produto",
            "produto" => "Produto",

            "pedidos" => "Pedido",
            "pedido" => "Pedido",

            "pedidosprodutos" => "PedidoProduto",
            "pedidoproduto" => "PedidoProduto",

            _ => identifier
        };
    }

    public string Pluralize(string identifier)
    {
        return identifier switch
        {
            "Cliente" => "Clientes",
            "Categoria" => "Categorias",
            "Produto" => "Produtos",
            "Pedido" => "Pedidos",
            "PedidoProduto" => "PedidosProdutos",

            _ => identifier
        };
    }
}
