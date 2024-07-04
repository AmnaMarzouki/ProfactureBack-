using Microsoft.EntityFrameworkCore;
using Pro_FactureAPI.Data;
using Pro_FactureAPI.Service.Fichier;
using Pro_FactureAPI.Service.Repertoire;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IFichier, FichierService>();
builder.Services.AddScoped<IRepertoire, RepertoireService>();
builder.Services.AddDbContext<ProfactureDb>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ProfactureDb"))); var app = builder.Build();

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
