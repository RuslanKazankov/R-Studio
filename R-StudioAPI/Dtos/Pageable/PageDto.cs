using System.ComponentModel.DataAnnotations;

namespace R_StudioAPI.Dtos.Pageable
{
    public class PageDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page must be no less than 1")]
        public int Page { get; set; } = 1;

        [Range(1, 30, ErrorMessage = "PageSize must be no less than 1 and no more than 100")]
        public int PageSize { get; set; } = 30;
    }
}
