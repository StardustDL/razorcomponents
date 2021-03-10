using System;
using Modulight.Modules;
using Modulight.Modules.Client.RazorComponents;
using Microsoft.JSInterop;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components;
using Modulight.Modules.Client.RazorComponents.UI;
using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules.Hosting;

namespace StardustDL.RazorComponents.JQuerys
{
    /// <summary>
    /// Extensions for JQuery module.
    /// </summary>
    public static class ModuleExtensions
    {
        /// <summary>
        /// Add <see cref="JQueryModule"/>.
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        public static IModuleHostBuilder AddJQueryModule(this IModuleHostBuilder modules) => modules.AddModule<JQueryModule>();
    }

    /// <summary>
    /// Provide JQuery.
    /// </summary>
    [Module(Description = "JQuery.", Url = "https://jquery.com/", Author = "jquery")]
    [ModuleUIResource(UIResourceType.Script, "_content/StardustDL.RazorComponents.JQuerys/jquery/jquery.min.js")]
    public class JQueryModule : RazorComponentClientModule
    {
        /// <inheritdoc/>
        public JQueryModule(IModuleHost host) : base(host)
        {
        }

        /// <inheritdoc/>
        public override RenderFragment Icon => Components.Fragments.Icon;
    }
}
