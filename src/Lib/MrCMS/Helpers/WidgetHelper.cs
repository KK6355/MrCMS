using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using MrCMS.Entities.Widget;

namespace MrCMS.Helpers
{
    public static class WidgetHelper
    {
        private static IEnumerable<Type> _widgetTypes;

        public static IEnumerable<Type> WidgetTypes =>
            _widgetTypes ?? (_widgetTypes = TypeHelper.GetAllConcreteMappedClassesAssignableFrom<Widget>());

        public static List<SelectListItem> WidgetTypeDropdownItems
        {
            get
            {
                return WidgetTypes.OrderBy(x => x.Name).BuildSelectItemList(type => type.Name.BreakUpString(),
                    type => type.FullName,
                    emptyItemText: null);
            }
        }

        public static Type GetTypeByName(string typeName)
        {
            return WidgetTypes.FirstOrDefault(x => x.FullName == typeName);
        }
    }
}