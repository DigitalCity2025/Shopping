using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shopping.API.Security;
using Shopping.DAL;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ShoppingContext>(
    o => o.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);

TokenManager.Config config = builder.Configuration.GetSection("Jwt")
    .Get<TokenManager.Config>() ?? throw new Exception("Missing jwt config");

builder.Services.AddSingleton<ITokenManager, TokenManager>(
    _ => new TokenManager(config)
);

// configuration du middleware d'authentification
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o => o.TokenValidationParameters = new TokenValidationParameters
{
    ValidateAudience = true,
    ValidAudience = config.Audience,
    ValidateIssuer = true,
    ValidIssuer = config.Issuer,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Secret))
});

// CORS CROSS ORIGIN RESOURCE SHARING
// partage de ressource inter domaine

builder.Services.AddCors(p => {
    p.AddDefaultPolicy(o =>
        o.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin()
    );

    //p.AddPolicy("test", o =>
    //{
    //    o.AllowAnyHeader()
    //    .AllowAnyMethod()
    //    //.AllowAnyOrigin()
    //    .WithOrigins("http://localhost:4200")
    //});
});

var app = builder.Build();
app.UseCors();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
