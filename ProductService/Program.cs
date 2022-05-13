using MediatR;
using ProductService.Domain.Mappings;
using ProductService.Infrastructure.Extensions;
using ProductService.Infrastructure.Mappings;
using ProductService.V1.Mappings;
using FluentValidation.AspNetCore;
using System.Reflection;
using ProductService.Domain.Common.Configuration;
using ProductService.Api.Middleware;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAutoMapper(typeof(EFCoreMappingProfile));
builder.Services.AddAutoMapper(typeof(ApiProductMappingProfile));
builder.Services.AddAutoMapper(typeof(DomainProductMappingProfile));

builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.Configure<CacheConfiguration>(builder.Configuration.GetSection("CacheConfiguration"));
builder.Services.AddSingleton(new CacheConfiguration());

builder.Services.AddDatabase();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddControllers().AddFluentValidation(c =>
    c.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Products API",
        Description = "ProductService.API"
    });

    c.EnableAnnotations();
});

builder.Logging.AddLog4Net("log4net.config");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ResponseTimeMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
