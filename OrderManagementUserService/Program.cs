using System.Data;
using Npgsql;
using OrderManagementCommon;
using OrderManagementUserService.Database;

var builder = WebApplication.CreateBuilder(args);

builder.BuildCustomWebApp();
builder.Services.AddDbContext<OrderManagementDbContext, UserServiceDbContext>();

var app = builder.Build();

app.ConfiguraCustomWebApp();

app.Run();
