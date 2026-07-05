using OrderManagementCommon;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.BuildCustomWebApp();

var app = builder.Build();

app.ConfiguraCustomWebApp();

app.Run();