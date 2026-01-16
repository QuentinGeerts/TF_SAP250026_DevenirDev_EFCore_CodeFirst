using DemoEFCodeFirst.Data;
using DemoEFCodeFirst.Models;
using DemoEFCodeFirst.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DemoEFCodeFirst.Repositories.Implementations;

public class FilmRepository : Repository<Film>, IFilmRepository<Film>
{
    public FilmRepository(DataContext context) : base(context)
    {
    }

    public IEnumerable<Film> GetByReleasedYear(int releasedYear)
    {
        return _dbSet
            .Where(f => f.ReleasedYear == releasedYear)
            .Include(f => f.Actors)
            .Include(f => f.Creator);
    }
}
