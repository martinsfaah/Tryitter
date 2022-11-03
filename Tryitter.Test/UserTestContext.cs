using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tryitter.Models;
using Tryitter.Repositories;

namespace Tryitter.Test;

public class TryitterTestContext : DbContext
{
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Post> Posts { get; set; }
    public TryitterTestContext(DbContextOptions<TryitterTestContext> options)
            : base(options)
    { }
}