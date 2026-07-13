using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OrderManagementCommon.Models;
using OrderManagementUserService.Database;
using OrderManagementUserService.Models;
using OrderManagementViewmodels.Utenti;

namespace OrderManagementUserService.Controllers;

[ApiController]
[Route("[controller]")]
public class UtenteController(UserServiceDbContext context, IConfiguration configuration, IHttpContextAccessor httpContext, IdUtente idUtenteLoggato) : ControllerBase
{
    public record LoginForm(string Username, string Password);

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginForm form)
    {
        var utente = await context.Utenti.Where((u) => u.Credenziali.Username == form.Username).FirstOrDefaultAsync();
        if (utente is null)
            return Unauthorized("Credential not valid");

        if (!utente.Credenziali.Password.Match(form.Password))
            return Unauthorized("Credential not valid");

        string jwtToken = GenerateToken(utente);

        var cookieOptions = new CookieOptions()
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.Now.AddDays(1)
        };

        httpContext.HttpContext!.Response.Cookies.Append("auth", jwtToken, cookieOptions);

        return Ok(new { Message = "Ok" });
    }

    [HttpPost("Logout")]
    public async Task<IActionResult> Logout()
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddDays(-1) // Scadenza nel passato
        };

        httpContext.HttpContext!.Response.Cookies.Delete("auth", cookieOptions);

        return Ok();
    }

    [HttpGet("UtenteLoggato")]
    [Authorize]
    public async Task<IActionResult> GetUtenteLoggato() => Ok(
        await context
                .Utenti
                .Select((u) => new { Id = EF.Property<Guid>(u, "IdRaw"), u.Credenziali.Username, Ruolo = EF.Property<string>(u, "Ruolo") })
                .Where((u) => u.Id == idUtenteLoggato.Id)
                .Select((u) => new UtenteLoggatoViewModel(u.Id, u.Username, u.Ruolo))
                .FirstOrDefaultAsync()
    );

    private string GenerateToken(Utente user)
    {
        Claim[] claims = [
            new Claim(ClaimTypes.NameIdentifier, user.Id.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Credenziali.Username),
            new Claim(ClaimTypes.Role, user.GetType().Name)
        ];

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
        var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credential
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUtenti() => Ok(
        await context
                .Utenti
                .Select((u) => new UtenteLoggatoViewModel(EF.Property<Guid>(u, "IdRaw"), u.Credenziali.Username, EF.Property<string>(u, "Ruolo")))
                .ToListAsync()
    );
}