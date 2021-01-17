using System;
using Modulight.Modules;
using Modulight.Modules.Services;
using Modulight.Modules.Options;
using Modulight.Modules.Client.RazorComponents;
using Microsoft.JSInterop;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components;
using Modulight.Modules.Client.RazorComponents.UI;
using Microsoft.Extensions.DependencyInjection;

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
        /// <param name="setupOptions"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IModuleHostBuilder AddMaterialDesignIconModule(this IModuleHostBuilder modules, Action<ModuleOption>? setupOptions = null, Action<ModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.TryAddModule<MaterialDesignIconModule, ModuleOption>(setupOptions, configureOptions);
            return modules;
        }
    }

    /// <summary>
    /// Provide Material Design Icon razor components.
    /// </summary>
    [Module(Description = "Material Design Icon Razor components.", Url = "https://materialdesignicons.com/", Author = "Austin Andrews")]
    public class MaterialDesignIconModule : RazorComponentClientModule<ModuleService, ModuleOption, ModuleUI>
    {
        /// <summary>
        /// Create the instance.
        /// </summary>
        public MaterialDesignIconModule() : base()
        {
        }
    }

    /// <summary>
    /// UI for <see cref="MaterialDesignIconModule"/>.
    /// </summary>
    public class ModuleUI : Modulight.Modules.Client.RazorComponents.UI.ModuleUI
    {
        /// <summary>
        /// Create the instance.
        /// </summary>
        /// <param name="jsRuntime"></param>
        /// <param name="logger"></param>
        public ModuleUI(IJSRuntime jsRuntime, ILogger<Modulight.Modules.Client.RazorComponents.UI.ModuleUI> logger) : base(jsRuntime, logger)
        {
            Resources = new UIResource[]
            {
                new UIResource(UIResourceType.StyleSheet,"_content/StardustDL.RazorComponents.MaterialDesignIcons/mdi/css/materialdesignicons.min.css"),
            };
        }

        /// <inheritdoc/>
        public override RenderFragment Icon => Components.Fragments.Icon;
    }

    /// <summary>
    /// Options for <see cref="MaterialDesignIconModule"/>.
    /// </summary>
    public class ModuleOption
    {

    }

    /// <summary>
    /// Services for <see cref="MaterialDesignIconModule"/>.
    /// </summary>
    public class ModuleService : IModuleService
    {

    }
}
