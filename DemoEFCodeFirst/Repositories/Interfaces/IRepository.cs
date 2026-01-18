using DemoEFCodeFirst.Models;

namespace DemoEFCodeFirst.Repositories.Interfaces;

// Définit les opérations CRUD génériques pour une entité T

public interface IRepository<T> where T : class
{
    // CRUD
    // C: Create
    // R: Read (ReadAll / ReadById)
    // U: Update
    // D: Delete

    void Add(T f);
    IEnumerable<T> GetAll();
    T? GetById(int id);
    void Update(int id, T entity);
    void Delete(int id);

}
