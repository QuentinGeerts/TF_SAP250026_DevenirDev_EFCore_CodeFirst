using DemoEFCodeFirst.Data;
using DemoEFCodeFirst.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DemoEFCodeFirst.Repositories.Implementations;

public abstract class Repository<T, U>(DataContext context) : IRepository<T, U> where T : class
{
    protected readonly DataContext _context = context;
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    //public Repository(DataContext context)
    //{
    //    //_context = context;
    //    _dbSet = _context.Set<T>();
    //}

    public abstract void Add(T f);
    public abstract Task AddAsync(T entity);
    public abstract void Delete(U id);
    public abstract Task DeleteAsync(U id);
    public abstract IEnumerable<T> GetAll();
    public abstract Task<IEnumerable<T>> GetAllAsync();
    public abstract T? GetById(U id);
    public abstract Task<T?> GetByIdAsync(U id);
    public abstract void Update(U id, T entity);
    public abstract Task UpdateAsync(U id, T entity);
}
