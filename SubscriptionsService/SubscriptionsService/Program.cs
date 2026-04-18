using AuthService.SwaggerFilters;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Serilog;
using SubscriptionsService.Abstractions;
using SubscriptionsService.DataBase;
using SubscriptionsService.Repositories;
using SubscriptionsService.Services;

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
        Title = "PostsAPI",
        Description = "API от микросервиса постов",
        Contact = new OpenApiContact
        {
            Name = "Ruslan",
            Url = new Uri("https://github.com/userchic")
        },

    });
    var basePath = AppContext.BaseDirectory;

    var xmlPath = Path.Combine(basePath, "SubscriptionsService.xml");
    options.IncludeXmlComments(xmlPath);
    options.AddOperationFilterInstance<AuthHeaderFilter>(new AuthHeaderFilter());
});
builder.Services.AddDbContext<SubscriptionsContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<ISubscriptionsService, SubscriptionsService.Services.SubscriptionsService>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionsRepository>();

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
app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());


app.UseAuthorization();

app.MapControllers();

app.Run();
