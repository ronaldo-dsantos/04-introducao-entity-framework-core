using CursoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CursoEFCore.Data.Configurations
{
  public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
  {
    public void Configure(EntityTypeBuilder<Pedido> builder)
    {
      builder.ToTable("Pedidos");
      builder.HasKey(p => p.Id);
      builder.Property(p => p.IniciadoEm).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd(); // HasDefaultValueSql informando de maneira explicita um comando SQL que desejamos executar
      builder.Property(p => p.Status).HasConversion<string>();
      builder.Property(p => p.TipoFrete).HasConversion<int>();
      builder.Property(p => p.Observacao).HasColumnType("VARCHAR(512)");

      builder.HasMany(p => p.Itens) // relacionamento muitos para um
        .WithOne(p => p.Pedido)
        .OnDelete(DeleteBehavior.Cascade); // deletar em cascata
    }
  }
}