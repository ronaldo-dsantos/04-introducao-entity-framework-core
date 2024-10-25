using Microsoft.EntityFrameworkCore;

namespace CursoEFCore.Data
{
  class ApplicationContext : DbContext
  {
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlServer("Data source=(localdb)\\mssqllocaldb;Initial Catalog=CursoEFCore;Integrated Security=true");
    }
  }
}