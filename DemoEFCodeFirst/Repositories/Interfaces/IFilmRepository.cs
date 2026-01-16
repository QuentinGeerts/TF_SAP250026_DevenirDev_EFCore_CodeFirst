namespace DemoEFCodeFirst.Repositories.Interfaces;

public interface IFilmRepository<T> : IRepository<T> where T : class
{
    IEnumerable<T> GetByReleasedYear (int releasedYear);
}
