using DemoEFCodeFirst.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoEFCodeFirst.Configurations;

public class FilmConfiguration : IEntityTypeConfiguration<Film>
{
    public void Configure(EntityTypeBuilder<Film> builder)
    {
        builder.ToTable("Films", schema =>
        {
            schema.HasCheckConstraint("CK_Film_ReleasedYear_Before1950", "ReleasedYear >= 1950");
        });

        // Configuration de la clef primaire
        builder.HasKey(f => f.Id);
        //builder.Property(f => f.Id).ValueGeneratedNever(); // Pour ne pas auto-incrémenter

        // Configuration des colonnes
        builder.Property(f => f.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(f => f.ReleasedYear)
            .IsRequired()
            .HasComment("L'année de sortie du film");

        builder.HasOne(c => c.Creator).WithMany(f => f.Films)
            .HasForeignKey(f => f.CreatorId);

        builder.HasMany(f => f.Actors)
            .WithMany(a => a.Films)
            .UsingEntity<FilmActor>(builder =>
            {
                builder.HasKey(fa => new { fa.ActorId, fa.FilmId });

                builder.HasData(
                    new FilmActor
                    {
                        ActorId = 1,
                        FilmId = 1
                    },
                    new FilmActor
                    {
                        ActorId = 2,
                        FilmId = 1
                    },
                    new FilmActor
                    {
                        ActorId = 3,
                        FilmId = 1
                    },
                    new FilmActor
                    {
                        ActorId = 1,
                        FilmId = 2
                    },
                    new FilmActor
                    {
                        ActorId = 2,
                        FilmId = 2
                    },
                    new FilmActor
                    {
                        ActorId = 1,
                        FilmId = 3
                    },
                    new FilmActor
                    {
                        ActorId = 2,
                        FilmId = 3
                    }
                );
            });

        builder.HasData(
            new Film
            {
                Id = 1,
                Title = "Avatar",
                ReleasedYear = 2009,
                CreatorId = 1
            },   
            new Film
            {
                Id = 2,
                Title = "Avatar 2",
                ReleasedYear = 2022,
                CreatorId = 1
            },   
            new Film
            {
                Id = 3,
                Title = "Avatar 3",
                ReleasedYear = 2025,
                CreatorId = 1
            }
        );

    }
}
