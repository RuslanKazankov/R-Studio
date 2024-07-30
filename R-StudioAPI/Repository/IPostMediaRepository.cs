using R_StudioAPI.Models;

namespace R_StudioAPI.Repository
{
    public interface IPostMediaRepository : IRepository<PostMedia>
    {
        void RemoveList(IEnumerable<PostMedia> postMedias);
    }
}
