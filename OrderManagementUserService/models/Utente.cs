using OrderManagementCommon.Events;
using OrderManagementCommon.Models;

namespace OrderManagementUserService.Models;

public record Password(string Value);
public record Credenziali(string Username, Password Password);
public record Generalita(string Nome, string Cognome);

public abstract class Utente : AggregateRoot
{

    public Utente(Credenziali credenziali, Generalita generalita, IdUtente idUtente)
        => (Credenziali, Generalita, Id) = (credenziali, generalita, idUtente);

    protected Utente()
    {
        Credenziali = null!;
        Generalita = null!;
        Id = null!;
    }
    public IdUtente Id { get; private set; }
    public Credenziali Credenziali { get; }
    public Generalita Generalita { get; }

    private Guid IdRaw
    {
        get => Id.Id;
        set => Id = IdUtente.GetIdUtenteFromGuidAndRole(value, this.GetType().Name);
    }
}

public class Admin : Utente
{
    public Admin(Credenziali credenziali, Generalita generalita) : base(credenziali, generalita, new IdAdmin(Guid.NewGuid())) { }
    private Admin() { }
}
public class Customer : Utente
{
    public Customer(Credenziali credenziali, Generalita generalita) : base(credenziali, generalita, new IdCustomer(Guid.NewGuid())) { }
    private Customer() { }
}
public class DeliveryGuy : Utente
{
    public DeliveryGuy(Credenziali credenziali, Generalita generalita) : base(credenziali, generalita, new IdDeliveryGuy(Guid.NewGuid()))
    {
        AddDomainEvent(new DeliveryGuyCreatoEvent((IdDeliveryGuy)Id));
    }
    private DeliveryGuy() { }
}