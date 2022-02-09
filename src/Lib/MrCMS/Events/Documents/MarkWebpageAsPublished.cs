using System.Threading.Tasks;
using MrCMS.Entities.Documents.Web;
using MrCMS.Website;

namespace MrCMS.Events.Documents
{
    public class MarkWebpageAsPublished : IOnUpdating<Webpage>, IOnAdding<Webpage>
    {
        private readonly IGetDateTimeNow _getDateTimeNow;

        public MarkWebpageAsPublished(IGetDateTimeNow getDateTimeNow)
        {
            _getDateTimeNow = getDateTimeNow;
        }

        public Task Execute(OnUpdatingArgs<Webpage> args)
        {
            var now = _getDateTimeNow.UtcNow;
            var webpage = args.Item;
            if (webpage.PublishOn.HasValue && webpage.PublishOn.Value.ToUniversalTime() <= now && webpage.Published == false)
            {
                webpage.Published = true;
            }
            return Task.CompletedTask;
        }

        public Task Execute(OnAddingArgs<Webpage> args)
        {
            var now = _getDateTimeNow.LocalNow;
            var webpage = args.Item;
            if (webpage.PublishOn.HasValue && webpage.PublishOn <= now && webpage.Published == false)
            {
                webpage.Published = true;
            }
            return Task.CompletedTask;
        }
    }
}