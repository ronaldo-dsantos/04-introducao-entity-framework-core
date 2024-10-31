using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CursoEFCore.Domain
{
  [Table("Clientes")] // nome da tabela, também podemos usar este recurso para o EF Core mapear quando o nome estiver diferente no bd
  public class Cliente
  {
    // Exemplos de mapeamento de dados usando o Data Annotations, mas recomenda-se fazer o mapeamento de dados através do Fluent API por ser muito mais rico
    [Key] // chave primária 
    public int Id { get; set; }
    [Required] // campo que não pode ser nulo
    public string Nome { get; set; }
    [Column("Phone")] // nome do campo, também podemos usar este recurso para o EF Core mapear quando o nome estiver diferente no bd
    public string Telefone { get; set; }
    public string CEP { get; set; }
    public string Estado { get; set; }
    public string Cidade { get; set; }
  }
} 