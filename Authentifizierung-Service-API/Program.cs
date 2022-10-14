using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Authentifizierung_Service_API.Models;
using Authentifizierung_Service_API.Data;
using Microsoft.Data.Sqlite;
using Authentifizierung_Service_API.Controllers;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("UserDb") ?? "Data Source=UserDb.db";
if (File.Exists("UserDb.db"))
{
    Console.WriteLine("--------------------------------------");
    Console.WriteLine("true");
}

// Add services to the container.
builder.Services.AddSqlite<AuthContext>(connectionString);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
