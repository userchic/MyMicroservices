using FluentValidation;
using MessageService.Abstractions;
using MessageService.DataBase;
using MessageService.DTO;
using MessageService.Repositories;
using MessageService.Services;
using MessageService.SwaggerFilters;
using MessageService.Validators;
using Microsoft.EntityFrameworkCore;
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
        Title = "MessagesAPI",
        Description = "API от микросервиса сообщений",
        Contact = new OpenApiContact
        {
            Name = "Ruslan",
            Url = new Uri("https://github.com/userchic")
        },

    });
    var basePath = AppContext.BaseDirectory;

    var xmlPath = Path.Combine(basePath, "MessageService.xml");
    options.IncludeXmlComments(xmlPath);
    options.AddOperationFilterInstance<AuthHeaderFilter>(new AuthHeaderFilter());
});

builder.Services.AddDbContext<MessageContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IMessageService, MessageService.Services.MessageService>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IDialogRepository, DialogRepository>();
builder.Services.AddScoped<IValidator<CreateMessageRequest>, CreateMessageRequestValidator>();
builder.Services.AddScoped<IValidator<UpdateMessageRequest>, UpdateMessageRequestValidator>();


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
