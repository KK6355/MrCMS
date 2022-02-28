﻿using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using MrCMS.Entities.Documents.Web;
using MrCMS.Web.Admin.Infrastructure.Models.Tabs;

namespace MrCMS.Web.Admin.Models.WebpageEdit
{
    public class ContentVersionsTab : AdminTab<Webpage>
    {
        public override int Order => 50;
        public override Type ParentType => null;
        public override Type ModelType => null;
        public override Task<string> Name(IServiceProvider serviceProvider, Webpage entity)
        {
            return Task.FromResult("Content Versions");
        }

        public override Task<bool> ShouldShow(IServiceProvider serviceProvider, Webpage entity)
        {
            return Task.FromResult(true);
        }

        public override string TabHtmlId => "content-versions";
        public override Task RenderTabPane(IHtmlHelper helper, IMapper mapper, Webpage entity)
        {
            return helper.RenderPartialAsync("ContentVersions", entity);
        }
    }
}