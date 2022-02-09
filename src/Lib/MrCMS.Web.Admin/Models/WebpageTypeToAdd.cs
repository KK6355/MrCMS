using MrCMS.Entities.Documents;

namespace MrCMS.Web.Admin.Models
{
    public class WebpageTypeToAdd
    {
        public WebpageMetadata Type { get; set; }
        public int? TemplateId { get; set; }
        public string DisplayName { get; set; }
        public string WebpageType => Type.Type.FullName;

        //public string DisplayId
        //{
        //    get { return TemplateId.HasValue ? Type.Type.FullName + "-" + TemplateId : Type.Type.FullName; }
        //}
    }
}