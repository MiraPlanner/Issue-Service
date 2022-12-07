using Mira_Common.MongoDB;
using Issue_Service.Interfaces;
using Issue_Service.Models;
using Issue_Service.Services;
using Mira_Common.MassTransit;

var builder = WebApplication.CreateBuilder(args);

var allowedOrigins = builder.Configuration.GetSection("AllowedHosts").Value.Split(";");

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddMongo()
    .AddMongoRepository<Issue>("issues")
    .AddMassTransitWithRabbitMq();

builder.Services.AddTransient<IIssueService, IssueService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(c => c
    .WithOrigins(allowedOrigins)
    .AllowAnyHeader()
    .AllowAnyMethod());

app.UseAuthorization();

app.MapControllers();

app.Run();

public abstract partial class Program { }