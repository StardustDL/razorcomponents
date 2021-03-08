using System;
using Modulight.Modules;
using Modulight.Modules.Client.RazorComponents;
using Microsoft.JSInterop;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components;
using Modulight.Modules.Client.RazorComponents.UI;
using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules.Hosting;
using Microsoft.Extensions.Options;

namespace StardustDL.RazorComponents.MaterialDesignIcons
{
    /// <summary>
    /// Provider for mdi-icon
    /// </summary>
    public enum MaterialDesignIconProvider
    {
        /// <summary>
        /// Web fonts
        /// </summary>
        Font,
        /// <summary>
        /// SVG
        /// </summary>
        Svg,
    }

    /// <summary>
    /// Options for <see cref="MaterialDesignIconModule"/>.
    /// </summary>
    public class MaterialDesignIconModuleOption
    {
        /// <summary>
        /// Provider
        /// </summary>
        public MaterialDesignIconProvider Provider { get; set; }
    }

    /// <summary>
    /// Extensions for Material Design Icon module.
    /// </summary>
    public static class ModuleExtensions
    {
        /// <summary>
        /// Add <see cref=" MaterialDesignIconModule"/>.
        /// </summary>
        /// <param name="modules"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IModuleHostBuilder AddMaterialDesignIconModule(this IModuleHostBuilder modules, Action<MaterialDesignIconModuleOption>? configureOptions = null)
        {
            if (configureOptions is not null)
            {
                modules.ConfigureBuilderOptions<MaterialDesignIconModuleOption>((o, sp) => configureOptions(o));
                modules.ConfigureOptions<MaterialDesignIconModuleOption>((o, sp) => configureOptions(o));
            }
            return modules.AddModule<MaterialDesignIconModule>();
        }
    }

    /// <summary>
    /// Provide Material Design Icon razor components.
    /// </summary>
    [Module(Description = "Material Design Icon Razor components.", Url = "https://materialdesignicons.com/", Author = "Austin Andrews")]
    [ModuleStartup(typeof(Startup))]
    public class MaterialDesignIconModule : RazorComponentClientModule
    {
        /// <inheritdoc/>
        public MaterialDesignIconModule(IOptions<MaterialDesignIconModuleOption> options, IModuleHost host) : base(host)
        {
            Options = options.Value;
        }

        /// <inheritdoc/>
        public override RenderFragment Icon => Components.Fragments.Icon;

        internal MaterialDesignIconModuleOption Options { get; }
    }

    class Startup : RazorComponentClientModuleStartup
    {
        public Startup(IOptions<MaterialDesignIconModuleOption> option) => Option = option.Value;

        MaterialDesignIconModuleOption Option { get; }

        public override void ConfigureRazorComponentClientModuleManifest(IRazorComponentClientModuleManifestBuilder builder)
        {
            switch (Option.Provider)
            {
                case MaterialDesignIconProvider.Font:
                    builder.WithResource(new UIResource(UIResourceType.StyleSheet, "_content/StardustDL.RazorComponents.MaterialDesignIcons/mdi/font/css/materialdesignicons.min.css"));
                    break;
            }
            base.ConfigureRazorComponentClientModuleManifest(builder);
        }
    }
}
