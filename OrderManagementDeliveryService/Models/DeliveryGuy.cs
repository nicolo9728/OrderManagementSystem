using OrderManagementCommon.Models;

namespace OrderManagementDeliveryService.Models;


public class DeliveryGuy : AggregateRoot
{
    public IdDeliveryGuy Id { get; private set; }
    public int NumeroConsegneAttive { get; private set; }

    public DeliveryGuy(IdDeliveryGuy id)
        => Id = id;
    
    private DeliveryGuy()
        => Id = null!;

    public void SegnalaNuovaConsegna()
        => NumeroConsegneAttive += 1;
    
    public void SegnalaConsegnaConclusa()
    {
        if(NumeroConsegneAttive - 1 < 0)
            throw new Exception("Il numero di consegne attive non puo superare lo zero");
        
        NumeroConsegneAttive -= 1;
    }
}