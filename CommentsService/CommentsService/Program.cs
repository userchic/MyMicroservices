using CommentsService.Abstractions;
using CommentsService.DataBase;
using CommentsService.Dto;
using CommentsService.Repository;
using CommentsService.Services;
using CommentsService.SwaggerFilters;
using CommentsService.Validators;
using FluentValidation;
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
        Title = "PostsAPI",
        Description = "API от микросервиса постов",
        Contact = new OpenApiContact
        {
            Name = "Ruslan",
            Url = new Uri("https://github.com/userchic")
        },

    });
    var basePath = AppContext.BaseDirectory;

    var xmlPath = Path.Combine(basePath, "CommentsService.xml");
    options.IncludeXmlComments(xmlPath);
    options.AddOperationFilterInstance<AuthHeaderFilter>(new AuthHeaderFilter());
});

builder.Services.AddDbContext<CommentsContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IValidator<CreateCommentRequest>, CreateCommentRequestValidator>();
builder.Services.AddScoped<IValidator<UpdateCommentRequest>, UpdateCommentRequestValidator>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ICommentsRepository,CommentsRepository>();

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
