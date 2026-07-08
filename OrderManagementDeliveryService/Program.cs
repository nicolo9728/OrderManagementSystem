using OrderManagementCommon;
using OrderManagementDeliveryService.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<OrderManagementDbContext, DeliveryServiceDbContext>();

builder.BuildCustomWebApp();

var app = builder.Build();

app.ConfiguraCustomWebApp();

app.Run();