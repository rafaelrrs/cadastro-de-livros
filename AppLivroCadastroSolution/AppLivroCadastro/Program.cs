using AppLivroCadastro.Application;
using AppLivroCadastro.Application.Interfaces;
using AppLivroCadastro.Application.Mappings;
using AppLivroCadastro.Application.Services;
using AppLivroCadastro.Infrastructure;
using AppLivroCadastro.Infrastructure.Persistence;
using AppLivroCadastro.Reports.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "App Livro Cadastro API",
        Version = "v1",
        Description = "API para cadastro e gestão de livros, autores e assuntos",
        Contact = new OpenApiContact
        {
            Name = "Rafael",
            Email = "rafaelsilva90@gmail.com",
            Url = new Uri("https://github.com/rafaelrrs")
        }
    });
});
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<ILivroService, LivroService>();
builder.Services.AddScoped<IFormaCompraService, FormaCompraService>();
builder.Services.AddScoped<IAutorService, AutorService>();
builder.Services.AddScoped<IAssuntoService, AssuntoService>();
builder.Services.AddScoped<LivroAutorAssuntoReportService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "App Livro Cadastro API V1");
    });
}

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();