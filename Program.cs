using Cp2Mottu.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// TODO: Adicionar documentação Swagger para a API
builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API de filiais e motos Mottu",
        Version = "v1",
        Description = "API para gerenciar filiais e motos da Mottu nos pátios",
        Contact = new OpenApiContact
        {
            Name = "Prisma.Code",
            Email = "prismacode3@gmail.com"
        },
        
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"; // Obtém o nome do arquivo XML de documentação
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile); // Cria o caminho completo para o arquivo XML
    swagger.IncludeXmlComments(xmlPath); // Inclui o arquivo XML de documentação no Swagger
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("Oracle"))); // Configura o DbContext para usar o Oracle com a string de conexão definida no appsettings.json

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
