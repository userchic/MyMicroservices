using AuthService;
using AuthService.Abstractions;
using AuthService.DataBase;
using AuthService.DTO;
using AuthService.Repositories;
using AuthService.Services;
using AuthService.SwaggerFilters;
using AuthService.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.CodeAnalysis.Workspaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Prometheus;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "User API",
        Description = "API от микросервиса пользователей",
        Contact = new OpenApiContact
        {
            Name = "Ruslan",
            Url = new Uri("https://github.com/userchic")
        },
        
    });
    var basePath = AppContext.BaseDirectory;

    var xmlPath = Path.Combine(basePath, "AuthService.xml");
    options.IncludeXmlComments(xmlPath);
    options.AddOperationFilterInstance<AuthHeaderFilter>(new AuthHeaderFilter());
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer("Bearer",options =>
        {
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                // укзывает, будет ли валидироваться издатель при валидации токена
                ValidateIssuer = true,
                // строка, представляющая издателя
                ValidIssuer = AuthOptions.ISSUER,

                // будет ли валидироваться потребитель токена
                ValidateAudience = true,
                // установка потребителя токена
                ValidAudience = AuthOptions.AUDIENCE,
                // будет ли валидироваться время существования
                ValidateLifetime = true,

                // установка ключа безопасности
                IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                // валидация ключа безопасности
                ValidateIssuerSigningKey = true,
                
            };
        });

builder.Services.AddAuthorization();
builder.Services.AddDbContext<UserContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
builder.Services.AddScoped<IValidator<RegistryRequest>, RegistryRequestValidator>();
builder.Services.AddScoped<IValidator<ChangeProfileRequest>, ChangeProfileRequestValidator>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UsersRepository>();

builder.Services.AddMemoryCache();
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.ConfigureLogging(logging =>
{
    logging.AddSerilog();
    logging.SetMinimumLevel(LogLevel.Information);
})
.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMetricServer();
app.UseHttpMetrics();

app.UseCors(builder =>
builder.AllowAnyHeader()
.AllowAnyOrigin()
.AllowAnyMethod());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
