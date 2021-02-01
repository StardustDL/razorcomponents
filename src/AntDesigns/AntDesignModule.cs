using System;
using Modulight.Modules;
using Modulight.Modules.Client.RazorComponents;
using Microsoft.JSInterop;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components;
using Modulight.Modules.Client.RazorComponents.UI;
using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules.Hosting;

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
        /// <returns></returns>
        public static IModuleHostBuilder AddAntDesignModule(this IModuleHostBuilder modules) => modules.AddModule<AntDesignModule>();
    }

    /// <summary>
    /// Provide Ant Design razor components.
    /// </summary>
    [Module(Description = "AntDesign Razor components.", Url = "https://github.com/ant-design-blazor/ant-design-blazor", Author = "ant-design-blazor", Version = "0.5.3")]
    [ModuleStartup(typeof(Startup))]
    [ModuleUIResource(UIResourceType.Script, "_content/AntDesign/js/ant-design-blazor.js")]
    [ModuleUIResource(UIResourceType.StyleSheet, "_content/AntDesign/css/ant-design-blazor.css")]
    public class AntDesignModule : RazorComponentClientModule<AntDesignModule>
    {
        /// <inheritdoc/>
        public AntDesignModule(IModuleHost host) : base(host)
        {
        }

        /// <inheritdoc/>
        public override RenderFragment Icon => Components.Fragments.Icon;
    }

    class Startup : ModuleStartup
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAntDesign();
            base.ConfigureServices(services);
        }
    }
}
