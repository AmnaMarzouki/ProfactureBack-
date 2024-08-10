using Microsoft.EntityFrameworkCore;
using Pro_FactureAPI.Data;
using Pro_FactureAPI.Service.Abonnement;
using Pro_FactureAPI.Service.Fichier;
using Pro_FactureAPI.Service.Repertoire;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.MaxDepth = 64; 
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IFichier, FichierService>();
builder.Services.AddScoped<IAbonnement, AbonnementService>();
builder.Services.AddScoped<IRepertoire, RepertoireService>();

// Add DbContext
builder.Services.AddDbContext<ProfactureDb>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProfactureDb")));

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

// CORS middleware should be placed here, before UseAuthorization and MapControllers
app.UseCors("AllowAll");

app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
