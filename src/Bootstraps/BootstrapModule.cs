using System;
using Modulight.Modules;
using Modulight.Modules.Client.RazorComponents;
using Microsoft.JSInterop;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components;
using Modulight.Modules.Client.RazorComponents.UI;
using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules.Hosting;

namespace StardustDL.RazorComponents.Bootstraps
{
    /// <summary>
    /// Extensions for Vditor module.
    /// </summary>
    public static class ModuleExtensions
    {
        /// <summary>
        /// Add <see cref="BootstrapModule"/>.
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        public static IModuleHostBuilder AddBootstrapModule(this IModuleHostBuilder modules) => modules.AddModule<BootstrapModule>();
    }

    /// <summary>
    /// Provide Vditor razor components.
    /// </summary>
    [Module(Description = "Bootstrap Razor components.", Url = "https://getbootstrap.com/", Author = "Twitter")]
    [ModuleUIResource(UIResourceType.StyleSheet, "_content/StardustDL.RazorComponents.Bootstraps/bootstrap/css/bootstrap.min.css")]
    [ModuleUIResource(UIResourceType.Script, "_content/StardustDL.RazorComponents.Bootstraps/bootstrap/js/bootstrap.bundle.min.js")]
    public class BootstrapModule : RazorComponentClientModule
    {
        /// <inheritdoc/>
        public BootstrapModule(IModuleHost host) : base(host)
        {
        }

        /// <inheritdoc/>
        public override RenderFragment Icon => Components.Fragments.Icon;
    }
}
