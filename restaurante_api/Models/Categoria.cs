using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace restaurante_api.Models;

[Table("categorias")]
public partial class Categoria
{
    [Key]
    [Column("categoria_id")]
    public int CategoriaId { get; set; }

    [Required]
    [StringLength(60)]
    [Column("categoria_nome")]
    public string CategoriaNome { get; set; } = null!;

    [StringLength(200)]
    [Column("categoria_descricao")]
    public string? CategoriaDescricao { get; set; }

    [JsonIgnore]
public virtual ICollection<Produto> Produtos { get; set; }
    = new List<Produto>();
}