using R_StudioAPI.Repository.Interfaces;

namespace R_StudioAPI.DAL.Interfaces
{
    public interface IPostUoW : IBaseUoW
    {
        IPostRepository PostRepository { get; }
        IPostMediaRepository PostMediaRepository { get; }
    }
}
