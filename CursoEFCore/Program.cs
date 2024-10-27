using Microsoft.EntityFrameworkCore;

namespace CursoEFCore;

class Program
{
  static void Main(string[] args)
  {
    using var db = new Data.ApplicationContext();
    //db.Database.Migrate();

    var migracaoPendente = db.Database.GetPendingMigrations().Any(); // GetPendingMigrations() para verificar se há migrações pendentes | Any() para transformar em booleano 

    if (migracaoPendente)
    {
      // refra de negócio se houver migracao pendente
    }
  }
}
