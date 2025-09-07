using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Database;

public class PaymentDbContextFactory : IDesignTimeDbContextFactory<PaymentDbContext>
{
    public PaymentDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PaymentDbContext>();
        
        optionsBuilder
            .UseNpgsql(
                "Server=localhost:5433;Database=real_it_database;Port=6432;User Id=postgres;Password=postgres; CommandTimeout=120;")
            .UseSnakeCaseNamingConvention();

        return new PaymentDbContext(optionsBuilder.Options);
    }
}