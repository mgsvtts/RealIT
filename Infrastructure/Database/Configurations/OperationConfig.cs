using Domain.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class OperationConfig : IEntityTypeConfiguration<Operation>
{
    public void Configure(EntityTypeBuilder<Operation> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasOne(b => b.User)
            .WithMany(p => p.Operations)
            .HasForeignKey(p => p.UserId);

        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.CreatedAt).IsDescending();
    }
}