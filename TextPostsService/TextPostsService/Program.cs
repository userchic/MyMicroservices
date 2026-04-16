using AuthService.SwaggerFilters;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Serilog;
using TextPostsService.Abstractions;
using TextPostsService.DataBase;
using TextPostsService.DTO;
using TextPostsService.Producer;
using TextPostsService.Repositories;
using TextPostsService.Services;
using TextPostsService.Validators;

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

    var xmlPath = Path.Combine(basePath, "TextPostsService.xml");
    options.IncludeXmlComments(xmlPath);
    options.AddOperationFilterInstance<AuthHeaderFilter>(new AuthHeaderFilter());
}); 
builder.Services.AddDbContext<TextPostContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddSingleton<IPostProducedMessagerService, PostProducedMessagerService>();
builder.Services.AddScoped<IValidator<CreatePostRequest>, CreatePostRequestValidator>();
builder.Services.AddScoped<IValidator<UpdatePostRequest>, UpdatePostRequestValidator>();
builder.Services.AddScoped<ITextPostService, TextPostService>();
builder.Services.AddScoped<ITextPostRepository, TextPostRepository>();

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
