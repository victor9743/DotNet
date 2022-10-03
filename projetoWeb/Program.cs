using Microsoft.AspNetCore.Mvc;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapPost("/nome", () => new{nome = "Victor", idade="25"});
app.MapGet("/rota", (HttpResponse response) => {
    response.Headers.Add("teste", "testeteste");
    return  new{nome = "Victor", idade="25"};
});

app.MapPost("/saveproduct", (Product product) => {
    // return product.Code + " - " + product.Name;
    ProductRepository.Add(product);
});

app.MapGet("/getproduct", ([FromQuery] string dateStart, [FromQuery] string dateEnd) => {
    return dateStart + " - " + dateEnd;
});

app.MapGet("/getproduct/{code}", ([FromRoute] string code) => {
    var product = ProductRepository.GetBy(code);
    return product;
});

app.MapGet("/getproducts", (HttpRequest request)=>{
    return request.Headers["product"].ToString();
});

app.Run();

public class Product {
    public string Code { get; set; }
    public string Name { get; set; }
}

public static class ProductRepository {
    // criando array
    public static List<Product> Products {get; set;}

    public static void Add(Product product) {
        if(Products == null) {
            Products = new List<Product>();
        }
        Products.Add(product);

    }

    public static Product GetBy(string code){
        return Products.First(p => p.Code == code);
    }
}