using System.Threading.Tasks;
using MrCMS.Entities.Documents.Media;
using MrCMS.Entities.Notifications;
using MrCMS.Services.Notifications;

namespace MrCMS.Events.Documents
{
    public class MediaCategoryUpdatedNotification : IOnUpdated<MediaCategory>
    {
        private readonly IDocumentModifiedUser _documentModifiedUser;
        private readonly INotificationPublisher _notificationPublisher;

        public MediaCategoryUpdatedNotification(IDocumentModifiedUser documentModifiedUser, INotificationPublisher notificationPublisher)
        {
            _documentModifiedUser = documentModifiedUser;
            _notificationPublisher = notificationPublisher;
        }

        public async Task Execute(OnUpdatedArgs<MediaCategory> args)
        {
            var webpage = args.Item;
            string message = string.Format("<a href=\"/Admin/MediaCategory/Edit/{1}\">{0}</a> has been updated{2}.",
                webpage.Name,
                webpage.Id, await _documentModifiedUser.GetInfo());
            await _notificationPublisher.PublishNotification(message, PublishType.Both, NotificationType.AdminOnly);
        }
    }
}