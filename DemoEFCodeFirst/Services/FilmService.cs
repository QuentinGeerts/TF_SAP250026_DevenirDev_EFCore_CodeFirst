using DemoEFCodeFirst.Data;
using DemoEFCodeFirst.Models;
using DemoEFCodeFirst.Repositories.Implementations;

namespace DemoEFCodeFirst.Services;

public class FilmService
{
    private readonly FilmRepository _filmRepository;

    public FilmService(DataContext dataContext)
    {
        _filmRepository = new FilmRepository(dataContext);
    }

    public IEnumerable<Film> GetAllFilmByReleasedYear(int releasedYear)
    {
        if (releasedYear < 1950) throw new ArgumentException("L'année doit être supérieure à 1950");

        return _filmRepository.GetByReleasedYear(releasedYear);
    }
}
