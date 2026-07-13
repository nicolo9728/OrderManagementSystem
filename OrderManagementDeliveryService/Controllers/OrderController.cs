using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementCommon.Models;
using OrderManagementDeliveryService.Database;
using OrderManagementDeliveryService.Models;

namespace OrderManagementDeliveryService.Controllers;

public record OrdiceCancellatoForm(string Ragione);

[Controller]
[Route("[controller]")]
public class OrderController(DeliveryServiceDbContext context, IdUtente idUtente): ControllerBase
{

    [HttpPost("{id}/Consegna")]
    [Authorize(Roles = "DeliveryGuy")]
    public async Task<IActionResult> ConsegnaOrdine(Guid id)
    {
        IdOrder idOrder = new(id);
        Order? orderDaConsegnare = await context.Ordini.Where((o)=>o.Id == idOrder).FirstOrDefaultAsync();

        if(orderDaConsegnare == null)
            return NotFound();
        
        if(!orderDaConsegnare.IsDeliveryGuyAssignedToThisOrder((IdDeliveryGuy)idUtente))
            return Forbid();
        
        orderDaConsegnare.TryDelivery(DateTime.Now);

        await context.SaveChangesAsync();

        if(orderDaConsegnare.Status is not OrderDelivered)
            return Problem("Impossibile effettuare la consegna dell'ordine");
        
        return Ok();
    }

    [HttpPost("{id}/Cancella")]
    [Authorize(Roles = "DeliveryGuy")]
    public async Task<IActionResult> CancellaOrdine(Guid id, OrdiceCancellatoForm form)
    {
        IdOrder idOrder = new(id);
        Order? orderDaConsegnare = await context.Ordini.Where((o)=>o.Id == idOrder).FirstOrDefaultAsync();

        if(orderDaConsegnare == null)
            return NotFound();
        
        if(!orderDaConsegnare.IsDeliveryGuyAssignedToThisOrder((IdDeliveryGuy)idUtente))
            return Forbid();
        
        orderDaConsegnare.TryCancel(DateTime.Now.ToUniversalTime(), form.Ragione);

        await context.SaveChangesAsync();

        if(orderDaConsegnare.Status is not OrderCanceled)
            return Problem("Impossibile cancellare l'ordine");
        
        return Ok();
    }
}