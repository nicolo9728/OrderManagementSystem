namespace OrderManagementViewmodels.Ordini;

public record OrderViewModel(Guid Id, string Indirizzo, string Status, DateTime MomentoAcquisto, Guid IdProdotto, int Quantita);