using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementCommon.Database;
using OrderManagementCommon.Models;
using OrderManagementProductService.Models;
using OrderManagementViewmodels.Prodotti;

namespace OrderManagementProductService.Controllers;


public record ProdottoInserireForm(string Nome, int QuantitaDisponibile);
public record AcquistaProdottoForm(int Quantita);

[ApiController]
[Route("[controller]")]
public class ProdottiController(ProductServiceDbContext context, IdUtente idUtente) : ControllerBase
{
    public async Task<IActionResult> GetProdotti()
        => Ok(context.Prodotti.AsNoTracking().Select((p)=> new ProdottoViewModel(p.Codice.Id, p.Nome, p.QuantitaDisponibile.Valore)));


    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> InserisciProdotto(ProdottoInserireForm form)
    {
        Prodotto prodotto = new(form.Nome, new Quantita(form.QuantitaDisponibile));
        await context.AddAsync(prodotto);
        await context.SaveChangesAsync();

        return Ok(new { prodotto.Codice.Id });
    }


    [Authorize]
    [HttpPost("{id}/Acquista")]
    public async Task<IActionResult> AcquistaProdotto(Guid id, AcquistaProdottoForm form)
    {
        using var transaction = await context.Database.BeginTransactionAsync();
        var prodotto = await context
                .Prodotti
                .Where((p) => p.Codice == new IdProdotto(id))
                .FirstOrDefaultAsync();

        if (prodotto == null)
            return NotFound();
        
        if (idUtente is not IdCustomer)
            return Forbid();

        Quantita quantitaDaAcquistare = new(form.Quantita);

        if (!prodotto.IsDisponibile(quantitaDaAcquistare))
            return Problem("Prodotto non disponibile");

        prodotto.RiduciScorta(quantitaDaAcquistare);

        Acquisto acquisto = new((IdCustomer)idUtente, prodotto.Codice, quantitaDaAcquistare);

        await context.AddAsync(acquisto);
        await context.SaveChangesAsync();

        await transaction.CommitAsync();

        return Ok();
    }
}