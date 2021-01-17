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
        /// <param name="setupOptions"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IModuleHostBuilder AddVditorModule(this IModuleHostBuilder modules, Action<ModuleOption>? setupOptions = null, Action<ModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.TryAddModule<VditorModule, ModuleOption>(setupOptions, configureOptions);
            return modules;
        }
    }

    /// <summary>
    /// Provide Vditor razor components.
    /// </summary>
    [Module(Description = "Vditor Razor components.", Url = "https://b3log.org/vditor/", Author = "B3log")]
    public class VditorModule : RazorComponentClientModule<ModuleService, ModuleOption, ModuleUI>
    {
        /// <summary>
        /// Create the instance.
        /// </summary>
        public VditorModule() : base()
        {
        }
    }

    /// <summary>
    /// UI for <see cref="VditorModule"/>.
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
                new UIResource(UIResourceType.StyleSheet,"https://cdn.jsdelivr.net/npm/vditor@3.7.5/dist/index.css"),
                new UIResource(UIResourceType.Script,"https://cdn.jsdelivr.net/npm/vditor@3.7.5/dist/index.min.js"),
                new UIResource(UIResourceType.Script, "_content/Vditor/vditor-blazor.js"),
            };
        }

        /// <inheritdoc/>
        public override RenderFragment Icon => Components.Fragments.Icon;
    }

    /// <summary>
    /// Options for <see cref="VditorModule"/>.
    /// </summary>
    public class ModuleOption
    {

    }

    /// <summary>
    /// Services for <see cref="VditorModule"/>.
    /// </summary>
    public class ModuleService : IModuleService
    {

    }
}
