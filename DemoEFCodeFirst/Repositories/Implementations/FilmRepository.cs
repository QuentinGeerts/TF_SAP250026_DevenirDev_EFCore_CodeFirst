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

    public override void Add(Film f)
    {
        _dbSet.Add(f);
    }

    public override void Delete(int id)
    {
        Film f = GetById(id)!;
        _dbSet.Remove(f);
    }

    public override IEnumerable<Film> GetAll()
    {
        return _dbSet
            .Include(f => f.Actors)
            .Include(f => f.Creator);
    }

    public override Film? GetById(int id)
    {
        return _dbSet
            .Where(f => f.Id == id)
            .Include(f => f.Actors).Include(f => f.Creator)
            .FirstOrDefault() ?? throw new ArgumentOutOfRangeException(nameof(id));
    }

    public IEnumerable<Film> GetByReleasedYear(int releasedYear)
    {
        return _dbSet
            .Where(f => f.ReleasedYear == releasedYear)
            .Include(f => f.Actors)
            .Include(f => f.Creator);
    }

    public override void Update(int id, Film entity)
    {
        Film f = GetById(id)!;
        f.Title = entity.Title;
        f.ReleasedYear = entity.ReleasedYear;
        f.CreatorId = entity.CreatorId;
    }
}
