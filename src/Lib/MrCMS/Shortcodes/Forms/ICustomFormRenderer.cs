﻿using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using MrCMS.Entities.Documents.Web;

namespace MrCMS.Shortcodes.Forms
{
    public interface ICustomFormRenderer
    {
        IHtmlContent GetForm(IHtmlHelper helper, Form form, FormSubmittedStatus submittedStatus);
    }
}