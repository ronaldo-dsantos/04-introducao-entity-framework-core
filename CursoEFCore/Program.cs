using CursoEFCore.Domain;
using CursoEFCore.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CursoEFCore;

class Program
{
  static void Main(string[] args)
  {
    using var db = new Data.ApplicationContext();
    //db.Database.Migrate();

    /*
    var migracaoPendente = db.Database.GetPendingMigrations().Any(); // GetPendingMigrations() para verificar se há migrações pendentes | Any() para transformar em booleano 

    if (migracaoPendente)
    {
      // regra de negócio se houver migraçoes pendentes
    }
    */

    //InserirDados();
    InserirDadosEmMassa();
  }

  private static void InserirDadosEmMassa()
  {
    var produto = new Produto
    {
      Descricao = "Produto Teste",
      CodigoBarras = "12345678910",
      Valor = 10m,
      TipoProduto = TipoProduto.MercadoriaParaRevenda,
      Ativo = true
    };

    var cliente = new Cliente
    {
      Nome = "Ronaldo Domingues",
      CEP = "04855140",
      Cidade = "Sao Paulo",
      Estado = "SP",
      Telefone = "11980726842"
    };

    using var db = new Data.ApplicationContext();
    db.AddRange(produto, cliente);

    var registros = db.SaveChanges();
    Console.WriteLine($"Total Registro(s): {registros}");

    // Outra maneira de inserir dados em massa usando uma lista
    /*
    var listaClientes = new[]
    {
      new Cliente
      {
        Nome = "Teste 01",
        CEP = "0000000",
        Cidade = "Sao Paulo",
        Estado = "SP",
        Telefone = "11123456789"
      },

      new Cliente
      {
        Nome = "Teste 02",
        CEP = "0000000",
        Cidade = "Sao Paulo",
        Estado = "SP",
        Telefone = "11123456789"
      },
    };

    using var db = new Data.ApplicationContext();
    db.AddRange(listaClientes);
    //db.Set<Cliente>().AddRange(listaClientes); // Essa é mais uma das opções de inserir registros em nossa base de dados

    var registros = db.SaveChanges();
    Console.WriteLine($"Total Registro(s): {registros}");
    */
  }

  private static void InserirDados()
  {
    var produto = new Produto
    {
      Descricao = "Produto Teste",
      CodigoBarras = "12345678910",
      Valor = 10m,
      TipoProduto = TipoProduto.MercadoriaParaRevenda,
      Ativo = true
    };

    // Instanciando o banco de dados
    using var db = new Data.ApplicationContext();

    // Adicionando registros em nosso banco de dados
    db.Produtos.Add(produto); // Essa é uma das opções de inserir registros em nossa base de dados (recomendada)
    //db.Set<Produto>().Add(produto); // Essa é mais uma das opções de inserir registros em nossa base de dados (recomendada)
    //db.Entry(produto).State = EntityState.Added; // Essa é mais uma das opções de inserir registros em nossa base de dados
    //db.Add(produto); // Essa é mais uma das opções de inserir registros em nossa base de dados

    // Salvando os registros em nosso banco de dados
    var registros = db.SaveChanges(); // SaveChanges() retorna a quantidade de regitros afetados em nossa base de dados

    Console.WriteLine($"Total Registro(s): {registros}");
  }
}
