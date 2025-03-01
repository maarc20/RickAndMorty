using Microsoft.Extensions.Options;
using PruebaEurofirms.Configurations;

using PruebaEurofirms.Application.Services;
using PruebaEurofirms.Application.Interfaces;

using PruebaEurofirms.Infrastructure;
using PruebaEurofirms.Infrastructure.Interfaces;
using PruebaEurofirms.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));
// Configurar HttpClient con la URL base desde la configuración
builder.Services.AddHttpClient<ApiClientService>((serviceProvider, client) =>
{
    var apiSettings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value;
    client.BaseAddress = new Uri(apiSettings.BaseUrl);  // Establece la BaseUrl de la configuración
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});


builder.Services.AddScoped<ICharacterService, CharacterService>();
builder.Services.AddScoped<IEpisodeService, EpisodeService>();
builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();
builder.Services.AddScoped<IEpisodeRepository, EpisodeRepository>();

string connectionString = builder.Configuration.GetConnectionString("DbConnection") ?? String.Empty;
builder.Services.AddSingleton<DbContext>(new DbContext(connectionString));


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
    dbContext.InitializeDatabase();
}


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
