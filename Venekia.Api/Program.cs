using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using Venekia.Application.Interfaces.Auth;
using Venekia.Application.Services.Auth;
using Venekia.Infrastructure.Data;
using Venekia.Infrastructure.Services.Auth;
using Venekia.Infrastructure.Repositories.Users;
using Venekia.Application.Interfaces.Users;
using Venekia.Application.Interfaces.Finance.Wallets;
using Venekia.Application.Services.Finance.Wallets;
using Venekia.Infrastructure.Repositories.Finance.Wallets;
using Venekia.Application.Interfaces.Common;
using Venekia.Infrastructure.Repositories.Common;

var builder = WebApplication.CreateBuilder(args);

#region Controllers & Swagger

// Agrega soporte para Controllers (API REST)
builder.Services.AddControllers();

// Permite que Swagger detecte los endpoints disponibles
builder.Services.AddEndpointsApiExplorer();

// Genera la UI de Swagger para probar la API
builder.Services.AddSwaggerGen();

//Cors para futura implementacion con frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:5173")

                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});
#endregion

#region Database 

// Registro del DbContext
// AddDbContext => EF Core gestiona el ciclo de vida (Scoped por request)
var provider = builder.Configuration["Database:Provider"];

builder.Services.AddDbContext<VenekiaDb>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SqlServer")
        ?? throw new InvalidOperationException("Connection string 'SqlServer' not found")
    );
});


#endregion

#region Dependency Injection (Application Layer)

// Repositorios (Infraestructura)
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// Scoped: una instancia por request HTTP

// Servicios de dominio / aplicación
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IWalletService, WalletService>();

#endregion

#region Authentication (JWT)

// Se obtiene la clave secreta del JWT desde configuración segura
var jwtKey = builder.Configuration["Jwt:SecretKey"]
    ?? throw new InvalidOperationException("JWT Key is not configured");

// Configuración de autenticación usando JWT Bearer
builder.Services.AddAuthentication(options =>
{
    // Esquema por defecto para autenticar
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

    // Esquema por defecto cuando se requiere autenticación
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Parámetros que se usarán para validar el token
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // Valida quién emitió el token
        ValidateIssuer = true,

        // Valida para quién fue emitido
        ValidateAudience = true,

        // Valida que el token no esté expirado
        ValidateLifetime = true,

        // Valida que el token esté firmado correctamente
        ValidateIssuerSigningKey = true,

        // Valores válidos esperados
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],

        // Clave usada para firmar el token
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey!)
        )
    };
});

#endregion

var app = builder.Build();

#region Middleware pipeline

// Swagger solo disponible en entorno Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirige HTTP a HTTPS
app.UseHttpsRedirection();

// Middleware de autenticación (lee el JWT)
app.UseAuthentication();

// Middleware de autorización (evalúa permisos)
app.UseAuthorization();

// Mapea los Controllers a endpoints HTTP
app.MapControllers();

#endregion

app.Run();
