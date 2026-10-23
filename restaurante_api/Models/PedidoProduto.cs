using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace restaurante_api.Models;

[Table("pedidos_produtos")]
public partial class PedidoProduto
{
    [Key]
    [Column("pedido_produto_id")]
    public int PedidoProdutoId { get; set; }

    [Column("pedido_id")]
    public int PedidoId { get; set; }

    [Column("produto_id")]
    public int ProdutoId { get; set; }

    [Column("pedido_produto_quantidade")]
    public int PedidoProdutoQuantidade { get; set; }

    [Column(
        "pedido_produto_preco_unitario",
        TypeName = "decimal(10, 2)")]
    public decimal PedidoProdutoPrecoUnitario { get; set; }

    [JsonIgnore]
public virtual Pedido Pedido { get; set; } = null!;

[JsonIgnore]
public virtual Produto Produto { get; set; } = null!;
}