# 📚 Démonstration Entity Framework Core - Approche Code First

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-12.0-239120?style=for-the-badge&logo=csharp&logoColor=white)
![Entity Framework Core](https://img.shields.io/badge/EF%20Core-10.0.2-512BD4?style=for-the-badge&logo=nuget&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-LocalDB-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)
![License](https://img.shields.io/badge/License-Educational-green?style=for-the-badge)

> Projet pédagogique illustrant les concepts fondamentaux d'Entity Framework Core avec l'approche Code First

## 🎯 Objectifs pédagogiques

Ce projet de démonstration vise à maîtriser :
- ✅ L'approche **Code First** avec Entity Framework Core
- ✅ La configuration des entités avec **Fluent API**
- ✅ Les **migrations** de base de données
- ✅ Les relations entre entités (One-to-One, One-to-Many, Many-to-Many)
- ✅ Le **pattern Repository**
- ✅ L'organisation en couches (Models, Data, Repositories, Services)

## 📋 Table des matières

- [Prérequis](#-prérequis)
- [Structure du projet](#-structure-du-projet)
- [Concepts clés](#-concepts-clés)
- [Installation et configuration](#-installation-et-configuration)
- [Les relations entre entités](#-les-relations-entre-entités)
- [Les migrations](#-les-migrations)
- [Utilisation](#-utilisation)
- [Pour aller plus loin](#-pour-aller-plus-loin)

## 🔧 Prérequis

- **.NET 10.0** ou supérieur
- **SQL Server LocalDB** (inclus avec Visual Studio)
- **Entity Framework Core Tools** (CLI ou PMC)

### Installation de l'outil CLI EF Core

```bash
# Installation
dotnet tool install --global dotnet-ef

# Mise à jour
dotnet tool update --global dotnet-ef

# Vérification
dotnet ef --version
```

## 📁 Structure du projet

```
DemoEFCodeFirst/
├── 📂 Models/                    # Entités du domaine
│   ├── Actor.cs
│   ├── Creator.cs
│   ├── Film.cs
│   └── FilmActor.cs             # Table de jonction Many-to-Many
├── 📂 Data/
│   └── DataContext.cs           # Contexte EF Core
├── 📂 Configurations/           # Configuration Fluent API
│   ├── ActorConfiguration.cs
│   ├── CreatorConfiguration.cs
│   └── FilmConfiguration.cs
├── 📂 Repositories/             # Pattern Repository
│   ├── Interfaces/
│   │   ├── IRepository.cs       # Interface générique CRUD
│   │   └── IFilmRepository.cs   # Interface spécifique Films
│   └── Implementations/
│       ├── Repository.cs        # Repository générique abstrait
│       └── FilmRepository.cs    # Repository Films
├── 📂 Services/                 # Logique métier
│   ├── Interfaces/
│   │   └── IFilmService.cs
│   └── FilmService.cs
├── 📂 Migrations/               # Historique des migrations
└── Program.cs                   # Point d'entrée
```

## 💡 Concepts clés

### 1. Code First

L'approche **Code First** consiste à :
1. Définir les classes C# (modèles)
2. Configurer les relations et contraintes
3. Générer automatiquement la base de données via les migrations

### 2. DataContext

Le `DataContext` est le point central d'interaction avec la base de données :

```csharp
public class DataContext : DbContext
{
    // Chaque DbSet<T> représente une table
    public DbSet<Actor> Actors { get; set; }
    public DbSet<Creator> Creators { get; set; }
    public DbSet<Film> Films { get; set; }
    public DbSet<FilmActor> FilmActors { get; set; }
}
```

**Méthodes importantes :**
- **OnConfiguring** : Configure la chaîne de connexion
- **OnModelCreating** : Configure le modèle via Fluent API

### 3. Fluent API vs Data Annotations

Ce projet utilise la **Fluent API** pour la configuration :

**Avantages :**
- ✅ Séparation des préoccupations (configuration externe aux modèles)
- ✅ Plus puissante et flexible que les Data Annotations
- ✅ Code plus propre dans les entités
- ✅ Configuration centralisée

**Exemple de configuration :**

```csharp
public class FilmConfiguration : IEntityTypeConfiguration<Film>
{
    public void Configure(EntityTypeBuilder<Film> builder)
    {
        builder.Property(f => f.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.ToTable("Films", schema =>
        {
            schema.HasCheckConstraint("CK_Film_ReleasedYear_Before1950", 
                                      "ReleasedYear >= 1950");
        });
    }
}
```

### 4. Pattern Repository

**Abstraction de la couche d'accès aux données** :

- `IRepository<T>` : Interface générique avec opérations CRUD
- `Repository<T>` : Implémentation abstraite de base
- `IFilmRepository<T>` : Interface spécifique aux films
- `FilmRepository` : Implémentation concrète avec méthodes personnalisées

**Avantages :**
- ✅ Testabilité (possibilité de mock)
- ✅ Réutilisabilité du code
- ✅ Séparation des responsabilités (SRP)
- ✅ Facilite le changement de provider (SQL Server → PostgreSQL, etc.)

### 5. Seed Data (données initiales)

Utilisation de `HasData()` pour pré-remplir la base :

```csharp
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
    }
);
```

**⚠️ Important :** Avec `HasData()`, vous devez **spécifier manuellement les clés primaires**.

## 🔗 Les relations entre entités

Entity Framework Core supporte trois types de relations. Voici comment les configurer avec la **Fluent API**.

### 1️⃣ Relation One-to-One (1:1)

**Exemple : Film ↔ FilmDetail**

Un film a **un seul** détail, et un détail appartient à **un seul** film.

#### Modèles

```csharp
public class Film
{
    public int Id { get; set; }
    public string Title { get; set; }
    
    // Navigation property
    public FilmDetail? Detail { get; set; }
}

public class FilmDetail
{
    public int Id { get; set; }
    public string Synopsis { get; set; }
    public int Duration { get; set; }
    
    // Foreign Key
    public int FilmId { get; set; }
    
    // Navigation property
    public Film Film { get; set; }
}
```

#### Configuration Fluent API

```csharp
public class FilmConfiguration : IEntityTypeConfiguration<Film>
{
    public void Configure(EntityTypeBuilder<Film> builder)
    {
        // Relation One-to-One
        builder.HasOne(f => f.Detail)
            .WithOne(d => d.Film)
            .HasForeignKey<FilmDetail>(d => d.FilmId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
```

**Points clés :**
- `HasOne()` ... `WithOne()` : Définit la relation 1:1
- `HasForeignKey<T>()` : Spécifie la table dépendante (avec la FK)
- La FK peut être optionnelle ou obligatoire selon le type (`int?` vs `int`)

### 2️⃣ Relation One-to-Many (1:N)

**Exemple : Creator → Films** *(utilisé dans le projet)*

Un créateur a **plusieurs** films, un film a **un seul** créateur.

#### Modèles

```csharp
public class Creator
{
    public int Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    
    // Navigation collection
    public ICollection<Film> Films { get; set; } = [];
}

public class Film
{
    public int Id { get; set; }
    public string Title { get; set; }
    
    // Foreign Key
    public int CreatorId { get; set; }
    
    // Navigation property
    public Creator Creator { get; set; }
}
```

#### Configuration Fluent API

```csharp
public class FilmConfiguration : IEntityTypeConfiguration<Film>
{
    public void Configure(EntityTypeBuilder<Film> builder)
    {
        // Relation One-to-Many
        builder.HasOne(f => f.Creator)          // Un film a un créateur
            .WithMany(c => c.Films)              // Un créateur a plusieurs films
            .HasForeignKey(f => f.CreatorId)     // La FK est CreatorId
            .OnDelete(DeleteBehavior.Cascade);   // Suppression en cascade
    }
}
```

**Points clés :**
- `HasOne()` ... `WithMany()` : Définit la relation 1:N
- `HasForeignKey()` : Spécifie la colonne de clé étrangère
- `OnDelete()` : Comportement lors de la suppression
  - `Cascade` : Supprime les entités liées
  - `Restrict` : Empêche la suppression si des entités liées existent
  - `SetNull` : Met la FK à NULL

### 3️⃣ Relation Many-to-Many (N:N)

**Exemple : Films ↔ Actors** *(utilisé dans le projet)*

Un film a **plusieurs** acteurs, un acteur joue dans **plusieurs** films.

#### Modèles

```csharp
public class Film
{
    public int Id { get; set; }
    public string Title { get; set; }
    
    // Navigation collection
    public ICollection<Actor> Actors { get; set; } = [];
}

public class Actor
{
    public int Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    
    // Navigation collection
    public ICollection<Film> Films { get; set; } = [];
}

// Table de jonction explicite
public class FilmActor
{
    public int FilmId { get; set; }
    public int ActorId { get; set; }
    
    // Navigation properties
    public Film Film { get; set; }
    public Actor Actor { get; set; }
}
```

#### Configuration Fluent API - Méthode 1 (Recommandée)

**Avec table de jonction explicite** (permet d'ajouter des propriétés supplémentaires) :

```csharp
public class FilmConfiguration : IEntityTypeConfiguration<Film>
{
    public void Configure(EntityTypeBuilder<Film> builder)
    {
        builder.HasMany(f => f.Actors)
            .WithMany(a => a.Films)
            .UsingEntity<FilmActor>(
                // Configuration de la table de jonction
                joinBuilder =>
                {
                    // Clé primaire composite
                    joinBuilder.HasKey(fa => new { fa.ActorId, fa.FilmId });
                    
                    // Relation vers Film
                    joinBuilder.HasOne(fa => fa.Film)
                        .WithMany()
                        .HasForeignKey(fa => fa.FilmId)
                        .OnDelete(DeleteBehavior.Cascade);
                    
                    // Relation vers Actor
                    joinBuilder.HasOne(fa => fa.Actor)
                        .WithMany()
                        .HasForeignKey(fa => fa.ActorId)
                        .OnDelete(DeleteBehavior.Cascade);
                    
                    // Seed data pour la table de jonction
                    joinBuilder.HasData(
                        new FilmActor { ActorId = 1, FilmId = 1 },
                        new FilmActor { ActorId = 2, FilmId = 1 },
                        new FilmActor { ActorId = 3, FilmId = 1 }
                    );
                }
            );
    }
}
```

#### Configuration Fluent API - Méthode 2 (Simple)

**Sans table de jonction explicite** (EF Core crée automatiquement la table) :

```csharp
public class FilmConfiguration : IEntityTypeConfiguration<Film>
{
    public void Configure(EntityTypeBuilder<Film> builder)
    {
        // EF Core créera automatiquement une table FilmActor
        builder.HasMany(f => f.Actors)
            .WithMany(a => a.Films);
    }
}
```

**⚠️ Limitation :** Impossible d'ajouter des colonnes supplémentaires (ex: rôle, date, ordre).

#### Quand utiliser la table de jonction explicite ?

✅ **Utilisez une table explicite si :**
- Vous avez besoin de propriétés supplémentaires (ex: `Role`, `Order`, `HiredDate`)
- Vous voulez plus de contrôle sur la configuration
- Vous avez besoin de seed data pour la relation

❌ **Utilisez la configuration simple si :**
- Aucune donnée supplémentaire n'est nécessaire
- Vous voulez une relation pure N:N

### 📊 Récapitulatif des relations

| Type | Syntaxe Fluent API | Exemple |
|------|-------------------|---------|
| **One-to-One** | `HasOne().WithOne().HasForeignKey<T>()` | Film → FilmDetail |
| **One-to-Many** | `HasOne().WithMany().HasForeignKey()` | Creator → Films |
| **Many-to-Many** | `HasMany().WithMany()` ou `UsingEntity<T>()` | Films ↔ Actors |

### 🎯 Règles de convention EF Core

Si vous ne configurez rien, EF Core applique des conventions :

1. **Clé primaire** : Propriété nommée `Id` ou `{ClassName}Id`
2. **Clé étrangère** : Propriété nommée `{NavigationPropertyName}Id`
3. **Type de relation** : Détecté automatiquement selon les navigation properties

**💡 Bonne pratique :** Toujours configurer explicitement avec Fluent API pour plus de clarté.

## 🚀 Installation et configuration

### 1. Packages NuGet installés

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.EntityFrameworkCore" Version="10.0.2" />
  <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="10.0.2" />
  <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="10.0.2">
    <PrivateAssets>all</PrivateAssets>
    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
  </PackageReference>
</ItemGroup>
```

### 2. Chaîne de connexion

Définie dans `DataContext.OnConfiguring()` :

```csharp
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    if (!optionsBuilder.IsConfigured)
    {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;
                                   Initial Catalog=MovieDB;
                                   Integrated Security=True;
                                   Trust Server Certificate=True";
        optionsBuilder.UseSqlServer(connectionString);
    }
}
```

### 3. Application des configurations

Dans `OnModelCreating()`, utilisation de `ApplyConfigurationsFromAssembly` :

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Scanne et applique automatiquement toutes les configurations
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
    
    base.OnModelCreating(modelBuilder);
}
```

**Avantage :** Plus besoin d'ajouter manuellement chaque configuration !

## 🔄 Les migrations

### Workflow complet

```
┌─────────────────┐
│ 1. Créer/Modifier│
│   les modèles   │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ 2. Ajouter une  │
│   migration     │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ 3. Vérifier le  │
│   code généré   │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ 4. Appliquer    │
│   à la BD       │
└─────────────────┘
```

### Commandes principales

#### 1. Créer une migration

```bash
# Via CLI
dotnet ef migrations add NomDeLaMigration

# Via Package Manager Console (PMC)
Add-Migration NomDeLaMigration
```

**Exemples du projet :**
```bash
dotnet ef migrations add AddConfigurations
dotnet ef migrations add AddManyToManyFilmActor
```

#### 2. Appliquer les migrations

```bash
# Via CLI
dotnet ef database update

# Via PMC
Update-Database

# Appliquer jusqu'à une migration spécifique
dotnet ef database update NomDeLaMigration
```

#### 3. Revenir en arrière

```bash
# Retour à la migration précédente
dotnet ef database update NomMigrationPrecedente

# Supprimer la dernière migration (non appliquée)
dotnet ef migrations remove
```

### Commandes utiles

```bash
# Lister toutes les migrations
dotnet ef migrations list

# Voir l'état actuel de la base
dotnet ef migrations has-pending-model-changes

# Générer un script SQL sans appliquer
dotnet ef migrations script

# Script entre deux migrations
dotnet ef migrations script Migration1 Migration2

# Supprimer la base de données
dotnet ef database drop

# Recréer la base depuis zéro
dotnet ef database drop --force
dotnet ef database update
```

### Contenu d'une migration

Chaque migration génère 3 fichiers :

```
Migrations/
├── 20260116092813_AddConfigurations.cs           # Code de migration
├── 20260116092813_AddConfigurations.Designer.cs  # Métadonnées
└── DataContextModelSnapshot.cs                    # État actuel du modèle
```

**Structure d'une migration :**

```csharp
public partial class AddConfigurations : Migration
{
    // Appliquer les changements
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Films",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Title = table.Column<string>(maxLength: 100, nullable: false),
                // ...
            });
    }

    // Annuler les changements
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Films");
    }
}
```

## 📖 Utilisation

### Exemple 1 : Requête avec Include (Eager Loading)

```csharp
using (DataContext context = new DataContext())
{
    // Chargement des relations avec Include()
    var films = context.Films
        .Include(f => f.Creator)      // Charge le créateur
        .Include(f => f.Actors);      // Charge les acteurs

    foreach (Film f in films)
    {
        Console.WriteLine($"Film: {f.Title}");
        Console.WriteLine($"Réalisateur: {f.Creator.Firstname} {f.Creator.Lastname}");
        Console.WriteLine($"Nombre d'acteurs: {f.Actors.Count}");

        foreach (var actor in f.Actors)
        {
            Console.WriteLine($"  - {actor.Firstname} {actor.Lastname}");
        }
        Console.WriteLine();
    }
}
```

### Exemple 2 : Utilisation du Service + Repository

```csharp
using (DataContext context = new DataContext())
{
    FilmService filmService = new FilmService(context);

    // Appel à la logique métier
    var films = filmService.GetAllFilmByReleasedYear(2009);

    foreach (var film in films)
    {
        Console.WriteLine($"{film.Title} ({film.ReleasedYear})");
        Console.WriteLine($"Réalisateur: {film.Creator.Lastname} {film.Creator.Firstname}");
        
        Console.WriteLine("Acteurs:");
        foreach (var actor in film.Actors)
        {
            Console.WriteLine($"  • {actor.Lastname} {actor.Firstname}");
        }
        Console.WriteLine();
    }
}
```

### Exemple 3 : Transaction manuelle

```csharp
using (DataContext context = new DataContext())
{
    FilmService filmService = new FilmService(context);

    // Début de transaction explicite
    using (var transaction = context.Database.BeginTransaction())
    {
        try
        {
            // Opérations sur la base de données
            var films = filmService.GetAllFilmByReleasedYear(2009);
            
            // Traitement...
            
            // Commit si tout s'est bien passé
            transaction.Commit();
            Console.WriteLine("✅ Transaction validée");
        }
        catch (Exception ex)
        {
            // Rollback en cas d'erreur
            transaction.Rollback();
            Console.WriteLine($"❌ Transaction annulée: {ex.Message}");
            throw;
        }
    }
}
```

### Exemple 4 : Ajouter un film avec acteurs

```csharp
using (DataContext context = new DataContext())
{
    // Récupérer des acteurs existants
    var actors = context.Actors
        .Where(a => a.Id == 1 || a.Id == 2)
        .ToList();

    // Créer un nouveau film
    var newFilm = new Film
    {
        Title = "Avatar 4",
        ReleasedYear = 2028,
        CreatorId = 1,
        Actors = actors  // Association Many-to-Many
    };

    context.Films.Add(newFilm);
    context.SaveChanges();
    
    Console.WriteLine($"Film '{newFilm.Title}' ajouté avec {newFilm.Actors.Count} acteurs");
}
```

## ⚠️ Points d'attention

### 1. Include() pour le chargement des relations

Sans `Include()`, les propriétés de navigation ne sont **pas chargées** (Lazy Loading désactivé par défaut) :

```csharp
// ❌ f.Creator sera NULL
var film = context.Films.FirstOrDefault();
Console.WriteLine(film.Creator.Firstname); // NullReferenceException!

// ✅ f.Creator sera chargé
var film = context.Films
    .Include(f => f.Creator)
    .FirstOrDefault();
Console.WriteLine(film.Creator.Firstname); // OK!
```

### 2. Gestion du cycle de vie du DbContext

Le `DbContext` **doit être disposé** après utilisation :

```csharp
// ✅ Bon : using garantit la libération des ressources
using (DataContext context = new DataContext())
{
    // Opérations...
} // Dispose() appelé automatiquement

// ❌ Éviter : risque de fuite mémoire
var context = new DataContext();
// ... opérations
// Oubli de context.Dispose()
```

**💡 Règle :** Un DbContext par unité de travail (requête ou transaction).

### 3. SaveChanges() est nécessaire

EF Core utilise un **pattern Unit of Work** :

```csharp
// ❌ Aucune modification en base
var film = context.Films.First();
film.Title = "Nouveau titre";
// Pas de SaveChanges() → changement perdu

// ✅ Modifications persistées
var film = context.Films.First();
film.Title = "Nouveau titre";
context.SaveChanges(); // Exécute UPDATE en base
```

### 4. Attention aux clés étrangères dans HasData()

Lors du seeding, vous devez respecter l'ordre des dépendances :

```csharp
// ✅ Bon ordre : Creator d'abord, puis Films
builder.HasData(new Creator { Id = 1, Lastname = "Cameron" });
builder.HasData(new Film { Id = 1, CreatorId = 1, Title = "Avatar" });

// ❌ Mauvais : Film référence un Creator inexistant
builder.HasData(new Film { Id = 1, CreatorId = 999, Title = "Avatar" });
```

### 5. Utiliser AsNoTracking() pour les lectures seules

Pour de meilleures performances sur les requêtes en lecture seule :

```csharp
// Plus rapide si vous ne modifiez pas les données
var films = context.Films
    .AsNoTracking()  // Pas de tracking EF Core
    .Include(f => f.Creator)
    .ToList();
```

### 6. Gérer les relations Many-to-Many existantes

```csharp
// ❌ Ajoute une nouvelle ligne en base
var film = context.Films.Include(f => f.Actors).First();
film.Actors.Add(new Actor { Id = 5, Firstname = "John", Lastname = "Doe" });

// ✅ Associe un acteur existant
var film = context.Films.Include(f => f.Actors).First();
var existingActor = context.Actors.Find(5);
film.Actors.Add(existingActor);
context.SaveChanges();
```

## 📚 Pour aller plus loin

### Concepts avancés à explorer

#### 1. Requêtes asynchrones

```csharp
public async Task<IEnumerable<Film>> GetAllFilmsAsync()
{
    return await context.Films
        .Include(f => f.Creator)
        .Include(f => f.Actors)
        .ToListAsync();
}
```

**Avantages :** Meilleure scalabilité, ne bloque pas le thread.

#### 2. Specification Pattern

Pour des requêtes complexes et réutilisables :

```csharp
public class FilmByYearSpecification : ISpecification<Film>
{
    private readonly int _year;
    
    public Expression<Func<Film, bool>> Criteria => f => f.ReleasedYear == _year;
}
```

#### 3. Unit of Work Pattern

Gérer plusieurs repositories dans une seule transaction :

```csharp
public interface IUnitOfWork : IDisposable
{
    IFilmRepository Films { get; }
    IActorRepository Actors { get; }
    Task<int> SaveChangesAsync();
}
```

#### 4. Projections avec Select()

Optimiser les performances en ne chargeant que les données nécessaires :

```csharp
var filmTitles = context.Films
    .Select(f => new { f.Title, f.ReleasedYear })
    .ToList();
```

#### 5. Global Query Filters

Filtrer automatiquement les requêtes (ex: Soft Delete) :

```csharp
modelBuilder.Entity<Film>()
    .HasQueryFilter(f => !f.IsDeleted);
```

#### 6. Shadow Properties

Propriétés gérées par EF Core mais absentes du modèle :

```csharp
builder.Property<DateTime>("CreatedAt");
builder.Property<DateTime>("UpdatedAt");
```

#### 7. Value Conversions

Convertir des types .NET vers des types SQL :

```csharp
builder.Property(f => f.Genres)
    .HasConversion(
        v => string.Join(',', v),        // Vers DB
        v => v.Split(',', StringSplitOptions.None) // Depuis DB
    );
```

#### 8. Owned Types

Pour des objets valeurs (Value Objects) :

```csharp
builder.OwnsOne(f => f.Address, address =>
{
    address.Property(a => a.Street).HasMaxLength(100);
    address.Property(a => a.City).HasMaxLength(50);
});
```

### Ressources recommandées

- 📖 [Documentation officielle EF Core](https://learn.microsoft.com/ef/core/)
- 📖 [Microsoft Learn - EF Core](https://learn.microsoft.com/training/modules/persist-data-ef-core/)

## 📝 Résumé du workflow Code First

```
1. Créer les modèles (classes C#)
                ↓
2. Configurer les entités (Fluent API)
                ↓
3. Créer le DbContext
                ↓
4. Générer une migration
   dotnet ef migrations add NomMigration / Add-Migration "nom de la migration"
                ↓
5. Appliquer la migration
   dotnet ef database update / Update-Database
                ↓
6. Utiliser le contexte (Repository/Service)
```

---

## 🏆 Bonne pratiques résumées

✅ **DO**
- Utiliser `using` pour le DbContext
- Configurer avec Fluent API dans des classes séparées
- Utiliser `Include()` pour charger les relations
- Appeler `SaveChanges()` après modifications
- Nommer les migrations de manière descriptive
- Utiliser `AsNoTracking()` pour les lectures seules

❌ **DON'T**
- Réutiliser un DbContext trop longtemps
- Oublier `SaveChanges()`
- Mélanger configuration et entités
- Utiliser Select N+1 (charger en boucle)
- Ignorer les migrations

---

**🎓 Bon apprentissage avec Entity Framework Core !**

*Projet pédagogique - 2026*
