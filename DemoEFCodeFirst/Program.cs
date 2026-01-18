/*
 * Démonstration EntityFrameworkCore - Approche Code First
 */

using DemoEFCodeFirst.Data;
using DemoEFCodeFirst.Services;

Console.WriteLine($"\nDémonstration EntityFrameworkCore - Approche Code First\n");


// 1.  Import des packages nugets:
// →  Microsoft.EntityFrameworkCore: Pour le cœur d'Entity Framework Core
// →  Microsoft.EntityFrameworkCore.SqlServer: Pour le provider SQL Server
// →  Microsoft.EntityFrameworkCore.Tools: Pour les outils de migration et gestion de la base de données
// →  Microsoft.EntityFrameworkCore.Design: Pour les outils de conception (optionnel)

// 2.  Création des classes du modèle (Dossier Models)

// 3.  Création du DataContext (Dossier Data)
// 3.1.  Héritage de DbContext
// 3.2.  Définition des DbSet<T> pour chaque entité
// 3.3.  Configuration du modèle (OnModelCreating)
// → Utilisation de la Fluent API pour configurer les entités dans des classes séparées (Dossier Configurations) [peut être réalisé plus tard après avoir faire la base]
// 3.4.  Application des configurations via ApplyConfigurationsFromAssembly dans la méthode OnModelCreating

// 4.  Configuration de la chaîne de connexion (OnConfiguring)

// 5.  Création de la base de données via les migrations

// Pour installer le CLI de EF Core (si pas déjà fait): [utilisation dans le terminal et non dans PMC]
// → dotnet tool install --global dotnet-ef
// Pour le mettre à jour:
// → dotnet tool update --global dotnet-ef

// 5.1.  Ajout de la migration initiale
// CLI: dotnet ef migrations add InitialCreate 
// PMC: Add-Migration InitialCreate

// 5.2.  Mise à jour de la base de données
// CLI: dotnet ef database update
// PMC: Update-Database

// 6. Création des repositories (Dossier Repositories) [peut être optionnel et hardcodé dans les services]
// 6.1.  Utilisation du pattern Repository pour abstraire les opérations CRUD

// 7.  Création des services (Dossier Services)
// 7.1.  Logique métier et utilisation des repositories




// -- Exemples ---

//using (DataContext context = new DataContext())
//{
//	var films = context.Films.Include(f => f.Creator).Include(f => f.Actors);

//    foreach (Film f in films)
//	{
//        Console.WriteLine($"Film: {f.Title}, " +
//			$"réalisateur: {f.Creator.Firstname} {f.Creator.Lastname}, " +
//			$"nombre d'acteurs: {f.Actors.Count}");

//		foreach (var a in f.Actors)
//		{
//            Console.WriteLine($" - {a.Firstname} {a.Lastname}");
//		}
//        Console.WriteLine();
//	}
//}

using (DataContext context = new DataContext())
{
    FilmService filmService = new FilmService(context);

    using (var transaction = context.Database.BeginTransaction())
    {

        try
        {
            //filmRepository.Add(new Film { Title = "Seigneur des Anneaux", ReleasedYear = 2001, CreatorId = 1 });
            //context.SaveChanges();

            var films = filmService.GetAllFilmByReleasedYear(2009);

            foreach (var f in films)
            {
                Console.WriteLine($"{f.Title} {f.ReleasedYear} - {f.Creator.Lastname} {f.Creator.Firstname}");

                Console.WriteLine($"Liste des acteurs :");
                foreach (var a in f.Actors)
                {
                    Console.WriteLine($"{a.Lastname} {a.Firstname}");
                }
            }

            transaction.Commit();
            Console.WriteLine($"> Commit");
        }
        catch (Exception)
        {
            transaction.Rollback();
            Console.WriteLine($"> Rollback");
            throw;
        }

    }


}


