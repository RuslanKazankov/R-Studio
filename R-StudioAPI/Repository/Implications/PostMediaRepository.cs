using Microsoft.Extensions.Options;
using R_StudioAPI.Config;
using R_StudioAPI.Data;
using R_StudioAPI.Models;
using R_StudioAPI.Repository.Interfaces;

namespace R_StudioAPI.Repository.Implications
{
    public class PostMediaRepository : Repository<PostMedia>, IPostMediaRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ApplicationConfig _appConfig;
        public PostMediaRepository(ApplicationDbContext context, IOptions<ApplicationConfig> appConfig) : base(context)
        {
            _context = context;
            _appConfig = appConfig.Value;
        }

        public void RemoveList(IEnumerable<PostMedia> postMedia)
        {
            string directory = $"{Directory.GetCurrentDirectory()}/{_appConfig.PathPostMedia}";
            if (!Path.Exists(directory))
                return;

            foreach (PostMedia media in postMedia)
            {
                if (File.Exists($"{directory}{media.Url}"))
                {
                    File.Delete($"{directory}{media.Url}");
                }
            }

            _context.RemoveRange(postMedia);
        }
    }
}
