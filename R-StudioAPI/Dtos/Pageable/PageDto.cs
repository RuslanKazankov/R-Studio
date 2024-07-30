using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace R_StudioAPI.Dtos.Pageable
{
    public class PageDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page must be no less than 1")]
        [DefaultValue(1)]
        public int Page { get; set; } = 1;

        [Range(1, 50, ErrorMessage = "PageSize must be no less than 1 and no more than 100")]
        [DefaultValue(50)]
        public int PageSize { get; set; } = 50;
    }
}
