﻿using MrCMS.Web.Admin.Infrastructure.Breadcrumbs;

namespace MrCMS.Web.Admin.Breadcrumbs.System
{
    public class AboutBreadcrumb : Breadcrumb<SystemBreadcrumb>
    {
        public override decimal Order => 15;
        public override string Controller => "About";
        public override string Action => "Index";
        public override bool IsNav => true;
    }
}