using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using WEBAPI.Application.Mappings;
using WEBAPI.Application.Validators;
using WEBAPI.Infrastructure;
using WEBAPI.Infrastructure.Data;
using WEBAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// auto mapper'ý assembly'deki tüm mapping profillerini tarayarak kaydetmek için yaptým. Böylece yeni bir mapping profili eklediðimde program.cs'ye ekleme yapmam gerekmiyor.
builder.Services.AddAutoMapper(_ => { }, typeof(TodoMappingProfile).Assembly);

//Infrastructure katmanýndaki servisleri extension method'lar aracýlýðýyla kaydetmek için yaptým. Bu sayede program.cs dosyasý daha temiz ve düzenli oluyor.
builder.Services.AddInfrastructure(builder.Configuration);

// Fluent Validation validator'larýný assembly'den otomatik olarak kaydetmek için.
builder.Services.AddValidatorsFromAssemblyContaining<CreateTodoRequestValidator>();
builder.Services.AddFluentValidationAutoValidation(builder => builder.DisableDataAnnotationsValidation = true);


// CORS policy'si ekleyerek frontend uygulamasýnýn API'ye eriþimini saðlamak için yaptým. Geliþtirme aþamasýnda tüm origin'lere izin veriyorum, ancak üretim ortamýnda bunu daha kýsýtlý hale getirilmeli.
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// middlewareleri sýrayla ekleyerek global hata yakalama ve istek loglama iþlemlerini saðlýyorum.
// GlobalExceptionMiddleware, uygulama genelinde beklenmeyen hatalarý yakalayarak standart bir hata yanýtý döndürüyor. Böylece her controller'da try-catch bloklarý eklemek zorunda kalmýyoruz.
// RequestLoggingMiddleware her isteðin ne kadar sürdüðünü loglayarak performans takibi yapmamýza yardýmcý oluyor.
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseHttpsRedirection();
app.UseCors("FrontendPolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();
