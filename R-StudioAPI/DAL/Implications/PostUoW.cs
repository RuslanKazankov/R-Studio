using R_StudioAPI.DAL.Interfaces;
using R_StudioAPI.Data;
using R_StudioAPI.Repository.Interfaces;

namespace R_StudioAPI.DAL.Implications
{
    public class PostUoW : BaseUow, IPostUoW
    {
        private readonly ApplicationDbContext _context;
        private readonly IPostRepository _postRepository;
        private readonly IPostMediaRepository _postMediaRepository;

        public PostUoW(ApplicationDbContext context, IPostRepository postRepository, IPostMediaRepository postMediaRepository) : base(context)
        {
            _context = context;
            _postRepository = postRepository;
            _postMediaRepository = postMediaRepository;
        }

        public IPostRepository PostRepository => _postRepository;
        public IPostMediaRepository PostMediaRepository => _postMediaRepository;
    }
}
