using FluentValidation;
using Microsoft.EntityFrameworkCore;
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
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TextPostContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddSingleton<IPostProducedMessagerService, PostProducedMessagerService>();
builder.Services.AddScoped<IValidator<CreatePostRequest>, CreatePostRequestValidator>();
builder.Services.AddScoped<IValidator<UpdatePostRequest>, UpdatePostRequestValidator>();
builder.Services.AddScoped<ITextPostService, TextPostService>();
builder.Services.AddScoped<ITextPostRepository, TextPostRepository>();
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
