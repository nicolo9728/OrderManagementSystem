using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using OrderManagementWebFrontend.Models;

namespace OrderManagementWebFrontend.Services;

public abstract class ApiClient(string baseUrl)
{
    private HttpClient _http = new HttpClient();

    private async Task HandleErrori(HttpResponseMessage res)
    {
        if (!res.IsSuccessStatusCode)
        {
            var problem = await res.Content.ReadFromJsonAsync<ProblemDetails>();
            
            if(problem == null)
                throw new Exception("Errore interno del server");

            throw new Exception(problem.Detail);
        }
    }

    public async Task<T> GetAsync<T>(string url)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}{url}");
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        var response = await _http.SendAsync(request);
        
        await HandleErrori(response);

        return (await response.Content.ReadFromJsonAsync<T>())!;
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest body)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}{url}")
        {
            Content = JsonContent.Create(body)
        };

        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        var response = await _http.SendAsync(request);
        
        await HandleErrori(response);

        return await response.Content.ReadFromJsonAsync<TResponse>();
    }


    public async Task PostAsync<TRequest>(string url, TRequest body)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}{url}")
        {
            Content = JsonContent.Create(body)
        };

        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        var response = await _http.SendAsync(request);

        await HandleErrori(response);
    }
}
