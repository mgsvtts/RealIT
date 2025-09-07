using System.Reflection;
using Domain.Operations;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public sealed class PaymentDbContext(DbContextOptions<PaymentDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Operation> Operations { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(builder);
    }
}