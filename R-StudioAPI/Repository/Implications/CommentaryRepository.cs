using Microsoft.EntityFrameworkCore;
using R_StudioAPI.Data;
using R_StudioAPI.Models;
using R_StudioAPI.Repository.Interfaces;

namespace R_StudioAPI.Repository.Implications
{
    public class CommentaryRepository : Repository<Commentary>, ICommentaryRepository
    {
        private readonly ApplicationDbContext _context;
        public CommentaryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Commentary> GetListByPostId(long postId, int page, int pageSize)
        {
            return _context.Commentaries
                .Where(c => c.PostId == postId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public IEnumerable<Commentary> GetListByVideoId(long videoId, int page, int pageSize)
        {
            return _context.Commentaries
                .Where(c => c.VideoId == videoId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
    }
}
