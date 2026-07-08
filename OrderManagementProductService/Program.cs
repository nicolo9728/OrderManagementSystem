using System.Data;
using Npgsql;
using OrderManagementCommon;
using OrderManagementCommon.Database;
using OrderManagementProductService.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<OrderManagementDbContext, ProductServiceDbContext>();
builder.BuildCustomWebApp();

var app = builder.Build();

app.ConfiguraCustomWebApp();

app.Run();