using System.Data;
using Npgsql;
using OrderManagementCommon;
using OrderManagementCommon.Database;
using OrderManagementProductService.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ProductServiceDbContext>();
builder.BuildCustomWebApp();

var app = builder.Build();

app.ConfiguraCustomWebApp();

app.Run();