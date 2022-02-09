﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using MrCMS.Entities.Notifications;
using MrCMS.Services.Notifications;
using MrCMS.Web.Admin.Hubs;
using MrCMS.Web.Admin.Models.Notifications;
using MrCMS.Website;

namespace MrCMS.Web.Admin.Services.Events
{
    public class SendTransientNotificationToHub : IOnTransientNotificationPublished
    {
        private readonly IHubContext<NotificationHub> _notificationHubContext;
        private readonly IGetDateTimeNow _getDateTimeNow;

        public SendTransientNotificationToHub(IHubContext<NotificationHub> notificationHubContext,
            IGetDateTimeNow getDateTimeNow)
        {
            _notificationHubContext = notificationHubContext;
            _getDateTimeNow = getDateTimeNow;
        }

        public async Task Execute(OnTransientNotificationPublishedEventArgs args)
        {
            var notification = args.Notification;
            var model = new NotificationModel {Message = notification.Message, DateValue = _getDateTimeNow.LocalNow};
            switch (notification.NotificationType)
            {
                case NotificationType.AdminOnly:
                    await _notificationHubContext.Clients.Group(NotificationHub.AdminGroup)
                        .SendCoreAsync("sendTransientNotification", new object[] {model});
                    break;
                case NotificationType.UserOnly:
                    await _notificationHubContext.Clients.Group(NotificationHub.UsersGroup)
                        .SendCoreAsync("sendTransientNotification", new object[] {model});
                    break;
                case NotificationType.All:
                    await _notificationHubContext.Clients.All
                        .SendCoreAsync("sendTransientNotification", new object[] {model});
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}