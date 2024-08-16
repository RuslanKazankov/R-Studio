using Microsoft.EntityFrameworkCore;
using R_StudioAPI.DAL.Interfaces;
using R_StudioAPI.Data;

namespace R_StudioAPI.DAL.Implications
{
    public abstract class BaseUow : IBaseUoW
    {
        private readonly ApplicationDbContext _context;
        public BaseUow(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
