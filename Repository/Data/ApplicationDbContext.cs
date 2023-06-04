using Microsoft.EntityFrameworkCore;
using AppRequest.Service.Products;
using Flunt.Notifications;
namespace AppRequest.Repository.Data;



public class ApplicationDbContext : DbContext {
    public DbSet<Product> Products {get;set;}
    public DbSet<Category> Categories { get;set;}
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options){}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<Notification>();
        modelBuilder.Entity<Product>().Property(p => p.Description).HasMaxLength(500).IsRequired(false);
        modelBuilder.Entity<Product>().Property(p => p.Name).IsRequired(true);
        modelBuilder.Entity<Product>().Property(p=> p.Name).IsRequired(true);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurations)
    {
        //para default no database
        configurations.Properties<string>().HaveMaxLength(100);

        
    }

}
