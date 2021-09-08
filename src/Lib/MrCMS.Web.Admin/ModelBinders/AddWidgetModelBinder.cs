﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using MrCMS.Entities.Widget;
using MrCMS.Helpers;

namespace MrCMS.Web.Admin.ModelBinders
{
    public class AddWidgetModelBinder : IModelBinder
    {
        private static Type GetTypeByName(ModelBindingContext bindingContext)
        {
            return WidgetHelper.GetTypeByName(bindingContext.ValueProvider.GetValue("WidgetType").FirstValue);
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task BindModelAsync(ModelBindingContext bindingContext)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var type = GetTypeByName(bindingContext);
            var serviceProvider = bindingContext.HttpContext.RequestServices;
            var metadataProvider = serviceProvider.GetRequiredService<IModelMetadataProvider>();
            bindingContext.ModelMetadata = metadataProvider.GetMetadataForType(type);
            //var instance = SystemEntityBinderProvider.GetModelBinder(bindingContext.ModelMetadata, serviceProvider);

            //await instance.BindModelAsync(bindingContext);

            if (bindingContext.Result.Model is Widget widget)
            {
                widget.LayoutArea?.AddWidget(widget);
            }
        }
    }
}