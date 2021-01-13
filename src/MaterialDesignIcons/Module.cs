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
    public static class ModuleExtensions
    {
        public static IModuleHostBuilder AddMaterialDesignIconModule(this IModuleHostBuilder modules, Action<ModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.TryAddModule<Module, ModuleOption>(configureOptions);
            return modules;
        }
    }

    public class Module : RazorComponentClientModule<ModuleService, ModuleOption, ModuleUI>
    {
        public Module() : base()
        {
            Manifest = Manifest with
            {
                Name = "MaterialDesignIcons",
                DisplayName = "Material Design Icons",
                Description = "Material Design Icon Razor components.",
                Url = "https://materialdesignicons.com/",
                Author = "Austin Andrews",
            };
        }
    }

    public class ModuleUI : Modulight.Modules.Client.RazorComponents.UI.ModuleUI
    {
        public ModuleUI(IJSRuntime jsRuntime, ILogger<Modulight.Modules.Client.RazorComponents.UI.ModuleUI> logger) : base(jsRuntime, logger)
        {
            Resources = new UIResource[]
            {
                new UIResource(UIResourceType.StyleSheet,"_content/StardustDL.RazorComponents.MaterialDesignIcons/mdi/css/materialdesignicons.min.css"),
            };
        }

        public override RenderFragment Icon => Components.Fragments.Icon;
    }

    public class ModuleOption
    {

    }

    public class ModuleService : IModuleService
    {

    }
}
