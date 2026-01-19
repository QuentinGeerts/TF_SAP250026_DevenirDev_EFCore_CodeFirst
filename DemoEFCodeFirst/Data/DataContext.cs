using DemoEFCodeFirst.Configurations;
using DemoEFCodeFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoEFCodeFirst.Data;

/// <summary>
/// Classe DataContext : Point d'entrée principal pour interagir avec la base de données
/// Hérite de DbContext (classe de base d'Entity Framework Core)
/// </summary>
public class DataContext : DbContext
{

    // Chaque DbSet<T> correspond à une table dans la base de données
    // T est le type d'entité (classe du modèle)
    public DbSet<Actor> Actors { get; set; }
    public DbSet<Creator> Creators { get; set; }
    public DbSet<Film> Films { get; set; }
    public DbSet<FilmActor> FilmActors { get; set; }


    // ONCONFIGURING : Configuration de la connexion à la base de données
    /// <summary>
    /// Méthode appelée lors de la configuration du contexte
    /// Permet de définir la chaîne de connexion et le provider de base de données
    /// </summary>
    /// <param name="optionsBuilder">Objet permettant de configurer les options du contexte</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MovieDB;Integrated Security=True;Trust Server Certificate=True";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    // ONMODELCREATING : Configuration du modèle de données (Fluent API)
    /// <summary>
    /// Méthode appelée lors de la création du modèle de données
    /// Permet de configurer les entités, relations, contraintes via Fluent API
    /// Cette méthode est exécutée UNE SEULE FOIS lors de la première utilisation du contexte
    /// </summary>
    /// <param name="modelBuilder">Objet permettant de configurer le modèle</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FilmActor>()
            .HasNoKey();

        //modelBuilder.ApplyConfiguration(new FilmConfiguration());

        // Application automatique de TOUTES les configurations
        // ApplyConfigurationsFromAssembly : Scanne l'assembly et applique automatiquement toutes les classes qui
        // implémentent IEntityTypeConfiguration<T>
        // typeof(DataContext).Assembly : Récupère l'assembly (projet) courant (où se trouve DataContext)
        // Avantage : Plus besoin d'ajouter manuellement chaque configuration
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
    }
}
