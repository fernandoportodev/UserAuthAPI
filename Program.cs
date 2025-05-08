using Microsoft.EntityFrameworkCore;
using System.Text;
using UserAuthAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using UserAuthAPI.Configurations;
using UserAuthAPI.Services;
using UserAuthAPI.Mappings;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(UserDTOMappingProfile));
builder.Services.AddScoped<TokenService>();

var jwtSection = builder.Configuration.GetSection("JwtSettings");

// Carregar o arquivo jwtsettings.json
var jwtSettingsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "jwtsettings.json");

var jwtSettings = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile(jwtSettingsFilePath, optional: false, reloadOnChange: true)
    .Build();

// Obter a chave secreta do arquivo de configurações
var secretKey = jwtSettings["SecretKey"];
if (string.IsNullOrEmpty(secretKey))
{
    throw new InvalidOperationException("A chave secreta do JWT não foi definida.");
}

var key = ConvertHexToByteArray(secretKey);

// Configurar a autenticação com JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,  // A validação de 'Issuer' é desabilitada
        ValidateAudience = false, // A validação de 'Audience' é desabilitada
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),

    };
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "UserAuthAPI", Version = "v1" });

    // Configuração para usar JWT no Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header usando o esquema Bearer. 
                        Exemplo: 'Bearer 12345abcdef'",
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
builder.Services.AddRouting(option => option.LowercaseUrls = true);
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

app.Run();

static byte[] ConvertHexToByteArray(string hex)
{
    int length = hex.Length;
    byte[] byteArray = new byte[length / 2];
    for (int i = 0; i < length; i += 2)
    {
        byteArray[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
    }
    return byteArray;
}