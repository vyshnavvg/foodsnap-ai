using Core.Interfaces;
using Infrastructure.Helper;
using Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10485760; //10MB limit
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontEnd",
        policy => policy
            .WithOrigins("http://localhost:4200") //Replace with Angular url
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// Register Services
builder.Services.AddSingleton<DbConnectionHelper>();
builder.Services.AddScoped<IFoodRepository, FoodRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontEnd");

app.UseAuthorization();

app.MapControllers();

app.Run();
