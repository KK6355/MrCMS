﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using MrCMS.Entities.Documents.Layout;
using MrCMS.Entities.Documents.Media;
using MrCMS.Entities.Documents.Web;
using MrCMS.Entities.Multisite;
using MrCMS.Entities.People;
using MrCMS.Entities.Widget;
using MrCMS.Helpers;
using MrCMS.Indexing.Management;
using MrCMS.Installation;
using MrCMS.Services;
using MrCMS.Settings;
using MrCMS.Website;
using MrCMS.Website.Controllers;
using System.Linq;
using NHibernate;
using Ninject;

namespace MrCMS.Apps
{
    public abstract class MrCMSApp
    {
        protected abstract void RegisterApp(MrCMSAppRegistrationContext context);

        /// <summary>
        /// Gets the name of the area to register.
        /// </summary>
        /// 
        /// <returns>
        /// The name of the area to register.
        /// </returns>
        public abstract string AppName { get; }

        public static readonly Dictionary<Type, string> AppWebpages = new Dictionary<Type, string>();
        public static readonly Dictionary<Type, string> AppWidgets = new Dictionary<Type, string>();
        public virtual IEnumerable<Type> BaseTypes { get { yield break; } }

        internal void CreateContextAndRegister(RouteCollection routes, object state)
        {
            var context = new MrCMSAppRegistrationContext(this.AppName, routes, state);
            string @namespace = this.GetType().Namespace;
            if (@namespace != null)
                context.Namespaces.Add(@namespace + ".*");
            this.RegisterApp(context);
            var webpageTypes = TypeHelper.GetAllConcreteTypesAssignableFrom<Webpage>().FindAll(type => type.Namespace.StartsWith(this.GetType().Namespace));
            webpageTypes.ForEach(type => AppWebpages[type] = AppName);
            var widgetTypes = TypeHelper.GetAllConcreteTypesAssignableFrom<Widget>().FindAll(type => type.Namespace.StartsWith(this.GetType().Namespace));
            widgetTypes.ForEach(type => AppWidgets[type] = AppName);
        }

        /// <summary>
        /// Registers all areas in an ASP.NET MVC application by using the specified user-defined information.
        /// </summary>
        /// <param name="state">An object that contains user-defined information to pass to the area.</param>
        public static void RegisterAllApps(object state = null)
        {
            RegisterAllApps(RouteTable.Routes, state);
        }

        private static void RegisterAllApps(RouteCollection routes, object state)
        {
            foreach (Type type in TypeHelper.GetAllConcreteTypesAssignableFrom<MrCMSApp>())
                ((MrCMSApp)Activator.CreateInstance(type)).CreateContextAndRegister(routes, state);
        }

        public static void RegisterAllServices(IKernel kernel)
        {
            foreach (Type type in TypeHelper.GetAllConcreteTypesAssignableFrom<MrCMSApp>())
                ((MrCMSApp)Activator.CreateInstance(type)).RegisterServices(kernel);
        }

        public static void InstallApps(ISession session, InstallModel model, Site site)
        {
            foreach (
                var app in
                    TypeHelper.GetAllConcreteTypesAssignableFrom<MrCMSApp>()
                              .Select(type => ((MrCMSApp)Activator.CreateInstance(type)))
                              .OrderBy(app => app.InstallOrder))
                app.OnInstallation(session, model, site);
        }

        protected virtual int InstallOrder { get { return 10; } }

        protected abstract void RegisterServices(IKernel kernel);

        protected abstract void OnInstallation(ISession session, InstallModel model, Site site);
    }
}