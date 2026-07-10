using OrderManagementCommon;
using OrderManagementCommon.Events;
using OrderManagementDeliveryService.Consumers;
using OrderManagementDeliveryService.Database;
using OrderManagementDeliveryService.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<OrderManagementDbContext, DeliveryServiceDbContext>();
builder.Services.AddScoped<IEventConsumer<AcquistoCreatoEvent>, AcquistoCreatoConsumer>();
builder.Services.AddScoped<IEventConsumer<OrderCreatoEvent>, OrderCreatoConsumer>();

builder.BuildCustomWebApp();

var app = builder.Build();

app.ConfiguraCustomWebApp();

app.Run();