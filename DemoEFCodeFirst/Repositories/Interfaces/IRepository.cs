using DemoEFCodeFirst.Models;

namespace DemoEFCodeFirst.Repositories.Interfaces;

// Définit les opérations CRUD génériques pour une entité T

public interface IRepository<TEntity, TKey> where TEntity : class
{
    // CRUD
    // C: Create
    // R: Read (ReadAll / ReadById)
    // U: Update
    // D: Delete

    void Add(TEntity f);
    IEnumerable<TEntity> GetAll();
    TEntity? GetById(TKey id);
    void Update(TKey id, TEntity entity);
    void Delete(TKey id);

    // Version asynchrone

    Task AddAsync(TEntity entity);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(TKey id);
    Task UpdateAsync(TKey id, TEntity entity);
    Task DeleteAsync(TKey id);

}
