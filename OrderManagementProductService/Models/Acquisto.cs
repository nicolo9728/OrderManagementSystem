using OrderManagementCommon.Models;
using OrderManagementProductService.Models;

namespace OrderManagementProductService.Models;

public record IdAcquisto(Guid Id);

public class Acquisto
{
    public IdAcquisto Id { get; } = new IdAcquisto(Guid.NewGuid());
    public DateTime Momento { get; } = DateTime.Now.ToUniversalTime();
    public IdUtente IdUtente { get; }
    public IdProdotto IdProdotto { get; }
    public Quantita QuantitaAcquistata { get; }

    public Acquisto(IdUtente idUtente, IdProdotto idProdotto, Quantita acquistata)
    {
        IdUtente = idUtente;
        IdProdotto = idProdotto;
        QuantitaAcquistata = acquistata;
    }

    private Acquisto()
    {
        IdUtente = null!;
        IdProdotto = null!;
        QuantitaAcquistata = null!;
    }
}