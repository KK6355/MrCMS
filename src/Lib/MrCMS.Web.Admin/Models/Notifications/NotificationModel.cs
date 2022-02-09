﻿using System;

namespace MrCMS.Web.Admin.Models.Notifications
{
    public class NotificationModel
    {
        public string Message { get; set; }
        public string Date { get { return DateValue.ToString(); } }

        public DateTime DateValue { get; set; }
    }
}