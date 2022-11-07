namespace Tryitter.Data;

using Microsoft.EntityFrameworkCore;
using Tryitter.Data.Mappings;
using Tryitter.Models;

public class TryitterContext : DbContext
{
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Post> Posts { get; set; }

    public TryitterContext(DbContextOptions<TryitterContext> options) : base(options) {}
    public TryitterContext() {}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = Environment.GetEnvironmentVariable("DOTNET_CONNECTION_STRING");

            if (connectionString == null)
            {
                throw new InvalidOperationException("Connection string not found");
            }

            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserMap());
        modelBuilder.ApplyConfiguration(new PostMap());
    }
}
