using DemoEFCodeFirst.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoEFCodeFirst.Configurations;

internal class CreatorConfiguration : IEntityTypeConfiguration<Creator>
{
    public void Configure(EntityTypeBuilder<Creator> builder)
    {
        builder.Property(c => c.Lastname)
            .HasMaxLength(50);

        builder.Property(c => c.Firstname)
           .HasMaxLength(50);

        builder.HasData(
            new Creator
            {
                Id = 1,
                Lastname = "Cameron",
                Firstname = "James"
            }    
        );
    }
}
