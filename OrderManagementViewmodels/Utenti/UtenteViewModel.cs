namespace OrderManagementViewmodels.Utenti;

public record UtenteLoggatoViewModel(Guid Id, string Username, string Ruolo);

public record UtenteViewModel(Guid Id, string Username, string Ruolo, string Nome, string Cognome);