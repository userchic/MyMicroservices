using AuthService.SwaggerFilters;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using NotificationsService.Abstractions;
using NotificationsService.Consumer;
using NotificationsService.DataBase;
using NotificationsService.DTO;
using NotificationsService.Repositories;
using NotificationsService.Services;
using NotificationsService.Validators;
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
        Title = "PostsAPI",
        Description = "API от микросервиса постов",
        Contact = new OpenApiContact
        {
            Name = "Ruslan",
            Url = new Uri("https://github.com/userchic")
        },

    });
    var basePath = AppContext.BaseDirectory;

    var xmlPath = Path.Combine(basePath, "NotificationsService.xml");
    options.IncludeXmlComments(xmlPath);
    options.AddOperationFilterInstance<AuthHeaderFilter>(new AuthHeaderFilter());
});
builder.Services.AddDbContext<NotificationContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IPersonalNotificationRulesService, PersonalNotificationRulesService>();
builder.Services.AddScoped<INotificationRulesRepository, NotificationRulesRepository>();
builder.Services.AddTransient<INotificationsService,NotificationsService.Services.NotificationsService>();
builder.Services.AddTransient<IMassNotificationRepository, MassNotificationRepository>();
builder.Services.AddSingleton<ConsumerService>();
builder.Services.AddSingleton<HttpClientService>();
builder.Services.AddScoped<IValidator<CreateNotificationRulesRequest>, CreateNotificationRuleRequestValidator>();
builder.Services.AddScoped<IValidator<UpdateNotificationRulesRequest>, UpdateNotificationRuleRequestValidator>();

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

app.UseMetricServer();
app.UseHttpMetrics();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
