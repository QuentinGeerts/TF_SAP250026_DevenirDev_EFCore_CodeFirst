using DemoEFCodeFirst.Data;
using DemoEFCodeFirst.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DemoEFCodeFirst.Repositories.Implementations;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly DataContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(DataContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public void Add(T f)
    {
        _dbSet.Add(f);
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<T> GetAll()
    {
        throw new NotImplementedException();
    }

    public T? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(int id, T entity)
    {
        throw new NotImplementedException();
    }
}
