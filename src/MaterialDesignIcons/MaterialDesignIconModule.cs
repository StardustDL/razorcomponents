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
        /// Add <see cref=" MaterialDesignIconModule"/>.
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        public static IModuleHostBuilder AddMaterialDesignIconModule(this IModuleHostBuilder modules) => modules.AddModule<MaterialDesignIconModule>();
    }

    /// <summary>
    /// Provide Material Design Icon razor components.
    /// </summary>
    [Module(Description = "Material Design Icon Razor components.", Url = "https://materialdesignicons.com/", Author = "Austin Andrews", Version = "5.9.55")]
    [ModuleUIResource(UIResourceType.StyleSheet, "_content/StardustDL.RazorComponents.MaterialDesignIcons/mdi/css/materialdesignicons.min.css")]
    public class MaterialDesignIconModule : RazorComponentClientModule
    {
        /// <inheritdoc/>
        public MaterialDesignIconModule(IModuleHost host) : base(host)
        {
        }

        /// <inheritdoc/>
        public override RenderFragment Icon => Components.Fragments.Icon;
    }
}
