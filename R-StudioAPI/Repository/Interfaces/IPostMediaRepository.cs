using R_StudioAPI.Models;

namespace R_StudioAPI.Repository.Interfaces
{
    public interface IPostMediaRepository : IRepository<PostMedia>
    {
        void RemoveList(IEnumerable<PostMedia> postMedias);
    }
}
