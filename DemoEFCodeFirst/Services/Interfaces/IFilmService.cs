using DemoEFCodeFirst.Models;

namespace DemoEFCodeFirst.Services.Interfaces;

// Définit toutes les opérations possibles sur les films
public interface IFilmService
{

    IEnumerable<Film> GetAllFilms();
    IEnumerable<Film> GetAllFilmByReleasedYear(int releasedYear);

}
