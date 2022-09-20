var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/nome", () => new{nome = "Victor", idade="25"});
app.MapGet("/rota", (HttpResponse response) => {
    response.Headers.Add("teste", "testeteste");
    return  new{nome = "Victor", idade="25"};
});

app.MapPost("/saveproduct", (Product product) => {
    return product.Code + " - " + product.Name;
});

app.Run();

public class Product {
    public string Code { get; set; }
    public string Name { get; set; }
}
