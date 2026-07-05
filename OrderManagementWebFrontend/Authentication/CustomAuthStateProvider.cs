using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using OrderManagementViewmodels.Utenti;
using OrderManagementWebFrontend.Services;

namespace OrderManagementWebFrontend.Authentication;


public class CustomAuthStateProvider(ApiClientUser api) : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            UtenteLoggatoViewModel utenteLoggato = await api.GetAsync<UtenteLoggatoViewModel>("/Utente/UtenteLoggato");
            Claim[] claims = [
                new Claim(ClaimTypes.NameIdentifier, utenteLoggato.Id.ToString()),
                new Claim(ClaimTypes.Name, utenteLoggato.Username),
                new Claim(ClaimTypes.Role, utenteLoggato.Ruolo)
            ];

            var identity = new ClaimsIdentity(claims, "CustomAuth");
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }
        catch (Exception)
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }
}