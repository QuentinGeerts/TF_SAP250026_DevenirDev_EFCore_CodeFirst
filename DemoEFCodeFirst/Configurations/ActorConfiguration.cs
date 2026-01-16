using DemoEFCodeFirst.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoEFCodeFirst.Configurations;

internal class ActorConfiguration : IEntityTypeConfiguration<Actor>
{
    public void Configure(EntityTypeBuilder<Actor> builder)
    {
        builder.Property(a => a.Lastname).HasMaxLength(50);
        builder.Property(a => a.Firstname).HasMaxLength(50);


        builder.HasData(
            new Actor
            {
                Id = 1,
                Lastname = "Worthington",
                Firstname = "Sam"
            },
            new Actor
            {
                Id = 2,
                Lastname = "Saldaña",
                Firstname = "Zoe"
            },
            new Actor
            {
                Id = 3,
                Lastname = "Weaver",
                Firstname = "Sigourney"
            }
        );
    }
}
