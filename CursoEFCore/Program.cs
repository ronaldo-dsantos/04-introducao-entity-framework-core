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
    //InserirDadosEmMassa();
    //ConsultarDados();
    //CadastrarPedido();

    ConsultarPedidoComCarregamentoAdiantado();
  }

  private static void ConsultarPedidoComCarregamentoAdiantado()
  {
    using var db = new Data.ApplicationContext(); // instanciando o banco de dados

    var pedidos = db // consultando todos os pedidos existentes no banco de dados
    .Pedidos
    .Include(p=>p.Itens) // Include(p=>p.Itens) para utilizar o carregamento adiantado da tabela itens | Include para relacionamentos que estão no primeiro nível 
      .ThenInclude(p=>p.Produto) // ThenInclude(p=>p.Produto) para utilizar o carregamento adiantado da tabela Produto | ThenInclude para relacionamentos que estão no primeiro nível, nesse caso é um relacionamento da tabela itens e não da tabela pedidos 
    .ToList();  

    Console.WriteLine(pedidos.Count()); // exibindo a quantidade de pedidos
  }

  private static void CadastrarPedido()
  {
    using var db = new Data.ApplicationContext(); // instanciando o banco de dados

    var cliente = db.Clientes.FirstOrDefault(); // fazendo uma consulta do primeiro cliente no banco de dados
    var produto = db.Produtos.FirstOrDefault(); // fazendo uma consulta do primeiro produto no banco de dados

    var pedido = new Pedido // criando um pedido com os dados do cliente consultado no banco de dados
    {
      ClienteId = cliente.Id,
      IniciadoEm = DateTime.Now,
      FinalizadoEm = DateTime.Now,
      Observacao = "Pedido Teste",
      Status = StatusPedido.Analise,
      TipoFrete = TipoFrete.SemFrete,
      Itens = new List<PedidoItem>
      {
        new PedidoItem // criando um pedido item com os dados do produto consultado no banco de dados
        {
          ProdutoId = produto.Id,
          Desconto = 0,
          Quantidade = 1,
          Valor = 10,
        }
      }
    };

    db.Pedidos.Add(pedido); // adicionando o pedido no banco de dados
    db.SaveChanges(); // salvando as alterações no banco de dados
  }

  public static void ConsultarDados()
  {
    using var db = new Data.ApplicationContext();

    //var consultaPorSintaxe = (from c in db.Clientes where c.Id > 0 select c).ToList();
    var consultaPorMetodo = db.Clientes
    .Where(p => p.Id > 0)
    .OrderBy(p => p.Id)
    .ToList();

    foreach (var cliente in consultaPorMetodo)
    {
      Console.WriteLine($"Consultado Cliente: {cliente.Id}");
      db.Clientes.FirstOrDefault(p => p.Id == cliente.Id);
    }

    /*
    var consultaPorMetodo = db.Clientes.AsNoTracking().Where(p => p.Id > 0).ToList(); // AsNoTracking() para que o entity framework não rastreie esse resultado na memória, obrigando a buscar as informações no banco de dados, útil dependendo da regra de negócio 

    foreach (var cliente in consultaPorMetodo)
    {
      Console.WriteLine($"Consultado Cliente: {cliente.Id}");
      db.Clientes.Find(cliente.Id); // Find() vai realizar a consulta primeiramente na memória, se não encontrar, aí sim busca no banco de dados 
    }
    */
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
