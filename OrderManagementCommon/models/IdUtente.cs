namespace OrderManagementCommon.Models;

public abstract record IdUtente(Guid Id);
public record IdUtenteEmpty() : IdUtente(Guid.Empty);

public record IdAdmin(Guid Id): IdUtente(Id);
public record IdCustomer(Guid Id): IdUtente(Id);
public record IdDeliveryGuy(Guid Id): IdUtente(Id);

public static class IdUtenteConversions
{
    extension(IdUtente id)
    {
        public static IdUtente GetIdUtenteFromGuidAndRole(Guid guid, string role)
            => role switch
            {
                "Admin" => new IdAdmin(guid),
                "Customer" => new IdCustomer(guid),
                "DeliveryGuy" => new IdDeliveryGuy(guid),
                _ => throw new Exception()
            };
    }
}