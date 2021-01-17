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

namespace StardustDL.RazorComponents.AntDesigns
{
    /// <summary>
    /// Extensions for Ant Design module.
    /// </summary>
    public static class ModuleExtensions
    {
        /// <summary>
        /// Add <see cref="AntDesignModule"/>.
        /// </summary>
        /// <param name="modules"></param>
        /// <param name="setupOptions"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IModuleHostBuilder AddAntDesignModule(this IModuleHostBuilder modules, Action<ModuleOption>? setupOptions = null, Action<ModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.TryAddModule<AntDesignModule, ModuleOption>(setupOptions, configureOptions);
            return modules;
        }
    }

    /// <summary>
    /// Provide Ant Design razor components.
    /// </summary>
    [Module(Description = "AntDesign Razor components.", Url = "https://github.com/ant-design-blazor/ant-design-blazor", Author = "ant-design-blazor")]
    public class AntDesignModule : RazorComponentClientModule<ModuleService, ModuleOption, ModuleUI>
    {
        /// <summary>
        /// Create the instance.
        /// </summary>
        public AntDesignModule() : base()
        {
        }

        /// <inheritdoc/>
        public override void RegisterUI(IServiceCollection services)
        {
            base.RegisterUI(services);
            services.AddAntDesign();
        }
    }

    /// <summary>
    /// UI for <see cref="AntDesignModule"/>.
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
                new UIResource(UIResourceType.StyleSheet,"_content/AntDesign/css/ant-design-blazor.css"),
                new UIResource(UIResourceType.Script,"_content/AntDesign/js/ant-design-blazor.js"),
            };
        }

        /// <inheritdoc/>
        public override RenderFragment Icon => Components.Fragments.Icon;
    }

    /// <summary>
    /// Options for <see cref="AntDesignModule"/>.
    /// </summary>
    public class ModuleOption
    {

    }

    /// <summary>
    /// Services for <see cref="AntDesignModule"/>.
    /// </summary>
    public class ModuleService : IModuleService
    {

    }
}
