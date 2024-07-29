using R_StudioAPI.Models;

namespace R_StudioAPI.Repository
{
    public interface IPostRepository : IRepository<Post>
    {
        IEnumerable<Post> GetPosts(int page, int pageSize);
    }
}
