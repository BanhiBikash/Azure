using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DBContext;
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

var app = builder.Build();
app.MapControllers();   
//app.MapGet("/", () => "Hello World!");

app.Run();
