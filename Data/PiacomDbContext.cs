    using ASP.NET_Core_MVC_Piacom.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Core_MVC_Piacom.Data;

public class PiacomDbContext : DbContext
{
    public PiacomDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<User> Users { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<Price> Prices { get; set; }
    public DbSet<PriceDetail> PriceDetails { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Unit> Units { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<CreditLimit> CreditLimits { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}