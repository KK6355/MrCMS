﻿using MrCMS.Web.Admin.Infrastructure.Breadcrumbs;

namespace MrCMS.Web.Admin.Breadcrumbs.System
{
    public class PageDefaultsBreadcrumb : Breadcrumb<SystemBreadcrumb>
    {
        public override decimal Order => 5;
        public override string Controller => "PageDefaults";
        public override string Action => "Index";
        public override bool IsNav => true;
    }
}