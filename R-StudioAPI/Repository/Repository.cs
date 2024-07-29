
using Microsoft.Extensions.Logging;
using R_StudioAPI.Data;

namespace R_StudioAPI.Repository
{
    public class Repository<T> : IRepository<T> where T: class
    {
        private bool disposedValue;
        private readonly ApplicationDbContext _context;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Create(T item)
        {
            await _context.Set<T>().AddAsync(item);
        }

        public void Delete(long id)
        {
            var willBeDeletedEntity = _context.Set<T>().Find(id);

            if (willBeDeletedEntity != null)
            {
                _context.Set<T>().Remove(willBeDeletedEntity);
            }
        }

        public T? Get(long id)
        {
            return _context.Set<T>().Find(id);
        }

        public IEnumerable<T> GetList()
        {
            return _context.Set<T>().ToList();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(T item)
        {
            _context.Set<T>().Update(item);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить метод завершения
                // TODO: установить значение NULL для больших полей
                disposedValue = true;
            }
        }

        // // TODO: переопределить метод завершения, только если "Dispose(bool disposing)" содержит код для освобождения неуправляемых ресурсов
        // ~Repository()
        // {
        //     // Не изменяйте этот код. Разместите код очистки в методе "Dispose(bool disposing)".
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки в методе "Dispose(bool disposing)".
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
