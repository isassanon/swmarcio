using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace restaurante_api.Models;

[Table("clientes")]
public partial class Cliente
{
    [Key]
    [Column("cliente_id")]
    public int ClienteId { get; set; }

    [Required]
    [StringLength(100)]
    [Column("cliente_nome")]
    public string ClienteNome { get; set; } = null!;

    [Required]
    [StringLength(20)]
    [Column("cliente_telefone")]
    public string ClienteTelefone { get; set; } = null!;

    [Required]
    [StringLength(120)]
    [Column("cliente_email")]
    public string ClienteEmail { get; set; } = null!;

    [JsonIgnore]
public virtual ICollection<Pedido> Pedidos { get; set; }
    = new List<Pedido>();
}