using DemoEFCodeFirst.Data;
using DemoEFCodeFirst.Models;
using DemoEFCodeFirst.Repositories.Implementations;
using DemoEFCodeFirst.Services.Interfaces;

namespace DemoEFCodeFirst.Services;

public class FilmService : IFilmService
{
    private readonly FilmRepository _filmRepository;

    public FilmService(DataContext dataContext)
    {
        _filmRepository = new FilmRepository(dataContext);
    }

    public IEnumerable<Film> GetAllFilms()
    {
        return _filmRepository.GetAll();
    }

    public IEnumerable<Film> GetAllFilmByReleasedYear(int releasedYear)
    {
        if (releasedYear < 1950) throw new ArgumentException("L'année doit être supérieure à 1950");

        return _filmRepository.GetByReleasedYear(releasedYear);
    }

    public async Task<IEnumerable<Film>> GetAllFilmsAsyncTracking()
    {
        return await _filmRepository.GetAllFilmsAsyncTracking();
    }

    public async Task<IEnumerable<Film>> GetAllFilmsAsyncNoTracking()
    {
        return await _filmRepository.GetAllFilmsAsyncTracking();
    }
}
