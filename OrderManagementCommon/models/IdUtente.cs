namespace OrderManagementCommon.Models;

public record IdUtente(Guid Id);
public record IdUtenteEmpty() : IdUtente(Guid.Empty);
