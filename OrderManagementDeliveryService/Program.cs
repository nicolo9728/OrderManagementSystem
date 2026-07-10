using OrderManagementCommon;
using OrderManagementCommon.Events;
using OrderManagementDeliveryService.Consumers;
using OrderManagementDeliveryService.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<OrderManagementDbContext, DeliveryServiceDbContext>();
builder.Services.AddScoped<IEventConsumer<AcquistoCreatoEvent>, AcquistoCreatoConsumer>();

builder.BuildCustomWebApp();

var app = builder.Build();

app.ConfiguraCustomWebApp();

app.Run();