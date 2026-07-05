namespace OrderManagementCommon.Models;

public record Quantita
{
    public Quantita(int valore)
    {
        Valore = valore >= 0 ? valore : throw new ArgumentException("Il valore non puo essere negativo");
    }

    public Quantita RiduciQuantitaDi(Quantita quantita)
        => new(Valore - quantita.Valore);

    public bool IsDisponibile(Quantita quantita)
        => Valore - quantita.Valore >= 0;

    public int Valore { get; }
}