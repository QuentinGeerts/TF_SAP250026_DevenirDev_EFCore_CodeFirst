using DemoEFCodeFirst.Data;
using DemoEFCodeFirst.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DemoEFCodeFirst.Repositories.Implementations;

public abstract class Repository<T> : IRepository<T> where T : class
{
    protected readonly DataContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(DataContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public abstract void Add(T f);

    public abstract void Delete(int id);

    public abstract IEnumerable<T> GetAll();

    public abstract T? GetById(int id);

    public abstract void Update(int id, T entity);
}
