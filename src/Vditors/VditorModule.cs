﻿using System;
using Modulight.Modules;
using Modulight.Modules.Client.RazorComponents;
using Microsoft.JSInterop;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components;
using Modulight.Modules.Client.RazorComponents.UI;
using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules.Hosting;

namespace StardustDL.RazorComponents.Vditors
{
    /// <summary>
    /// Extensions for Vditor module.
    /// </summary>
    public static class ModuleExtensions
    {
        /// <summary>
        /// Add <see cref="VditorModule"/>.
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        public static IModuleHostBuilder AddVditorModule(this IModuleHostBuilder modules) => modules.AddModule<VditorModule>();
    }

    /// <summary>
    /// Provide Vditor razor components.
    /// </summary>
    [Module(Description = "Vditor Razor components.", Url = "https://b3log.org/vditor/", Author = "B3log", Version = "3.8.0")]
    [ModuleUIResource(UIResourceType.StyleSheet, "https://cdn.jsdelivr.net/npm/vditor@3.8.0/dist/index.css")]
    [ModuleUIResource(UIResourceType.Script, "https://cdn.jsdelivr.net/npm/vditor@3.8.0/dist/index.min.js")]
    [ModuleUIResource(UIResourceType.Script, "_content/Vditor/vditor-blazor.js")]
    public class VditorModule : RazorComponentClientModule<VditorModule>
    {
        /// <inheritdoc/>
        public VditorModule(IModuleHost host) : base(host)
        {
        }

        /// <inheritdoc/>
        public override RenderFragment Icon => Components.Fragments.Icon;
    }
}
