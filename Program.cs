using Microsoft.EntityFrameworkCore;
using CompanySearchBackend.Models;
using CompanySearchBackend.Interfaces;
using CompanySearchBackend.Repository;
using CompanySearchBackend.Services;
using CompanySearchBackend.StringEnc;
using DotNetEnv;
using Microsoft.Extensions.Options;
using Supabase;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<Supabase.Client>(_ =>
    new Supabase.Client(
        Environment.GetEnvironmentVariable("Url"),
        Environment.GetEnvironmentVariable("Key"),
        new SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = true
        }));

builder.Services.AddHttpClient<ICompanyService, CompanyService>();
builder.Services.AddHttpClient<IAddressService, AddressService>();
builder.Services.AddHttpClient<IOfficialService, OfficialService>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IOfficialRepository, OfficialRepository>();


// Configure CORS properly
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowedOrigins", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173", 
                "http://localhost:5174",
                "http://localhost:7256",
                "https://yellow-smoke-02d271f03.6.azurestaticapps.net")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // Add this if you need to send cookies or auth headers
    });
});

Env.Load(".env");

var app = builder.Build();

// Use the named CORS policy
app.UseCors("AllowedOrigins");

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseAuthorization();
app.MapControllers();

app.Run();