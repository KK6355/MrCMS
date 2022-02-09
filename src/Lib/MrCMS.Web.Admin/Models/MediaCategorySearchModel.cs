using System.ComponentModel;
using MrCMS.Web.Admin.Infrastructure.Models;

namespace MrCMS.Web.Admin.Models
{
    public class MediaCategorySearchModel:IHaveId
    {
        public MediaCategorySearchModel()
        {
            Page = 1;
        }

        public int? Id { get; set; }
        public int Page { get; set; }

        [DisplayName("Search")]
        public string SearchText { get; set; }

        public MediaCategorySortMethod SortBy { get; set; }
    }
}