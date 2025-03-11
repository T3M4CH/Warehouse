// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Warehouse.Data;

//warehouse.AddContainer(animalsPallet);
//warehouse.AddContainer(clothesBox);
//warehouse.AddContainer(foodBox);

//warehouse.SortContainersByWeight();

//Console.WriteLine("Clothes < 2kg " + string.Join(',', warehouse.FilterProducts(product => product is Clothes { Weight: <= 2 }).Select(pr => pr.Name)));
//Console.WriteLine("Not expired food " + string.Join(',', warehouse.FilterProducts(product => product is Food food && food.ExpireDate > DateTime.Now).Select(pr => pr.Name)));

// ###

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<DataContext>
    (options => options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapGet("/", () => "Hello World");

var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

var context = services.GetRequiredService<DataContext>();

context.Database.Migrate();

app.MapControllers();

//app.UseHttpsRedirection();

app.Run();