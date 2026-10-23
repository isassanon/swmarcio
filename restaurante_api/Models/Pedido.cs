using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace restaurante_api.Models;

[Table("pedidos")]
public partial class Pedido
{
    [Key]
    [Column("pedido_id")]
    public int PedidoId { get; set; }

    [Column("cliente_id")]
    public int ClienteId { get; set; }

    [Column("pedido_data", TypeName = "datetime")]
    public DateTime PedidoData { get; set; }

    [Required]
    [StringLength(10)]
    [Column("pedido_status")]
    public string PedidoStatus { get; set; } = "aberto";

    [JsonIgnore]
public virtual Cliente Cliente { get; set; } = null!;

[JsonIgnore]
public virtual ICollection<PedidoProduto> PedidosProdutos { get; set; }
    = new List<PedidoProduto>();
}