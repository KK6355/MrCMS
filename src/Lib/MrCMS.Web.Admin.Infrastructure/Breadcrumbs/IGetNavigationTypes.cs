﻿using System;
using System.Collections.Generic;

namespace MrCMS.Web.Admin.Infrastructure.Breadcrumbs
{
    public interface IGetNavigationTypes
    {
        IEnumerable<Type> GetChildren(Type type);
        IEnumerable<Type> GetRootNavTypes();
    }
}