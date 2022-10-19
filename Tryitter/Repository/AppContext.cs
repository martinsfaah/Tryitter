using Microsoft.EntityFrameworkCore;
using tryitter.Models;

namespace tryitter.Repository
{
    public class TryitterContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Post> Posts { get; set; }

        public TryitterContext(DbContextOptions<TryitterContext> options) : base(options) { }
        public TryitterContext() { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=127.0.0.1;Database=Tryitter;User=SA;Password=Password12!;");
            }
        }
    }
}