namespace DemoEFCodeFirst.Repositories.Interfaces;

// Définit les opérations spécifiques aux films en plus des opérations génériques
public interface IFilmRepository<T, U> : IRepository<T, U> where T : class
{
    IEnumerable<T> GetByReleasedYear (U releasedYear);
    Task<IEnumerable<T>> GetAllFilmsAsyncTracking();
    Task<IEnumerable<T>> GetAllFilmsAsyncNoTracking();
}
