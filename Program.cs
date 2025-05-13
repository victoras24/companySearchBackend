using Microsoft.EntityFrameworkCore;
using CompanySearchBackend.Models;
using CompanySearchBackend.Interfaces;
using CompanySearchBackend.Repository;
using CompanySearchBackend.StringEnc;
using DotNetEnv;
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

builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IOfficialRepository, OfficialRepository>();

builder.Services.AddCors();

Env.Load(".env");

var app = builder.Build();

app.UseCors(options =>
{
    options.WithOrigins(
        "http://localhost:5173", 
        "http://localhost:5174",
        "https://company-search-cy-vgib-delta.vercel.app/", 
        "https://company-search-cy-vgib-ajwi90uio-victoras24s-projects.vercel.app/");
});

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseAuthorization();
app.MapControllers();

app.Run();