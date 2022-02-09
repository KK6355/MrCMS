﻿using MrCMS.Web.Admin.Infrastructure.Breadcrumbs;

namespace MrCMS.Web.Admin.Breadcrumbs.Webpages
{
    public class MergeWebpageBreadcrumb : Breadcrumb<WebpageBreadcrumb>
    {
        public override string Controller => "MergeWebpage";
        public override string Action => "Index";
        public override string Name => "Merge";
        public override void Populate()
        {
            ParentActionArguments = CreateIdArguments(Id);
        }
    }
}