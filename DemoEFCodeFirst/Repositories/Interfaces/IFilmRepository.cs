namespace DemoEFCodeFirst.Repositories.Interfaces;

// Définit les opérations spécifiques aux films en plus des opérations génériques
public interface IFilmRepository<T> : IRepository<T> where T : class
{
    IEnumerable<T> GetByReleasedYear (int releasedYear);
    Task<IEnumerable<T>> GetAllFilmsAsyncTracking();
    Task<IEnumerable<T>> GetAllFilmsAsyncNoTracking();
}
