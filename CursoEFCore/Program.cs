using CursoEFCore.Domain;
using CursoEFCore.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CursoEFCore;

class Program
{
  static void Main(string[] args)
  {
    using var db = new Data.ApplicationContext();
    //db.Database.Migrate(); // Esta é uma das maneiras de aplicar as migrações ao BD (não indicada para ambiente de produção, caso tenha outras aplicação utilizando seu BD pode gerar conflitos), outra maneira é via prompt de comando

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
    //ConsultarPedidoComCarregamentoAdiantado();
    //AtualizarDados();
    RemoverRegistro();
  }

  private static void RemoverRegistro()
  {
    
    using var db = new Data.ApplicationContext(); // instanciando o banco de dados

    var cliente = db.Clientes.Find(2); // localizando o cliente que vamos remover em nosso banco de dados | Find() busca por padrão a chave primária, por isso não precisamos informar qual campo estamos pesquisando 

    //db.Clientes.Remove(cliente); // uma das maneiras de se remover um cliente do banco de dados
    db.Remove(cliente); // outra maneira de se remover um cliente do banco de dados, usando a instancia que já consta no objeto cliente
    //db.Entry(cliente).State = EntityState.Deleted; // outra maneira de se remover um cliente do banco de dados, alterando o estado do objeto para deleted

    db.SaveChanges(); // salvando as alterações em nosso banco de dados
    
    /*
    // Cenário Desconectado (cenário que o dados não foram instanciados ainda, exemplo um frontend que manda os dados para que uma api possa tratá-los e acessar o bd)
    using var db = new Data.ApplicationContext(); // instanciando o banco de dados

    var cliente = new Cliente { Id = 3 }; // informando de forma explicita qual cliente iremos remover

    db.Entry(cliente).State = EntityState.Deleted; // alterando o estado do objeto para deleted

    db.SaveChanges(); // salvando as alteração no banco de dados
    */
  }

  private static void AtualizarDados()
  {
    using var db = new Data.ApplicationContext(); // instanciando do banco de dados
    var cliente = db.Clientes.FirstOrDefault(p => p.Id == 1); // buscando na tabela clientes o cliente com o id = 1

    cliente.Nome = "Nome Alterado Passo 2"; // alterando o objeto cliente

    //db.Clientes.Update(cliente); // enviando a alteração para o banco de dados | se utilizarmos essa linha de código com o update, ele vai atualizar todos os dados de um cliente, com isso, podemos usar apenas o SaveChanges() para atualizar no bd somente o dado que realmente foi alterado
    //db.Entry(cliente).State = EntityState.Modified; // essa é uma segunda opção de informar de maneira explicita para o entity framework alterar apenas o estado que foi modificado
    db.SaveChanges(); // salvando as alterações no banco de dados

    /*
      // Cenário Desconectado (cenário que o dados não foram instanciados ainda, exemplo um frontend que manda os dados para que uma api possa tratá-los e acessar o bd)
      using var db = new Data.ApplicationContext();
      
      //var cliente = db.Clientes.Find(1); // uma das maneiras de trabalhar com o cliente desconectado é consultando ele no banco de dados

      var cliente = new Cliente // outra maneira é informar explicitamente o cliente que vamos fazer a alteração e usar o attach para começar a rastrear esse objeto
      {
        Id = 1
      };

      db.Attach(cliente); // Attach atachar o objeto para que ele comece a ser rastreado internamente
      
      var clienteDesconetado = new // criando um objeto anonimo 
      {
        Nome = "Cliente Desconectado 03",
        Telefone = "99980726843"
      };
      
      db.Entry(cliente).CurrentValues.SetValues(clienteDesconetado);

      db.SaveChanges();
    */
  }

  private static void ConsultarPedidoComCarregamentoAdiantado()
  {
    // Carregamento adiantado significa que os dados relacionados (relacionamento) serão carregados do banco de dados em uma única consulta
    using var db = new Data.ApplicationContext(); // instanciando o banco de dados

    var pedidos = db // consultando todos os pedidos existentes no banco de dados
    .Pedidos
    .Include(p => p.Itens) // Include(p=>p.Itens) para utilizar o carregamento adiantado da tabela itens | Include para relacionamentos que estão no primeiro nível 
      .ThenInclude(p => p.Produto) // ThenInclude(p=>p.Produto) para utilizar o carregamento adiantado da tabela Produto | ThenInclude para relacionamentos que estão no primeiro nível, nesse caso é um relacionamento da tabela itens e não da tabela pedidos 
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

  private static void ConsultarDados()
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
      db.Clientes.Find(cliente.Id); // Find() vai realizar a consulta primeiramente na memória, como a busca já foi realizada acima, ele não precisa ir no bd para percorrer cada instância, útil por questões de performance dependendo da regra de negócio
      //db.Clientes.FirstOrDefault(p => p.Id == cliente.Id); // FirstOrDefault() vai realizar a consulta diretamente no banco de dados, ao contrário do Find()
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

    // Outra maneira de inserir dados em massa é utilizando uma lista
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
    // Intanciando um produto e atribuindo valores
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
