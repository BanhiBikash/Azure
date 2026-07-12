using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DBContext;
using WebApplication1.Services;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddApplicationInsightsTelemetry(new Microsoft.ApplicationInsights.AspNetCore.Extensions.ApplicationInsightsServiceOptions
{
    ConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]
});

//Fetch the connection string from secrets.json file
var cString = builder.Configuration["AzureSQLConnection"];

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(cString);
});

builder.Services.AddScoped<ITableStorageService, TableStorageService>();
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();

var app = builder.Build();
app.MapControllers();   
//app.MapGet("/", () => "Hello World!");

app.Run();
