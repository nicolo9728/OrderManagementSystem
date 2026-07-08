using System.Text.Json;

namespace OrderManagementCommon.Events;

public record IdEvento(Guid Id);

public class DomainEventRow
{
    public IdEvento Id { get; }
    public string Tipo { get; }
    public string Contenuto { get; }
    public DateTime MomentoCreazione { get; }
    public bool IsCompletato { get; private set; }
    public DomainEventRow(string tipo, string contenuto)
    {
        Id = new IdEvento(Guid.NewGuid());
        Tipo = tipo;
        Contenuto = contenuto;
        MomentoCreazione = DateTime.Now.ToUniversalTime();
    }

    public void MarcaCompletato()
        => IsCompletato = true;

    private DomainEventRow()
    {
        Id = null!;
        Tipo = null!;
        Contenuto = null!;
    }

    public static DomainEventRow FromEvent(DomainEvent e)
        => new(e.GetType().Name, JsonSerializer.Serialize(e, e.GetType()));

    public DomainEvent? ToDomainEvent()
        => JsonSerializer.Deserialize(Contenuto, typeof(DomainEvent).Assembly.GetType(Tipo)!) as DomainEvent;
}