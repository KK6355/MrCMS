﻿using System;
using System.Collections.Generic;
using MrCMS.Entities.People;
using MrCMS.Services;

namespace MrCMS.Entities.Messaging
{
    public class ResetPasswordMessageTemplate : MessageTemplate, IMessageTemplate<User>
    {
        public override MessageTemplate GetInitialTemplate()
        {
            throw new NotImplementedException();
        }

        public List<string> GetTokens()
        {
            return NotificationTemplateProcessor.GetTokens<User>();
        }
    }
}