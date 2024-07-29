using R_StudioAPI.Data;
using R_StudioAPI.Models;

namespace R_StudioAPI.Repository
{
    public class PostMediaRepository : Repository<PostMedia>, IPostMediaRepository
    {
        private readonly ApplicationDbContext _context;
        public PostMediaRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
