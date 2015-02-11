﻿using System.Collections.Generic;
using EPiServer.Framework.Web.Resources;
using EPiServer.Shell.Modules;

namespace EPiTube.FasetFilter.Widget.Edit
{
    public class EPiTubeFasetFilterModule : ShellModule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EPiTubeFasetFilterModule"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="routeBasePath">The route base path.</param>
        /// <param name="resourceBasePath">The resource base path.</param>
        public EPiTubeFasetFilterModule(string name, string routeBasePath, string resourceBasePath)
            : base(name, routeBasePath, resourceBasePath)
        {
        }

        /// <summary>
        /// Creates the view module to be rendered as setting of CommerceApplication (in client side).
        /// </summary>
        /// <param name="moduleTable">The module table.</param>
        /// <param name="clientResourceService">The client resource service.</param>
        /// <returns>An object that will be serialized and sent to the client when initializing a view.</returns>
        public override ModuleViewModel CreateViewModel(ModuleTable moduleTable, IClientResourceService clientResourceService)
        {
            var model = base.CreateViewModel(moduleTable, clientResourceService);
            AddUiRoute(model);
            return model;
        }

        private void AddUiRoute(ModuleViewModel viewModel)
        {
            viewModel.Routes.Add(new ModuleRoutePair(ResourceBasePath, new Dictionary<string, string> { { "moduleArea", "EPiTubeFasetFilter" } }));
        }
    }
}