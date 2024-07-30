using R_StudioAPI.Models;

namespace R_StudioAPI.Repository
{
    public interface ICommentaryRepository : IRepository<Commentary>
    {
        IEnumerable<Commentary> GetListByPostId(long postId, int page, int pageSize);
        IEnumerable<Commentary> GetListByVideoId(long videoId, int page, int pageSize);
    }
}
