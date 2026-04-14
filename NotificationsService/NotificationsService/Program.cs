using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NotificationsService.Abstractions;
using NotificationsService.Consumer;
using NotificationsService.DataBase;
using NotificationsService.DTO;
using NotificationsService.Repositories;
using NotificationsService.Services;
using NotificationsService.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
