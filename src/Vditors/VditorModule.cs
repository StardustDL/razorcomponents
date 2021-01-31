using System;
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
    [Module(Description = "Vditor Razor components.", Url = "https://b3log.org/vditor/", Author = "B3log")]
    [ModuleUI(typeof(ModuleUI))]
    public class VditorModule : RazorComponentClientModule<VditorModule>
    {
        public VditorModule(IModuleHost host) : base(host)
        {
        }
    }

    [ModuleUIResource(UIResourceType.StyleSheet, "https://cdn.jsdelivr.net/npm/vditor@3.7.5/dist/index.css")]
    [ModuleUIResource(UIResourceType.Script, "https://cdn.jsdelivr.net/npm/vditor@3.7.5/dist/index.min.js")]
    [ModuleUIResource(UIResourceType.Script, "_content/Vditor/vditor-blazor.js")]
    class ModuleUI : Modulight.Modules.Client.RazorComponents.UI.ModuleUI
    {
        public ModuleUI(IJSRuntime jsRuntime, ILogger<ModuleUI> logger) : base(jsRuntime, logger)
        {
        }

        public override RenderFragment Icon => Components.Fragments.Icon;
    }
}
