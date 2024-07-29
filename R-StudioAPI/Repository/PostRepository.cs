using R_StudioAPI.Data;
using R_StudioAPI.Models;
using R_StudioAPI.Repository;
using System.Collections;

namespace R_StudioAPI.Repository
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        private readonly ApplicationDbContext _context;
        public PostRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Post> GetPosts(int page, int pageSize)
        {
            return _context.Posts.OrderByDescending(p => p.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
        } 
    }
}
