namespace CursoEFCore.Domain
{
  public class PedidoItem
  {
    public int Id { get; set; }
    public int PedidoId { get; set; }
    public Pedido Pedido { get; set; } // utilizando a propriedade de navegação Pedido (relacionamento)
    public int ProdutoId { get; set; }
    public Produto Produto { get; set; } // utilizando a propriedade de navegação Produto 
    public int Quantidade { get; set; }
    public decimal Valor { get; set; }
    public decimal Desconto { get; set; }
  }
}