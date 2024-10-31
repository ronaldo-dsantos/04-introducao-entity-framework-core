using CursoEFCore.ValueObjects;

namespace CursoEFCore.Domain
{
  public class Pedido
  {
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; } // utilizando a propriedade de navegação Cliente
    public DateTime IniciadoEm { get; set; }
    public DateTime FinalizadoEm { get; set; }
    public TipoFrete TipoFrete { get; set; } // utilizando o enum TipoFrete
    public StatusPedido Status { get; set; } // utilizando o enum StatusPedido
    public string Observacao { get; set; }
    public ICollection<PedidoItem> Itens { get; set; } // utilizando a propriedade de navegação PedidoItem em uma coleção
  }
}