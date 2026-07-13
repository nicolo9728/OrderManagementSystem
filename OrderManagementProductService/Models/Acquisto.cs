using OrderManagementCommon.Events;
using OrderManagementCommon.Models;
using OrderManagementProductService.Models;

namespace OrderManagementProductService.Models;

public record IdAcquisto(Guid Id);


public class Acquisto : AggregateRoot
{
    public IdAcquisto Id { get; } = new IdAcquisto(Guid.NewGuid());
    public DateTime Momento { get; } = DateTime.Now.ToUniversalTime();
    public IdCustomer IdUtente { get; }
    public IdProdotto IdProdotto { get; }
    public Quantita QuantitaAcquistata { get; }

    public Acquisto(IdCustomer idUtente, IdProdotto idProdotto, Quantita acquistata, Indirizzo IndirizzoCorrente)
    {
        IdUtente = idUtente;
        IdProdotto = idProdotto;
        QuantitaAcquistata = acquistata;
        
        AddDomainEvent(new AcquistoCreatoEvent(idProdotto, idUtente, acquistata, Momento, IndirizzoCorrente));
    }

    private Acquisto()
    {
        IdUtente = null!;
        IdProdotto = null!;
        QuantitaAcquistata = null!;
    }
}