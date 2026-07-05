using OrderManagementCommon.Models;

namespace OrderManagementUserService.Models;

public record Password(string Value);
public record Credenziali(string Username, Password Password);
public record Generalita(string Nome, string Cognome);

public abstract class Utente
{

    public Utente(Credenziali credenziali, Generalita generalita)
        => (Credenziali, Generalita) = (credenziali, generalita);

    protected Utente()
    {
        Credenziali = null!;
        Generalita = null!;
    }
    public IdUtente Id { get; } = new IdUtente(Guid.NewGuid());
    public Credenziali Credenziali { get; }
    public Generalita Generalita { get; }
}

public class Admin : Utente
{
    public Admin(Credenziali credenziali, Generalita generalita): base(credenziali, generalita) {}
    private Admin(){}
}
public class Customer : Utente
{
    public Customer(Credenziali credenziali, Generalita generalita): base(credenziali, generalita){ }
    private Customer(){}
}
public class DeliveryGuy : Utente
{
    public DeliveryGuy(Credenziali credenziali, Generalita generalita): base(credenziali, generalita){ }
    private DeliveryGuy(){}
}