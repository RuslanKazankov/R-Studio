using R_StudioAPI.Repository.Interfaces;

namespace R_StudioAPI.DAL.Interfaces
{
    public interface ICommentaryUoW : IBaseUoW
    {
        IPostRepository PostRepository { get; }
        ICommentaryRepository CommentaryRepository { get; }
        IVideoRepository VideoRepository { get; }
    }
}
