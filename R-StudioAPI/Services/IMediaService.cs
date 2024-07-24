namespace R_StudioAPI.Services
{
    public interface IMediaService
    {
        string[] GetMediaExtensions();
        string? TypeFromMediaExtension(string extension);
    }
}
