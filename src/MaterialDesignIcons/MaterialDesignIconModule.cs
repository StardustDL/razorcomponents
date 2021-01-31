using System;
using Modulight.Modules;
using Modulight.Modules.Client.RazorComponents;
using Microsoft.JSInterop;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components;
using Modulight.Modules.Client.RazorComponents.UI;
using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules.Hosting;

namespace StardustDL.RazorComponents.MaterialDesignIcons
{
    /// <summary>
    /// Extensions for Material Design Icon module.
    /// </summary>
    public static class ModuleExtensions
    {
        /// <summary>
        /// Add 
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        public static IModuleHostBuilder AddMaterialDesignIconModule(this IModuleHostBuilder modules) => modules.AddModule<MaterialDesignIconModule>();
    }

    /// <summary>
    /// Provide Material Design Icon razor components.
    /// </summary>
    [Module(Description = "Material Design Icon Razor components.", Url = "https://materialdesignicons.com/", Author = "Austin Andrews")]
    [ModuleUI(typeof(ModuleUI))]
    public class MaterialDesignIconModule : RazorComponentClientModule<MaterialDesignIconModule>
    {
        public MaterialDesignIconModule(IModuleHost host) : base(host)
        {
        }
    }

    [ModuleUIResource(UIResourceType.StyleSheet, "_content/StardustDL.RazorComponents.MaterialDesignIcons/mdi/css/materialdesignicons.min.css")]
    class ModuleUI : Modulight.Modules.Client.RazorComponents.UI.ModuleUI
    {
        public ModuleUI(IJSRuntime jsRuntime, ILogger<ModuleUI> logger) : base(jsRuntime, logger)
        {
        }

        public override RenderFragment Icon => Components.Fragments.Icon;
    }
}
