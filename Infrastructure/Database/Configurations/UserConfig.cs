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
        
        builder.HasMany(b => b.Operations)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId);

        builder.HasIndex(x => x.Login).IsUnique();
    }
}