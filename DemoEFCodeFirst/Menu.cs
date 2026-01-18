using DemoEFCodeFirst.Data;
using DemoEFCodeFirst.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DemoEFCodeFirst;

/// <summary>
/// Classe statique pour gérer les démonstrations du projet Code First
/// </summary>
public static class Menu
{
    /// <summary>
    /// Point d'entrée principal du menu de démonstration
    /// </summary>
    /// <param name="context">Le contexte de base de données à utiliser</param>
    public static async Task AfficherMenuPrincipal(DataContext context)
    {
        bool continuer = true;

        while (continuer)
        {
            Console.Clear();
            Console.WriteLine("\nDémonstration EntityFrameworkCore - Approche Code First\n");
            Console.WriteLine();
            Console.WriteLine("1. Démonstrations CRUD de base");
            Console.WriteLine("2. Démonstrations des relations");
            Console.WriteLine("3. Démonstrations de requêtes avancées");
            Console.WriteLine("4. Démonstrations de tracking et performance");
            Console.WriteLine("5. Démonstrations de migrations et schema");
            Console.WriteLine("0. Quitter");
            Console.WriteLine();
            Console.Write("Votre choix : ");

            string choix = Console.ReadLine() ?? "";

            switch (choix)
            {
                case "1":
                    await MenuCRUD(context);
                    break;
                case "2":
                    await MenuRelations(context);
                    break;
                case "3":
                    await MenuRequetesAvancees(context);
                    break;
                case "4":
                    await MenuTrackingPerformance(context);
                    break;
                case "5":
                    await MenuMigrationsSchema(context);
                    break;
                case "0":
                    continuer = false;
                    Console.WriteLine("\nAu revoir !");
                    break;
                default:
                    Console.WriteLine("\nChoix invalide. Appuyez sur une touche...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    #region Menu CRUD
    private static async Task MenuCRUD(DataContext context)
    {
        bool retour = false;

        while (!retour)
        {
            Console.Clear();
            Console.WriteLine("\nDémonstrations CRUD de base\n");
            Console.WriteLine();
            Console.WriteLine("1. CREATE - Ajouter des données");
            Console.WriteLine("2. READ - Lire des données");
            Console.WriteLine("3. UPDATE - Modifier des données");
            Console.WriteLine("4. DELETE - Supprimer des données");
            Console.WriteLine("5. Démonstration SaveChanges vs SaveChangesAsync");
            Console.WriteLine("0. Retour au menu principal");
            Console.WriteLine();
            Console.Write("Votre choix : ");

            string choix = Console.ReadLine() ?? "";

            switch (choix)
            {
                case "1":
                    await DemoCreate(context);
                    break;
                case "2":
                    await DemoRead(context);
                    break;
                case "3":
                    await DemoUpdate(context);
                    break;
                case "4":
                    await DemoDelete(context);
                    break;
                case "5":
                    await DemoSaveChanges(context);
                    break;
                case "0":
                    retour = true;
                    break;
                default:
                    Console.WriteLine("\nChoix invalide. Appuyez sur une touche...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private static async Task DemoCreate(DataContext context)
    {
        Console.Clear();
        Console.WriteLine("\nDémonstration CREATE\n");

        // Création d'un nouvel acteur
        var actor = new Actor
        {
            Lastname = "Dujardin",
            Firstname = "Jean"
        };

        Console.WriteLine($"Ajout d'un acteur : {actor.Lastname} {actor.Firstname}");
        context.Actors.Add(actor);

        // Sauvegarder les modifications
        int result = await context.SaveChangesAsync();
        Console.WriteLine($"\n{result} enregistrement(s) ajouté(s) à la base de données.");
        Console.WriteLine($"ID généré automatiquement : {actor.Id}");

        PauseEtRetour();
    }

    private static async Task DemoRead(DataContext context)
    {
        Console.Clear();
        Console.WriteLine("\nDémonstration READ\n");

        // 1. Récupérer tous les acteurs
        Console.WriteLine("1. ToListAsync() - Récupérer tous les acteurs :");
        var actors = await context.Actors.ToListAsync();
        foreach (var actor in actors)
        {
            Console.WriteLine($"Id: {actor.Id} - Lastname: {actor.Lastname} Firstname: {actor.Firstname}");
        }

        Console.WriteLine("\n2. FirstOrDefaultAsync() - Récupérer le premier acteur :");
        var firstActor = await context.Actors.FirstOrDefaultAsync();
        if (firstActor != null)
            Console.WriteLine($"Id: {firstActor.Id} - {firstActor.Lastname} {firstActor.Firstname}");

        Console.WriteLine("\n3. FindAsync() - Recherche par clé primaire (ID=1) :");
        var actorById = await context.Actors.FindAsync(1);
        if (actorById != null)
            Console.WriteLine($"Id: {actorById.Id} - {actorById.Lastname} {actorById.Firstname}");

        Console.WriteLine("\n4. Where() - Filtrer les films sortie en 2009 :");
        var film2009 = await context.Films
            .Where(f => f.ReleasedYear == 2009)
            .ToListAsync();
        foreach (var film in film2009)
        {
            Console.WriteLine($"Id: {film.Id} - {film.Title} {film.ReleasedYear} {film.Duration} min");
        }

        PauseEtRetour();
    }

    private static async Task DemoUpdate(DataContext context)
    {
        Console.Clear();
        Console.WriteLine("\nDémonstration UPDATE\n");

        // Récupérer un acteur
        var film = await context.Films.FirstOrDefaultAsync();

        if (film == null)
        {
            Console.WriteLine("Aucun film trouvé dans la base de données.");
            PauseEtRetour();
            return;
        }

        Console.WriteLine($"Acteur trouvé : {film.Title} {film.ReleasedYear}");

        // Modifier l'acteur
        film.ReleasedYear = 2010;
        Console.WriteLine($"Nouvelle année : {film.ReleasedYear}");

        // Sauvegarder les modifications
        int result = await context.SaveChangesAsync();
        Console.WriteLine($"\n{result} enregistrement(s) modifié(s).");

        PauseEtRetour();
    }

    private static async Task DemoDelete(DataContext context)
    {
        Console.Clear();
        Console.WriteLine("\nDémonstration DELETE\n");

        // Récupérer le dernier acteur
        var actor = await context.Actors.OrderByDescending(a => a.Id).FirstOrDefaultAsync();

        if (actor == null)
        {
            Console.WriteLine("Aucun acteur trouvé dans la base de données.");
            PauseEtRetour();
            return;
        }

        Console.WriteLine($"Acteur à supprimer : Id: {actor.Id} - {actor.Lastname} {actor.Firstname}");

        // Supprimer l'acteur
        context.Actors.Remove(actor);

        // Sauvegarder les modifications
        int result = await context.SaveChangesAsync();
        Console.WriteLine($"\n{result} enregistrement(s) supprimé(s).");

        PauseEtRetour();
    }

    private static async Task DemoSaveChanges(DataContext context)
    {
        Console.Clear();
        Console.WriteLine("=== DÉMONSTRATION SaveChanges vs SaveChangesAsync ===\n");

        Console.WriteLine("SaveChanges() :");
        Console.WriteLine("  - Version synchrone (bloque le thread)");
        Console.WriteLine("  - À éviter dans les applications modernes");
        Console.WriteLine();
        Console.WriteLine("SaveChangesAsync() :");
        Console.WriteLine("  - Version asynchrone (libère le thread)");
        Console.WriteLine("  - Recommandé pour les applications web et API");
        Console.WriteLine("  - Utilise await pour attendre le résultat");
        Console.WriteLine();
        Console.WriteLine("Exemple :");
        Console.WriteLine("  int result = await context.SaveChangesAsync();");

        PauseEtRetour();
    }
    #endregion

    #region Menu Relations
    private static async Task MenuRelations(DataContext context)
    {
        bool retour = false;

        while (!retour)
        {
            Console.Clear();
            Console.WriteLine("\nDémonstrations des relations\n");
            Console.WriteLine();
            Console.WriteLine("1. One-to-Many : Film et Creator");
            Console.WriteLine("2. Many-to-Many : Film et Actor");
            Console.WriteLine("3. Include() et ThenInclude()");
            Console.WriteLine("4. Eager Loading vs Lazy Loading");
            Console.WriteLine("0. Retour au menu principal");
            Console.WriteLine();
            Console.Write("Votre choix : ");

            string choix = Console.ReadLine() ?? "";

            switch (choix)
            {
                case "1":
                    await DemoOneToMany(context);
                    break;
                case "2":
                    await DemoManyToMany(context);
                    break;
                case "3":
                    await DemoInclude(context);
                    break;
                case "4":
                    await DemoEagerLazyLoading(context);
                    break;
                case "0":
                    retour = true;
                    break;
                default:
                    Console.WriteLine("\nChoix invalide. Appuyez sur une touche...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private static async Task DemoOneToMany(DataContext context)
    {
        Console.Clear();
        Console.WriteLine("\nDémonstration One-To-Many\n");
        Console.WriteLine("Un Creator peut avoir plusieurs Films");
        Console.WriteLine("Un Film a un seul Creator\n");

        // Créer un réalisateur
        var creator = new Creator
        {
            Lastname = "Nolan",
            Firstname = "Christopher",
        };

        // Créer des films liés au réalisateur
        var film1 = new Film
        {
            Title = "Inception",
            ReleasedYear = 2010,
            Creator = creator  // Liaison via l'objet
        };

        var film2 = new Film
        {
            Title = "Interstellar",
            ReleasedYear = 2014,
            Creator = creator  // Même réalisateur
        };

        context.Films.AddRange(film1, film2);
        await context.SaveChangesAsync();

        Console.WriteLine($"Créateur ajouté : {creator.Lastname} {creator.Firstname}");
        Console.WriteLine($"  - Film 1 : {film1.Title}");
        Console.WriteLine($"  - Film 2 : {film2.Title}");
        Console.WriteLine($"\nLa clé étrangère CreatorId est automatiquement remplie par EF Core :");
        Console.WriteLine($"  - Film 1 CreatorId : {film1.CreatorId}");
        Console.WriteLine($"  - Film 2 CreatorId : {film2.CreatorId}");

        PauseEtRetour();
    }

    private static async Task DemoManyToMany(DataContext context)
    {
        Console.Clear();
        Console.WriteLine("\nDémonstration Many-To-Many\n");
        Console.WriteLine("Un Film peut avoir plusieurs Acteurs");
        Console.WriteLine("Un Acteur peut jouer dans plusieurs Films\n");

        // Récupérer ou créer un film
        var film = await context.Films.FirstOrDefaultAsync();
        if (film == null)
        {
            Console.WriteLine("Aucun film trouvé. Créez d'abord des données.");
            PauseEtRetour();
            return;
        }

        // Récupérer ou créer des acteurs
        var actor1 = await context.Actors.FirstOrDefaultAsync();
        var actor2 = await context.Actors.Skip(1).FirstOrDefaultAsync();

        if (actor1 == null || actor2 == null)
        {
            Console.WriteLine("Pas assez d'acteurs. Créez d'abord des données.");
            PauseEtRetour();
            return;
        }

        // Créer les relations via la table de jonction
        var filmActor1 = new FilmActor
        {
            FilmId = film.Id,
            ActorId = actor1.Id
        };

        var filmActor2 = new FilmActor
        {
            FilmId = film.Id,
            ActorId = actor2.Id
        };

        context.FilmActors.AddRange(filmActor1, filmActor2);
        await context.SaveChangesAsync();

        Console.WriteLine($"Film : {film.Title}");
        Console.WriteLine($"  - Acteur 1 : {actor1.Lastname} {actor1.Firstname} (Rôle : {filmActor1?.CharacterLastname} {filmActor1?.CharacterFirstname})");
        Console.WriteLine($"  - Acteur 2 : {actor2.Lastname} {actor2.Firstname} (Rôle : {filmActor2?.CharacterLastname} {filmActor2?.CharacterFirstname})");

        PauseEtRetour();
    }

    private static async Task DemoInclude(DataContext context)
    {
        Console.Clear();
        Console.WriteLine("\nDémonstration Include()\n");

        Console.WriteLine("1. Sans Include() - Les relations ne sont pas chargées :");
        var filmSansInclude = await context.Films.FirstOrDefaultAsync();
        if (filmSansInclude != null)
        {
            Console.WriteLine($" Film : {filmSansInclude.Title}");
            Console.WriteLine($" Creator : {filmSansInclude.Creator?.Lastname ?? "NULL - Non chargé!"}");
        }

        Console.WriteLine("\n2. Avec Include() - Charge la relation Creator :");
        var filmAvecInclude = await context.Films
            .Include(f => f.Creator)
            .FirstOrDefaultAsync();
        if (filmAvecInclude != null)
        {
            Console.WriteLine($" Film : {filmAvecInclude.Title}");
            Console.WriteLine($" Creator : {filmAvecInclude.Creator?.Lastname ?? "NULL"}");
        }

        PauseEtRetour();
    }

    private static async Task DemoEagerLazyLoading(DataContext context)
    {
        Console.Clear();
        Console.WriteLine("\nDémonstration Eager vs  Lazy Loading\n");

        Console.WriteLine("EAGER LOADING (recommandé) :");
        Console.WriteLine("  - Charge toutes les données en UNE requête SQL");
        Console.WriteLine("  - Utilise Include() et ThenInclude()");
        Console.WriteLine("  - Meilleure performance (1 requête au lieu de N+1)");
        Console.WriteLine();
        Console.WriteLine("Exemple :");
        Console.WriteLine("  var films = await context.Films");
        Console.WriteLine("      .Include(f => f.Creator)");
        Console.WriteLine("      .Include(f => f.Actor)");
        Console.WriteLine("      .ToListAsync();");
        Console.WriteLine();
        Console.WriteLine("LAZY LOADING :");
        Console.WriteLine("  - Charge les données à la demande (quand on y accède)");
        Console.WriteLine("  - Nécessite : UseLazyLoadingProxies()");
        Console.WriteLine("  - Risque de problème N+1 (une requête par relation)");
        Console.WriteLine("  - NON configuré dans ce projet (Eager Loading uniquement)");

        PauseEtRetour();
    }
    #endregion

    #region Menu Requêtes Avancées
    private static async Task MenuRequetesAvancees(DataContext context)
    {
        bool retour = false;

        while (!retour)
        {
            Console.Clear();
            Console.WriteLine("\nDémonstrations requêtes avancées\n");
            Console.WriteLine();
            Console.WriteLine("1. Filtres avec Where()");
            Console.WriteLine("2. Tri avec OrderBy() / OrderByDescending()");
            Console.WriteLine("3. Pagination avec Skip() et Take()");
            Console.WriteLine("4. Agrégations (Count, Sum, Average, etc.)");
            Console.WriteLine("5. Projections avec Select() et SelectMany()");
            Console.WriteLine("0. Retour au menu principal");
            Console.WriteLine();
            Console.Write("Votre choix : ");

            string choix = Console.ReadLine() ?? "";

            switch (choix)
            {
                case "1":
                    await DemoWhere(context);
                    break;
                case "2":
                    await DemoOrderBy(context);
                    break;
                case "3":
                    await DemoPagination(context);
                    break;
                case "4":
                    await DemoAgregations(context);
                    break;
                case "5":
                    await DemoProjections(context);
                    break;
                case "0":
                    retour = true;
                    break;
                default:
                    Console.WriteLine("\nChoix invalide. Appuyez sur une touche...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private static async Task DemoWhere(DataContext context)
    {
        Console.Clear();
        Console.WriteLine("\nDémonstration WHERE()\n");

        Console.WriteLine("1. Filtrage simple :");
        var filmsCourts = await context.Films
            .Where(f => f.Duration >= 180)
            .ToListAsync();
        Console.WriteLine($"   Films de plus de 3h : {filmsCourts.Count}");

        Console.WriteLine("\n2. Filtrage multiple (AND) :");
        var filmsRecents = await context.Films
            .Where(f => f.ReleasedYear > 2010 && f.Duration > 180)
            .ToListAsync();
        Console.WriteLine($"   Films récents de plus de 3h : {filmsRecents.Count}");

        Console.WriteLine("\n3. Filtrage avec Contains (LIKE SQL) :");
        var filmsInception = await context.Films
            .Where(f => f.Title.Contains("Inception"))
            .ToListAsync();
        Console.WriteLine($"   Films contenant 'Inception' : {filmsInception.Count}");

        PauseEtRetour();
    }

    private static async Task DemoOrderBy(DataContext context)
    {
        Console.Clear();
        Console.WriteLine("\nDémonstration ORDERBY()\n");

        Console.WriteLine("1. Tri croissant par titre :");
        var filmsTriesTitre = await context.Films
            .OrderBy(f => f.Title)
            .Take(5)
            .ToListAsync();
        foreach (var film in filmsTriesTitre)
        {
            Console.WriteLine($"   - {film.Title} {film.ReleasedYear} {film.Duration}");
        }

        Console.WriteLine("\n2. Tri décroissant par date :");
        var filmsTrjesDate = await context.Films
            .OrderByDescending(f => f.ReleasedYear)
            .Take(5)
            .ToListAsync();
        foreach (var film in filmsTrjesDate)
        {
            Console.WriteLine($"   - {film.Title} ({film.ReleasedYear:yyyy})");
        }

        Console.WriteLine("\n3. Tri multiple :");
        var filmsTriesMultiple = await context.Films
            .OrderByDescending(f => f.ReleasedYear)
            .ThenBy(f => f.Title)
            .Take(5)
            .ToListAsync();
        foreach (var film in filmsTriesMultiple)
        {
            Console.WriteLine($"   - {film.Title} ({film.ReleasedYear:yyyy})");
        }

        PauseEtRetour();
    }

    private static async Task DemoPagination(DataContext context)
    {
        Console.Clear();
        Console.WriteLine("\nDémonstration PAGINATION\n");

        int pageSize = 3;
        int pageNumber = 1;

        Console.WriteLine($"Taille de page : {pageSize}");
        Console.WriteLine($"Page demandée : {pageNumber}\n");

        var filmsPage = await context.Films
            .OrderBy(f => f.Title)
            .Skip((pageNumber - 1) * pageSize)  // Sauter les éléments des pages précédentes
            .Take(pageSize)                      // Prendre uniquement les éléments de la page
            .ToListAsync();

        Console.WriteLine($"Résultats (page {pageNumber}) :");
        foreach (var film in filmsPage)
        {
            Console.WriteLine($"   - {film.Title}");
        }

        var totalFilms = await context.Films.CountAsync();
        var totalPages = (int)Math.Ceiling(totalFilms / (double)pageSize);
        Console.WriteLine($"\nTotal : {totalFilms} films, {totalPages} page(s)");

        PauseEtRetour();
    }

    private static async Task DemoAgregations(DataContext context)
    {
        Console.Clear();
        Console.WriteLine("\nDémonstration AGRÉGATIONS\n");

        Console.WriteLine("1. Count() - Nombre de films :");
        var count = await context.Films.CountAsync();
        Console.WriteLine($"   Total : {count} films");

        Console.WriteLine("\n2. Average() - Durée moyenne des films :");
        var avgDuration = await context.Films.AverageAsync(f => f.Duration);
        Console.WriteLine($"   Moyenne : {avgDuration:F2} minutes");

        Console.WriteLine("\n3. Max() et Min() :");
        var maxDuration = await context.Films.MaxAsync(f => f.Duration);
        var minDuration = await context.Films.MinAsync(f => f.Duration);
        Console.WriteLine($"   Plus long : {maxDuration} min");
        Console.WriteLine($"   Plus court : {minDuration} min");

        Console.WriteLine("\n4. Any() - Vérifier l'existence :");
        var hasLongFilms = await context.Films.AnyAsync(f => f.Duration > 180);
        Console.WriteLine($"   Y a-t-il des films de plus de 3h ? {(hasLongFilms ? "Oui" : "Non")}");

        PauseEtRetour();
    }

    private static async Task DemoProjections(DataContext context)
    {
        Console.Clear();
        Console.WriteLine("\nDémonstration PROJECTIONS\n");

        Console.WriteLine("1. Select() - Projection simple :");
        var titres = await context.Films
            .Select(f => f.Title)
            .Take(5)
            .ToListAsync();
        Console.WriteLine("   Titres uniquement :");
        foreach (var titre in titres)
        {
            Console.WriteLine($"     - {titre}");
        }

        Console.WriteLine("\n2. Select() - Projection avec type anonyme :");
        var filmsSummary = await context.Films
            .Select(f => new
            {
                f.Title,
                DurationHours = f.Duration / 60.0,
                Year = f.ReleasedYear
            })
            .Take(3)
            .ToListAsync();

        foreach (var film in filmsSummary)
        {
            Console.WriteLine($"     - {film.Title} ({film.Year}) - {film.DurationHours:F1}h");
        }

        Console.WriteLine("\n3. Select() avec Include - Film et Créateur :");
        var filmsAvecCreator = await context.Films
            .Include(f => f.Creator)
            .Select(f => new
            {
                FilmTitle = f.Title,
                DirectorName = f.Creator != null ? f.Creator.Lastname + " " + f.Creator.Firstname : "Inconnu"
            })
            .Take(3)
            .ToListAsync();

        foreach (var film in filmsAvecCreator)
        {
            Console.WriteLine($"     - {film.FilmTitle} par {film.DirectorName}");
        }

        PauseEtRetour();
    }
    #endregion

    #region Menu Tracking et Performance
    private static async Task MenuTrackingPerformance(DataContext context)
    {
        bool retour = false;

        while (!retour)
        {
            Console.Clear();
            Console.WriteLine("\nDémonstrations TRACKING et PERFORMANCE");
            Console.WriteLine();
            Console.WriteLine("1. AsNoTracking() - Lecture seule optimisée");
            Console.WriteLine("2. ChangeTracker - État des entités");
            Console.WriteLine("3. Requêtes SQL générées (ToQueryString)");
            Console.WriteLine("4. Batch vs Individual SaveChanges");
            Console.WriteLine("0. Retour au menu principal");
            Console.WriteLine();
            Console.Write("Votre choix : ");

            string choix = Console.ReadLine() ?? "";

            switch (choix)
            {
                case "1":
                    await DemoAsNoTracking(context);
                    break;
                case "2":
                    await DemoChangeTracker(context);
                    break;
                case "3":
                    await DemoToQueryString(context);
                    break;
                case "4":
                    await DemoBatchSaveChanges(context);
                    break;
                case "0":
                    retour = true;
                    break;
                default:
                    Console.WriteLine("\nChoix invalide. Appuyez sur une touche...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private static async Task DemoAsNoTracking(DataContext context)
    {
        Console.Clear();
        Console.WriteLine("\nDémonstration AsNoTracking()\n");

        Console.WriteLine("1. Requête AVEC tracking (par défaut) :");
        var watch = Stopwatch.StartNew();
        var filmsTracked = await context.Films.ToListAsync();
        watch.Stop();
        Console.WriteLine($"   {filmsTracked.Count} films chargés");
        Console.WriteLine($"   Entités trackées : {context.ChangeTracker.Entries().Count()}");
        Console.WriteLine($"   Temps écoulé : {watch.ElapsedMilliseconds} ms");
        Console.WriteLine("   Usage : Quand vous allez modifier/supprimer les données");

        Console.WriteLine("\n2. Requête SANS tracking (AsNoTracking) :");
        watch.Restart();
        var filmsNoTracked = await context.Films.AsNoTracking().ToListAsync();
        watch.Stop();
        Console.WriteLine($"   {filmsNoTracked.Count} films chargés");
        Console.WriteLine($"   Entités trackées : {context.ChangeTracker.Entries().Count()}");
        Console.WriteLine($"   Temps écoulé : {watch.ElapsedMilliseconds} ms");
        Console.WriteLine("   Usage : Lecture seule, meilleures performances");

        Console.WriteLine("\nAvantages de AsNoTracking() :");
        Console.WriteLine("  ✓ Consomme moins de mémoire");
        Console.WriteLine("  ✓ Plus rapide pour les requêtes de lecture");
        Console.WriteLine("  ✓ Parfait pour les API GET, exports, rapports");
        Console.WriteLine("\nInconvénients :");
        Console.WriteLine("  ✗ Impossible de modifier directement les entités");
        Console.WriteLine("  ✗ Pas de détection automatique des changements");

        context.ChangeTracker.Clear(); // Nettoyer le tracker
        PauseEtRetour();
    }

    private static async Task DemoChangeTracker(DataContext context)
    {
        Console.Clear();
        Console.WriteLine("\nDémonstration ChangeTracker ===\n");

        // Charger un film
        var film = await context.Films.FirstOrDefaultAsync();

        if (film == null)
        {
            Console.WriteLine("Aucun film trouvé.");
            PauseEtRetour();
            return;
        }

        Console.WriteLine($"Film chargé : {film.Title}");
        var entry = context.Entry(film);
        Console.WriteLine($"État initial : {entry.State}"); // Unchanged

        // Modifier le film
        film.Title = "Nouveau Titre";
        Console.WriteLine($"\nAprès modification : {entry.State}"); // Modified

        // Annuler les changements
        entry.Reload();
        Console.WriteLine($"Après Reload() : {entry.State}"); // Unchanged
        Console.WriteLine($"Titre restauré : {film.Title}");

        Console.WriteLine("\nÉtats possibles d'une entité :");
        Console.WriteLine("  - Detached : Non suivie par le contexte");
        Console.WriteLine("  - Unchanged : Suivie, pas de changement");
        Console.WriteLine("  - Added : Nouvelle entité à insérer");
        Console.WriteLine("  - Modified : Entité modifiée à mettre à jour");
        Console.WriteLine("  - Deleted : Entité marquée pour suppression");

        PauseEtRetour();
    }

    private static async Task DemoToQueryString(DataContext context)
    {
        Console.Clear();
        Console.WriteLine("\nDémonstration ToQueryString()\n");

        Console.WriteLine("Requête LINQ :");
        Console.WriteLine("var query = context.Films");
        Console.WriteLine("    .Where(f => f.Duration > 120)");
        Console.WriteLine("    .OrderBy(f => f.Title)");
        Console.WriteLine("    .Include(f => f.Creator);");

        var query = context.Films
            .Where(f => f.Duration > 120)
            .OrderBy(f => f.Title)
            .Include(f => f.Creator);

        Console.WriteLine("\nSQL généré par EF Core :");
        Console.WriteLine("────────────────────────────────────────");
        var sql = query.ToQueryString();
        Console.WriteLine(sql);
        Console.WriteLine("────────────────────────────────────────");

        Console.WriteLine("\nUtilité de ToQueryString() :");
        Console.WriteLine("  ✓ Debugger les requêtes complexes");
        Console.WriteLine("  ✓ Optimiser les performances");
        Console.WriteLine("  ✓ Comprendre les JOIN générés");
        Console.WriteLine("  ✓ Apprendre SQL à partir de LINQ");

        PauseEtRetour();
    }

    private static async Task DemoBatchSaveChanges(DataContext context)
    {
        Console.Clear();
        Console.WriteLine("\nDémonstration Batch SaveChanges\n");

        Console.WriteLine("MAUVAISE PRATIQUE - SaveChanges dans la boucle :");
        Console.WriteLine("for (int i = 0; i < 10; i++)");
        Console.WriteLine("{");
        Console.WriteLine("    context.Actors.Add(new Actor { Name = $\"Actor {i}\" });");
        Console.WriteLine("    await context.SaveChangesAsync(); // ❌ 10 requêtes SQL!");
        Console.WriteLine("}");

        Console.WriteLine("\nBONNE PRATIQUE - Un seul SaveChanges :");
        Console.WriteLine("for (int i = 0; i < 10; i++)");
        Console.WriteLine("{");
        Console.WriteLine("    context.Actors.Add(new Actor { Name = $\"Actor {i}\" });");
        Console.WriteLine("}");
        Console.WriteLine("await context.SaveChangesAsync(); // ✓ Une seule requête batch!");

        Console.WriteLine("\nAvantages du batching :");
        Console.WriteLine("  ✓ Beaucoup plus rapide (moins d'allers-retours DB)");
        Console.WriteLine("  ✓ Transactionnel par défaut");
        Console.WriteLine("  ✓ Moins de charge sur le serveur SQL");

        PauseEtRetour();
    }
    #endregion

    #region Menu Migrations et Schema
    private static async Task MenuMigrationsSchema(DataContext context)
    {
        bool retour = false;

        while (!retour)
        {
            Console.Clear();
            Console.WriteLine("\nDémonstrations Migrations et schema\n");
            Console.WriteLine();
            Console.WriteLine("1. Commandes de migration (théorie)");
            Console.WriteLine("2. Fluent API vs Data Annotations");
            Console.WriteLine("3. Seed Data - Données initiales");
            Console.WriteLine("4. Vérifier l'état de la base de données");
            Console.WriteLine("0. Retour au menu principal");
            Console.WriteLine();
            Console.Write("Votre choix : ");

            string choix = Console.ReadLine() ?? "";

            switch (choix)
            {
                case "1":
                    DemoCommandesMigrations();
                    break;
                case "2":
                    DemoFluentVsAnnotations();
                    break;
                case "3":
                    await DemoSeedData(context);
                    break;
                case "4":
                    await DemoVerifierDB(context);
                    break;
                case "0":
                    retour = true;
                    break;
                default:
                    Console.WriteLine("\nChoix invalide. Appuyez sur une touche...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private static void DemoCommandesMigrations()
    {
        Console.Clear();
        Console.WriteLine("\nCommandes de migration\n");

        Console.WriteLine("1. Ajouter une migration :");
        Console.WriteLine("   CLI: dotnet ef migrations add NomMigration");
        Console.WriteLine("   PMC: Add-Migration NomMigration");
        Console.WriteLine("   → Génère le code de migration dans /Migrations");

        Console.WriteLine("\n2. Appliquer les migrations :");
        Console.WriteLine("   CLI: dotnet ef database update");
        Console.WriteLine("   PMC: Update-Database");
        Console.WriteLine("   → Exécute les migrations en attente sur la DB");

        Console.WriteLine("\n3. Annuler la dernière migration :");
        Console.WriteLine("   CLI: dotnet ef migrations remove");
        Console.WriteLine("   PMC: Remove-Migration");
        Console.WriteLine("   → Supprime la dernière migration NON appliquée");

        Console.WriteLine("\n4. Revenir à une migration spécifique :");
        Console.WriteLine("   CLI: dotnet ef database update NomMigration");
        Console.WriteLine("   PMC: Update-Database NomMigration");
        Console.WriteLine("   → Rollback ou forward vers cette migration");

        Console.WriteLine("\n5. Générer un script SQL :");
        Console.WriteLine("   CLI: dotnet ef migrations script");
        Console.WriteLine("   PMC: Script-Migration");
        Console.WriteLine("   → Crée un fichier .sql pour la production");

        Console.WriteLine("\n6. Lister les migrations :");
        Console.WriteLine("   CLI: dotnet ef migrations list");
        Console.WriteLine("   PMC: Get-Migration");
        Console.WriteLine("   → Affiche toutes les migrations et leur état");

        PauseEtRetour();
    }

    private static void DemoFluentVsAnnotations()
    {
        Console.Clear();
        Console.WriteLine("\nFluent API vs Data Annotations\n");

        Console.WriteLine("DATA ANNOTATIONS (dans le modèle) :");
        Console.WriteLine("─────────────────────────────────────");
        Console.WriteLine("public class Film");
        Console.WriteLine("{");
        Console.WriteLine("    [Key]");
        Console.WriteLine("    public int Id { get; set; }");
        Console.WriteLine();
        Console.WriteLine("    [Required]");
        Console.WriteLine("    [MaxLength(200)]");
        Console.WriteLine("    public string Title { get; set; }");
        Console.WriteLine("}");

        Console.WriteLine("\n\nFLUENT API (dans OnModelCreating) :");
        Console.WriteLine("─────────────────────────────────────");
        Console.WriteLine("modelBuilder.Entity<Film>(entity =>");
        Console.WriteLine("{");
        Console.WriteLine("    entity.HasKey(e => e.Id);");
        Console.WriteLine();
        Console.WriteLine("    entity.Property(e => e.Title)");
        Console.WriteLine("        .IsRequired()");
        Console.WriteLine("        .HasMaxLength(200);");
        Console.WriteLine("});");

        Console.WriteLine("\n\nCOMPARAISON :");
        Console.WriteLine("─────────────────────────────────────");
        Console.WriteLine("Data Annotations :");
        Console.WriteLine("  ✓ Plus simple et concis");
        Console.WriteLine("  ✓ Directement dans le modèle");
        Console.WriteLine("  ✗ Mélange domaine et persistence");
        Console.WriteLine("  ✗ Moins de fonctionnalités");

        Console.WriteLine("\nFluent API :");
        Console.WriteLine("  ✓ Séparation domaine/persistence");
        Console.WriteLine("  ✓ Plus puissant et flexible");
        Console.WriteLine("  ✓ Configuration centralisée");
        Console.WriteLine("  ✗ Plus verbeux");

        PauseEtRetour();
    }

    private static async Task DemoSeedData(DataContext context)
    {
        Console.Clear();
        Console.WriteLine("\nDémonstration Seed Data\n");

        Console.WriteLine("Le Seed Data permet d'insérer des données initiales lors des migrations.");

        PauseEtRetour();
    }

    private static async Task DemoVerifierDB(DataContext context)
    {
        Console.Clear();
        Console.WriteLine("\nVérification de la base de données\n");

        try
        {
            bool canConnect = await context.Database.CanConnectAsync();
            Console.WriteLine($"Connexion à la DB : {(canConnect ? "OK" : "ÉCHEC")}");

            if (canConnect)
            {
                var pendingMigrations = await context.Database.GetPendingMigrationsAsync(); // Permet de vérifier les migrations en attente
                var appliedMigrations = await context.Database.GetAppliedMigrationsAsync(); // Permet de vérifier les migrations déjà appliquées

                Console.WriteLine($"\nMigrations appliquées : {appliedMigrations.Count()}");
                foreach (var migration in appliedMigrations)
                {
                    Console.WriteLine($"  ✓ {migration}");
                }

                Console.WriteLine($"\nMigrations en attente : {pendingMigrations.Count()}");
                foreach (var migration in pendingMigrations)
                {
                    Console.WriteLine($"  ⚠ {migration}");
                }

                if (pendingMigrations.Any())
                {
                    Console.WriteLine("\n⚠ Exécutez 'dotnet ef database update' pour appliquer les migrations.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur : {ex.Message}");
        }

        PauseEtRetour();
    }
    #endregion

    #region Helpers
    private static void PauseEtRetour()
    {
        Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...\n");
        Console.ReadKey();
    }
    #endregion
}