using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace restaurante_api.Models;

[Table("produtos")]
public partial class Produto
{
    [Key]
    [Column("produto_id")]
    public int ProdutoId { get; set; }

    [Column("categoria_id")]
    public int CategoriaId { get; set; }

    [Required]
    [StringLength(100)]
    [Column("produto_nome")]
    public string ProdutoNome { get; set; } = null!;

    [Column("produto_preco", TypeName = "decimal(10, 2)")]
    public decimal ProdutoPreco { get; set; }

    [JsonIgnore]
public virtual Categoria Categoria { get; set; } = null!;

[JsonIgnore]
public virtual ICollection<PedidoProduto> PedidosProdutos { get; set; }
    = new List<PedidoProduto>();
}