using Tryitter.Models;
using Tryitter.Repositories;
using Tryitter.UseCases;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<TryitterContext>();
builder.Services.AddScoped<TryitterContext>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserUseCase>();
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

var context = new TryitterContext();
context.Database.EnsureCreated();

app.Run();

public partial class program {}