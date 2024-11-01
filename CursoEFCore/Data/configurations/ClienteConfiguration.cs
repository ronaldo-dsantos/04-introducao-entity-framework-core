using CursoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CursoEFCore.Data.Configurations
{
  // mapeamento de dados através do Fluent API
  public class ClienteConfiguration : IEntityTypeConfiguration<Cliente> // criando a classe ClienteConfiguration herdando a interface generica IEntityTypeConfiguration com o nome da entidade que vamos configurar
  {
    public void Configure(EntityTypeBuilder<Cliente> builder) // implementando a interface
    {      
      builder.ToTable("Clientes"); // nome da tabela
      builder.HasKey(p => p.Id); // chave primária
      builder.Property(p => p.Nome).HasColumnType("VARCHAR(80)").IsRequired(); // propriedades, tipo de dados e não pode ser nulo
      builder.Property(p => p.Telefone).HasColumnType("CHAR(11)");
      builder.Property(p => p.CEP).HasColumnType("CHAR(8)").IsRequired();
      builder.Property(p => p.Estado).HasColumnType("CHAR(2)").IsRequired();
      builder.Property(p => p.Cidade).HasMaxLength(60).IsRequired();

      builder.HasIndex(i => i.Telefone).HasDatabaseName("idx_cliente_telefone"); // indice para o campo telefone 
    }
  }
}