﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MrCMS.Web.Admin.Infrastructure.ModelBinding;
using MrCMS.Web.Apps.Articles.Widgets;

namespace MrCMS.Web.Apps.Articles.Areas.Admin.Models
{
    public class UpdateArticleCategoriesModel : IAddPropertiesViewModel<ArticleCategories>, IUpdatePropertiesViewModel<ArticleCategories>
    {
        [Required, DisplayName("Article List Page")]
        public int ArticleListId { get; set; }

        [DisplayName("Show Name As Title")]
        public bool ShowNameAsTitle { get; set; }
    }
}