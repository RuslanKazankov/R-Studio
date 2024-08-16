namespace R_StudioAPI.Services.Implications
{
    public class MediaService : IMediaService
    {
        private readonly string[] extensions = [".jpg", ".jpeg", ".png", ".gif", ".mp4", ".avi", ".mov"];
        private readonly Dictionary<string, string> types = new()
        {
            { ".jpg", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".png", "image/png" },
            { ".gif", "image/gif" },
            { ".mp4", "video/mp4" },
            { ".avi", "video/x-msvideo" },
            { ".mov", "video/quicktime" }
        };

        public string[] GetMediaExtensions()
        {
            return extensions;
        }

        public string? TypeFromMediaExtension(string extension)
        {
            if (!types.TryGetValue(extension, out string? value)) 
                return null;
            
            return value;
        }

        public bool IsMediaFile(IFormFile file)
        {
            string fileExtension = Path.GetExtension(file.FileName).ToLower();
            return GetMediaExtensions().Contains(fileExtension);
        }
    }
}
