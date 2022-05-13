using API.Extensions;
using Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
IConfiguration config = builder.Configuration;
builder.Services.AddApplications(config);

using (var services = builder.Services.BuildServiceProvider())
{
    try
    {
        var context = services.GetRequiredService<TodoContext>();
        await context.Database.MigrateAsync();
        // await Seed.SeedData(context);
    }
    catch (Exception exc)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(exc, "An Error occurred during migration");
    }
}

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
