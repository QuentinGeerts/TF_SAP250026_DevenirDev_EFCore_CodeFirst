/*
 * Démonstration EntityFrameworkCore - Approche Code First
 */

using DemoEFCodeFirst.Data;
using DemoEFCodeFirst.Models;
using DemoEFCodeFirst.Repositories.Implementations;
using DemoEFCodeFirst.Services;
using Microsoft.EntityFrameworkCore;

Console.WriteLine($"\nDémonstration EntityFrameworkCore - Approche Code First\n");


// 1.  Import des packages nugets:
// Microsoft.EntityFrameworkCore
// Microsoft.EntityFrameworkCore.SqlServer
// Microsoft.EntityFrameworkCore.Tools

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


