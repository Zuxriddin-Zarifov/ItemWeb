using Anjir.Zuhriddin.Items.DataAccess.Map;
using Anjir.Zuhriddin.Items.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Anjir.Zuhriddin.Items.DataAccess;

public class MainContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Item> Items { get; set; }

    public MainContext(DbContextOptions<MainContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserMap());
        modelBuilder.ApplyConfiguration(new ItemMap());
        base.OnModelCreating(modelBuilder);
    }
    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.UseSqlServer("YourConnectionStringHere");
    //    base.OnConfiguring(optionsBuilder);
    //}
}
