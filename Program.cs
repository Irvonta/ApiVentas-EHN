using ApiVentas.Configurations;
using ApiVentas.Services;
using System.Net;
using System.Security.Authentication;

ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Mongo
builder.Services.Configure<MongoSettings>(
    builder.Configuration.GetSection("MongoDB"));

builder.Services.AddSingleton<AgenteService>();

builder.Services.AddSingleton<NotaService>();

builder.Services.AddSingleton<CobranzaService>();

builder.Services.AddSingleton<ClienteService>();

builder.Services.AddSingleton<MetaService>();


var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Habilita Controllers
app.MapControllers();

app.Run();