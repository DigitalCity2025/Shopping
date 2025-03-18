using Microsoft.EntityFrameworkCore;
using Shopping.API.Security;
using Shopping.DAL;

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

// CORS CROSS ORIGIN RESOURCE SHARING
// partage de ressource inter domaine

builder.Services.AddCors(p => {
    p.AddDefaultPolicy(o =>
        o.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin()
    );
});

var app = builder.Build();
app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
