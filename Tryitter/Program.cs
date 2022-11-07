using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using Tryitter.Data;
using Tryitter.Models;
using Tryitter.Repositories;
using Tryitter.UseCases;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();
// Add services to the container.


var secret = Environment.GetEnvironmentVariable("DOTNET_JWT_SECRET");
var key = Encoding.ASCII.GetBytes(secret!);

builder.Services.AddAuthentication(x => 
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddControllers();

builder.Services.AddDbContext<TryitterContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserUseCase, UserUseCase>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IPostUseCase, PostUseCase>();

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

var adminEmail = Environment.GetEnvironmentVariable("SERVER_ADMIN_EMAIL");
var adminPassword = Environment.GetEnvironmentVariable("SERVER_ADMIN_PASSWORD");
if (adminEmail is null || adminPassword is null)
{
    adminEmail = "admin@example.com";
    adminPassword = "admin123";
}

var userExists = context.Users.FirstOrDefault(x => x.Email == adminEmail);

if (userExists is null)
{
    var passwordHash = BCrypt.Net.BCrypt.HashPassword(adminPassword);

    if (adminEmail is null || adminPassword is null)
    {
        throw new Exception("Admin email or password not found");
    }

    context.Users.Add(new User() {
        Username = "admin",
        Email = adminEmail,
        Name = "Administrador",
        Password = passwordHash,
        Role = "Admin",
        Module = "",
        Status = ""
    });
    context.SaveChanges();
}

app.Run();

public partial class program {}
