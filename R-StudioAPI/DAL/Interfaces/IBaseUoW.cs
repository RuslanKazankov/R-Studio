namespace R_StudioAPI.DAL.Interfaces
{
    public interface IBaseUoW : IDisposable
    {
        Task Save();
    }
}
