using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SubscriptionsService.Abstractions;
using SubscriptionsService.DataBase;
using SubscriptionsService.Repositories;
using SubscriptionsService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<SubscriptionsContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<ISubscriptionsService, SubscriptionsService.Services.SubscriptionsService>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionsRepository>();
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
