using System.Threading.Tasks;
using MrCMS.Entities.People;

namespace MrCMS.Services
{
    public interface IGetCurrentUser
    {
        Task<User> Get();
        Task<User> GetLoggedInUser();
        Task<User> GetImpersonatedUser();
    }
}