using DemoEFCodeFirst.Data;
using DemoEFCodeFirst.Models;
using DemoEFCodeFirst.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DemoEFCodeFirst.Repositories.Implementations;

public class FilmRepository(DataContext context) // Primary constructor (C# 12)
    : Repository<Film, int>(context), IFilmRepository<Film, int>
{
    //public FilmRepository(DataContext context) : base(context)
    //{
    //}

    public override void Add(Film f)
    {
        _dbSet.Add(f);
    }

    public override async Task AddAsync(Film entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public override void Delete(int id)
    {
        Film f = GetById(id)!;
        _dbSet.Remove(f);
    }

    public override async Task DeleteAsync(int id)
    {
        Film? f = await GetByIdAsync(id);
        if (f != null)
            _dbSet.Remove(f);
    }

    public override IEnumerable<Film> GetAll()
    {
        return _dbSet
            .Include(f => f.Actors)
            .Include(f => f.Creator)
            .ToList();
    }

    public override async Task<IEnumerable<Film>> GetAllAsync()
    {
        return await _dbSet
            .Include(f => f.Actors)
            .Include(f => f.Creator)
            .ToListAsync();
    }

    public async Task<IEnumerable<Film>> GetAllFilmsAsyncNoTracking()
    {
        return await _dbSet
            .Include(f => f.Actors)
            .Include(f => f.Creator)
            .ToListAsync();
    }

    public async Task<IEnumerable<Film>> GetAllFilmsAsyncTracking()
    {
        return await _dbSet
            .AsNoTracking()
            .Include(f => f.Actors)
            .Include(f => f.Creator)
            .ToListAsync();
    }

    public override Film? GetById(int id)
    {
        return _dbSet
            .Where(f => f.Id == id)
            .Include(f => f.Actors).Include(f => f.Creator)
            .FirstOrDefault();
    }

    public override async Task<Film?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Where(f => f.Id == id)
            .Include(f => f.Actors).Include(f => f.Creator)
            .FirstOrDefaultAsync();
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
        f.Duration = entity.Duration;
        f.CreatorId = entity.CreatorId;
    }

    public override async Task UpdateAsync(int id, Film entity)
    {
        Film? f = await GetByIdAsync(id);

        if (f == null) throw new ArgumentNullException(nameof(id));

        f.Title = entity.Title;
        f.ReleasedYear = entity.ReleasedYear;
        f.Duration = entity.Duration;
        f.CreatorId = entity.CreatorId;
    }
}
