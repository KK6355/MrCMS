using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MrCMS.Entities.Documents.Web;

namespace MrCMS.Web.Admin.Services
{
    public abstract class BaseAssignWebpageAdminViewData
    {
        public abstract Task AssignViewDataBase(Webpage webpage, ViewDataDictionary viewData);
    }

    public abstract class BaseAssignWebpageAdminViewData<T> : BaseAssignWebpageAdminViewData where T : Webpage
    {
        public abstract Task AssignViewData(T webpage, ViewDataDictionary viewData);

        public sealed override async Task AssignViewDataBase(Webpage webpage, ViewDataDictionary viewData)
        {
            await AssignViewData(webpage as T, viewData);
        }
    }
}