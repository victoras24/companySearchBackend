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
builder.Services.AddOpenApi();

builder.Services.AddScoped<Supabase.Client>(_ =>
    new Supabase.Client(
        builder.Configuration["Supabase:Url"],
        builder.Configuration["Supabase:Key"],
        new SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = true
        }));

builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IOfficialRepository, OfficialRepository>();

builder.Services.AddCors();

Env.Load(".env");

string connString = "";

{
    if (!builder.Environment.IsDevelopment())
    {
        connString = Environment.GetEnvironmentVariable("DB_ORGANISATIONS_CONN_STRING") ?? string.Empty;

        var decryptor = new StringDecryptor("ElonMaHowYouDoingBruda!");
        connString = decryptor.Decrypt(connString);

    }
    else
    {
        connString = builder.Configuration.GetConnectionString("CompanyConnection") ?? string.Empty;
    }
}

// builder.Services.AddDbContext<CompanyDbContext>(options =>
// {
//     options.UseSqlServer(connString);
// });

var app = builder.Build();

app.UseCors(options =>
{
    options.WithOrigins("http://localhost:5173");
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