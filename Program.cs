using MyofficeApi.Repositories;
using MyofficeApi.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. µù¥U Controller »P Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Myoffice API", Version = "v1" });
});

// 2. µù¥U¤T¼h¦¡¬[ºc (¨Ì¿àª`¤J)
builder.Services.AddScoped<IMyofficeAcpdService, MyofficeAcpdService>();
builder.Services.AddScoped<IMyofficeAcpdRepository, MyofficeAcpdRepository>();

var app = builder.Build();

// 3. ±Ò¥Î Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Myoffice API v1"));
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();