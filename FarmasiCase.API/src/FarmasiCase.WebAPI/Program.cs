using FarmasiCase.Infrastructure;
using FarmasiCase.Persistence;
using FarmasiCase.Persistence.Models;
using FarmasiCase.Service;

var builder = WebApplication.CreateBuilder(args);

//// Service Registrations
builder.Services.Configure<FarmasiCaseSettings>(builder.Configuration.GetSection("FarmasiCaseMongoDbSettings"));
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "FarmasiCase";
});
builder.Services.ImplementInfrastructureServices();
builder.Services.ImplementServiceServices();

// Adding CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("FarmasiCase",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000",
                                       "http://localhost:3001")
                                       .AllowAnyHeader()
                                       .AllowAnyMethod()
                                       .AllowCredentials();
        });
});

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Cors
app.UseCors("FarmasiCase");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
