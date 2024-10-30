using CursoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CursoEFCore.Data
{
  public class ApplicationContext : DbContext
  {
    private static readonly ILoggerFactory _logger = LoggerFactory.Create(p => p.AddConsole()); // Criando a instancia do log que iremos utilizar

    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Cliente> Clientes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder
        .UseLoggerFactory(_logger) // Informando qual log iremos utilizar
        .EnableSensitiveDataLogging() // habilitando o método de extensão para que possamos visualizar os dados sensiveis
        .UseSqlServer("Data source=(localdb)\\mssqllocaldb;Initial Catalog=CursoEFCore;Integrated Security=true",
          p => p.EnableRetryOnFailure( // habilitando o método Retry para os casos de falha de conexão, por padrão em caso de falhas ele tenta se conectar por 6 vezes em um minuto, mas podemos alterar esse padrão
            maxRetryCount: 2, // alterando o número de tentativas para 2
            maxRetryDelay: TimeSpan.FromSeconds(5), // alterando o tempo de delay entre uma tentativa e outra para 5 segundos
            errorNumbersToAdd: null) // podemos informar errorNumbersToAdd quais os códigos dos erros adicionas que desejamos que seja interpredado, nesse caso usamos o null para nenhum adicional e manter o padrão
            .MigrationsHistoryTable("curso_ef_core")); // alterando o nome padrão da tabela de historico de migrações
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
      MapearPropriedadesEsquecidas(modelBuilder); // chamando o métodos de propriedades esquecidas
    }

    // Detectando propridades não configuradas (método para que possamos configurar propriedades que eventualmente podemos esquecido de configurar em nossa aplicação, neste exemplo vamos verificar as do tipo string)
    private void MapearPropriedadesEsquecidas(ModelBuilder modelBuilder) // criando o método e recebendo a propridade modelbuilder por parametro
    {
      foreach (var entity in modelBuilder.Model.GetEntityTypes()) // percorrendo a lista das entidades configuradas em nossa aplicação
      {
        var properties = entity.GetProperties().Where(p => p.ClrType == typeof(string)); // carregando todas as propriedades dessa entidade que sejam do tipo string, mas podemos alterar a estratégia para outros tipos também

        foreach (var property in properties) // percorrendo a lista de propriedades
        {
          if (string.IsNullOrEmpty(property.GetColumnType()) // verificando se o tipo da coluna está vazio
              && !property.GetMaxLength().HasValue) // veriticando se o tamanho da propriedade foi informado
          {
            //property.SetMaxLength(100); // informando o tamanho para o campo desejado
            property.SetColumnType("VARCHAR(100)"); // informando o tipo e tamanho para o campo desejado
          }
        }
      }
    }
  }
}