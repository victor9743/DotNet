using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSqlServer<ApplicationDBContext>(builder.Configuration["Database:SqlServer"]);

var app = builder.Build();



app.MapPost("/products", (ProductRequest request, ApplicationDBContext context) =>
{
    var category = context.Categories.Where(c => c.Id == request.CategoryId).First();
    var product = new Product
    {
        Code = request.Code,
        Name = request.Name,
        Description = request.Description,
        Category = category
    };

    if (request.Tags != null)
    {
        product.Tags = new List<Tag>();

        foreach(var item in request.Tags)
        {
            product.Tags.Add(new Tag { Name = item });
        }
    }

    context.Products.Add(product);
    context.SaveChanges();

    return Results.Created($"/products/{product.Id}", product.Id);
});

app.MapGet("/products/{id}", ([FromRoute] int id, ApplicationDBContext context) =>
{
    var product = context.Products
    .Include(p => p.Category)
    .Include(p => p.Tags)
    .Where(p => p.Id == id).First();

    if (product != null)
        return Results.Ok(product);

    return Results.NotFound();
});

app.MapPut("/products/{id}", ([FromRoute] int id, ProductRequest request, ApplicationDBContext context) =>
{
    var product = context.Products
    .Include(p => p.Category)
    .Include(p => p.Tags)
    .Where(p => p.Id == id).First();

    var category = context.Categories.Where(c => c.Id == request.CategoryId).First();

    product.Code = request.Code;
    product.Name = request.Name;
    product.Description = request.Description;
    product.Category = category;
    product.Tags = new List<Tag>();

    if (request.Tags != null)
    {
        product.Tags = new List<Tag>();
        foreach(var item in request.Tags)
        {
            product.Tags.Add(new Tag { Name = item });
        }
    }

    context.SaveChanges();
    return Results.Ok();
});

app.MapDelete("/products/{id}", ([FromRoute] int id, ApplicationDBContext context) =>
{
    var product = context.Products
    .Where(p => p.Id == id).First();

    context.Products.Remove(product);
    context.SaveChanges();
    return Results.Ok();
});

app.Run();







