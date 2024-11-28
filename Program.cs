using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using QuadraFacil_backend.API.Data;
using System.Text;
using Microsoft.OpenApi.Models;
using QuadraFacil_backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuração do DbContext com a string de conexão
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Adiciona controladores com configurações JSON
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});

// Adiciona serviços ao contêiner
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Registra o TokenService no DI
builder.Services.AddScoped<TokenService>(); // Registre o TokenService

// Configuração do Swagger para autenticação via Token JWT
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

// Configuração de autenticação JWT
var jwtSettings = builder.Configuration.GetSection("Jwt"); // Variável que busca no appsettings.json
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// Registrar o serviço de e-mail
builder.Services.AddTransient<IEmailService, EmailService>();

// Configuração do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsQuadraFacil", policy =>
    {
        // Adicione as URLs que você quer permitir que acessem a API
        policy.WithOrigins("http://localhost:5173") // Exemplo: URL do front-end (Angular, React, etc)
              .AllowAnyHeader()   // Permite qualquer cabeçalho
              .AllowAnyMethod()   // Permite qualquer método HTTP (GET, POST, PUT, DELETE)
              .AllowCredentials(); // Permite o envio de credenciais como cookies e cabeçalhos de autenticação
    });
});

var app = builder.Build();

// Configuração do pipeline de requisição HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Aplique o middleware de CORS antes do middleware de autenticação e autorização
app.UseCors("CorsQuadraFacil");

// Middleware de autenticação e autorização devem ser aplicados após o CORS
app.UseAuthentication();  // Registra o middleware de autenticação
app.UseAuthorization();   // Registra o middleware de autorização

app.MapControllers();

app.Run();
