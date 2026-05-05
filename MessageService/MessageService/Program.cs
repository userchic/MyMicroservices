using FluentValidation;
using MessageService.Abstractions;
using MessageService.DataBase;
using MessageService.DTO;
using MessageService.Repositories;
using MessageService.Services;
using MessageService.Validators;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MessageContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IMessageService, MessageService.Services.MessageService>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IDialogRepository, DialogRepository>();
builder.Services.AddScoped<IValidator<CreateMessageRequest>, CreateMessageRequestValidator>();
builder.Services.AddScoped<IValidator<UpdateMessageRequest>, UpdateMessageRequestValidator>();

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
