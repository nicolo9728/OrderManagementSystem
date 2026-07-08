using System.Data;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using OrderManagementCommon.Models;
using OrderManagementCommon.Workers;

namespace OrderManagementCommon;

public static class ConfigureWebApp
{
    extension(WebApplicationBuilder builder)
    {
        public void BuildCustomWebApp()
        {
            builder.Services.AddControllers();
            builder.Services.AddScoped<IdUtente>((options) =>
            {
                HttpContext? accessor = options.GetService<IHttpContextAccessor>()?.HttpContext;
                string? idValue = accessor?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                string? role = accessor?.User.FindFirst(ClaimTypes.Role)?.Value;

                return idValue != null && role != null ? IdUtente.GetIdUtenteFromGuidAndRole(Guid.Parse(idValue), role) : new IdUtenteEmpty(); 
            });

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer((options) =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:Key"]!))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies.TryGetValue("auth", out string? token) ? token : null;
                            return Task.CompletedTask;
                        }
                    };
                });

            builder.Services.AddHostedService<EventPublisherWorker>();

            builder.Services.AddHttpContextAccessor();
        }
    }

    extension(WebApplication app)
    {
        public void ConfiguraCustomWebApp()
        {
            IConfiguration configuration = app.Services.GetService<IConfiguration>()!;

            app.UseCors((options) =>
                options
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithOrigins(configuration["Frontend:Domain"]!));

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

        }
    }
}