using Domain.Users;
using Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasMany(x => x.Operations)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);

        builder.Property(x => x.Login)
            .HasConversion(
                x => x.Value,
                y => new Login(y));

        builder.Property(x => x.AccessToken)
            .HasConversion(
                x => x.Value, 
                y => string.IsNullOrEmpty(y) ? default : new AccessToken(y));

        builder.HasIndex(x => x.Login).IsUnique();
    }
}