using OrderManagementCommon.Models;

namespace OrderManagementProductService.Models;



public class Prodotto
{
    public IdProdotto Codice { get; }
    public string Nome { get; }
    public Quantita QuantitaDisponibile { get; private set; }
    
    public Prodotto(string nome, Quantita quantitaDisponibile)
        =>(Codice, Nome, QuantitaDisponibile) = (new IdProdotto(Guid.NewGuid()), nome, quantitaDisponibile);


    public bool IsDisponibile(Quantita quantita)
        => QuantitaDisponibile.IsDisponibile(quantita);

    public void RiduciScorta(Quantita quantitaDaRidurre)
    {
        if (!QuantitaDisponibile.IsDisponibile(quantitaDaRidurre))
            throw new Exception("Prodotto esaurito");

        QuantitaDisponibile = QuantitaDisponibile.RiduciQuantitaDi(quantitaDaRidurre);
    }
}

