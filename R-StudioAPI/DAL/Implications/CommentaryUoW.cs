using R_StudioAPI.DAL.Interfaces;
using R_StudioAPI.Data;
using R_StudioAPI.Repository.Interfaces;

namespace R_StudioAPI.DAL.Implications
{
    public class CommentaryUoW : BaseUow, ICommentaryUoW
    {
        private readonly ApplicationDbContext _context;
        private readonly IPostRepository _postRepository;
        private readonly ICommentaryRepository _commentaryRepository;
        private readonly IVideoRepository _videoRepository;

        public CommentaryUoW(ApplicationDbContext context, IPostRepository postRepository, ICommentaryRepository commentaryRepository, IVideoRepository videoRepository)
            : base(context)
        {
            _context = context;
            _postRepository = postRepository;
            _commentaryRepository = commentaryRepository;
            _videoRepository = videoRepository;
        }

        public IPostRepository PostRepository => _postRepository;

        public ICommentaryRepository CommentaryRepository => _commentaryRepository;

        public IVideoRepository VideoRepository => _videoRepository;
    }
}
