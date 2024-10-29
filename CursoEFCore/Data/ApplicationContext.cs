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
           p => p.EnableRetryOnFailure(
               maxRetryCount: 2,
               maxRetryDelay: TimeSpan.FromSeconds(5),
               errorNumbersToAdd: null).MigrationsHistoryTable("curso_ef_core"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
      MapearPropriedadesEsquecidas(modelBuilder);
    }

    private void MapearPropriedadesEsquecidas(ModelBuilder modelBuilder)
    {
      foreach (var entity in modelBuilder.Model.GetEntityTypes())
      {
        var properties = entity.GetProperties().Where(p => p.ClrType == typeof(string));

        foreach (var property in properties)
        {
          if (string.IsNullOrEmpty(property.GetColumnType())
              && !property.GetMaxLength().HasValue)
          {
            //property.SetMaxLength(100);
            property.SetColumnType("VARCHAR(100)");
          }
        }
      }
    }
  }
}