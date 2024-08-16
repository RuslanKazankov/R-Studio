using R_StudioAPI.Data;
using R_StudioAPI.Models;
using R_StudioAPI.Repository.Interfaces;

namespace R_StudioAPI.Repository.Implications
{
    public class VideoRepository : Repository<Video>, IVideoRepository
    {
        public VideoRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
