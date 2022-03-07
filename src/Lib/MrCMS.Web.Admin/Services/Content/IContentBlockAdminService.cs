using MrCMS.Entities.Documents.Web;
using MrCMS.Web.Admin.Models.Content;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MrCMS.Web.Admin.Services.Content;

public interface IContentBlockAdminService
{
    Task<IReadOnlyList<ContentBlockOption>> GetContentRowOptions(int id);
    Task<AddContentBlockModel> GetAddModel(int id);
    Task<ContentBlock> AddBlock(AddContentBlockModel model);
    Task<IContentBlock> GetBlock(int id);
    Task UpdateBlock(int id, object model);
    Task RemoveBlock(int id);
    Task AddChild(int id);
    Task<object> GetUpdateModel(int id);
    Task SetBlockOrders(List<ContentBlockSortModel> contentBlockSortModel);
    Task ToggleBlockHidden(int id);
}