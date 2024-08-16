using R_StudioAPI.Models;

namespace R_StudioAPI.Repository.Interfaces
{
    public interface IRepository<T> : IDisposable where T : class
    {
        IEnumerable<T> GetList();
        T? Get(long id);
        Task Create(T item);
        void Update(T item);
        void Delete(long id);
        Task Save();
    }
}
