using IWantApp.Domain.Products;
using Microsoft.EntityFrameworkCore;
public class ApplicationDBContext : DbContext
{
    // transformando a classe Product em uma tabela no banco
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
    {

    }

    // modulando as tabelas adicionando campos não obrigatório e tamanhos do campo
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Product>()
        .Property(p => p.Description).HasMaxLength(255);

        builder.Entity<Product>()
        .Property(p => p.Name).IsRequired();

        builder.Entity<Category>()
        .Property(p => p.Name).IsRequired();
    }

    // modificando de forma global
    protected override void ConfigureConventions(ModelConfigurationBuilder configuration)
    {
        configuration.Properties<string>()
        .HaveMaxLength(100);
    }

}