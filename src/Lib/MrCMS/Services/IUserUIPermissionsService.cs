using System.Threading.Tasks;
using MrCMS.Entities.Documents.Web;

namespace MrCMS.Services
{
    public interface IUserUIPermissionsService
    {
        Task<PageAccessPermission> IsCurrentUserAllowed(Webpage webpage);
    }
}