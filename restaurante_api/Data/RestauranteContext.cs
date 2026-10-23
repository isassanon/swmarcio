using Microsoft.EntityFrameworkCore;
using restaurante_api.Models;

namespace restaurante_api.Data;

public partial class RestauranteContext : DbContext
{
    public RestauranteContext(DbContextOptions<RestauranteContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Produto> Produtos { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<PedidoProduto> PedidosProdutos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.ClienteId)
                .HasName("PRIMARY");

            entity.ToTable("clientes");

            entity.HasIndex(e => e.ClienteEmail)
                .IsUnique()
                .HasDatabaseName("cliente_email");

            entity.Property(e => e.ClienteId)
                .HasColumnName("cliente_id");

            entity.Property(e => e.ClienteNome)
                .HasMaxLength(100)
                .HasColumnName("cliente_nome");

            entity.Property(e => e.ClienteTelefone)
                .HasMaxLength(20)
                .HasColumnName("cliente_telefone");

            entity.Property(e => e.ClienteEmail)
                .HasMaxLength(120)
                .HasColumnName("cliente_email");
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.CategoriaId)
                .HasName("PRIMARY");

            entity.ToTable("categorias");

            entity.HasIndex(e => e.CategoriaNome)
                .IsUnique()
                .HasDatabaseName("categoria_nome");

            entity.Property(e => e.CategoriaId)
                .HasColumnName("categoria_id");

            entity.Property(e => e.CategoriaNome)
                .HasMaxLength(60)
                .HasColumnName("categoria_nome");

            entity.Property(e => e.CategoriaDescricao)
                .HasMaxLength(200)
                .HasColumnName("categoria_descricao");
        });

        modelBuilder.Entity<Produto>(entity =>
        {
            entity.HasKey(e => e.ProdutoId)
                .HasName("PRIMARY");

            entity.ToTable("produtos");

            entity.Property(e => e.ProdutoId)
                .HasColumnName("produto_id");

            entity.Property(e => e.CategoriaId)
                .HasColumnName("categoria_id");

            entity.Property(e => e.ProdutoNome)
                .HasMaxLength(100)
                .HasColumnName("produto_nome");

            entity.Property(e => e.ProdutoPreco)
                .HasPrecision(10, 2)
                .HasColumnName("produto_preco");

            entity.HasOne(d => d.Categoria)
                .WithMany(p => p.Produtos)
                .HasForeignKey(d => d.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("produtos_ibfk_1");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.PedidoId)
                .HasName("PRIMARY");

            entity.ToTable("pedidos");

            entity.Property(e => e.PedidoId)
                .HasColumnName("pedido_id");

            entity.Property(e => e.ClienteId)
                .HasColumnName("cliente_id");

            entity.Property(e => e.PedidoData)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("pedido_data");

            entity.Property(e => e.PedidoStatus)
                .HasMaxLength(10)
                .HasColumnName("pedido_status");

            entity.HasOne(d => d.Cliente)
                .WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("pedidos_ibfk_1");
        });

        modelBuilder.Entity<PedidoProduto>(entity =>
        {
            entity.HasKey(e => e.PedidoProdutoId)
                .HasName("PRIMARY");

            entity.ToTable("pedidos_produtos");

            entity.HasIndex(e => new { e.PedidoId, e.ProdutoId })
                .IsUnique()
                .HasDatabaseName("pedido_id_produto_id");

            entity.Property(e => e.PedidoProdutoId)
                .HasColumnName("pedido_produto_id");

            entity.Property(e => e.PedidoId)
                .HasColumnName("pedido_id");

            entity.Property(e => e.ProdutoId)
                .HasColumnName("produto_id");

            entity.Property(e => e.PedidoProdutoQuantidade)
                .HasColumnName("pedido_produto_quantidade");

            entity.Property(e => e.PedidoProdutoPrecoUnitario)
                .HasPrecision(10, 2)
                .HasColumnName("pedido_produto_preco_unitario");

            entity.HasOne(d => d.Pedido)
                .WithMany(p => p.PedidosProdutos)
                .HasForeignKey(d => d.PedidoId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("pedidos_produtos_ibfk_1");

            entity.HasOne(d => d.Produto)
                .WithMany(p => p.PedidosProdutos)
                .HasForeignKey(d => d.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("pedidos_produtos_ibfk_2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}