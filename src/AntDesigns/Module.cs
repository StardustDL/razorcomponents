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
    public static class ModuleExtensions
    {
        public static IModuleHostBuilder AddAntDesignModule(this IModuleHostBuilder modules, Action<ModuleOption, IServiceProvider>? configureOptions = null)
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
                Name = "AntDesign",
                DisplayName = "AntDesign Razor Components",
                Description = "AntDesign Razor components.",
                Url = "https://github.com/ant-design-blazor/ant-design-blazor",
                Author = "ant-design-blazor",
            };
        }

        public override void RegisterUI(IServiceCollection services)
        {
            base.RegisterUI(services);
            services.AddAntDesign();
        }
    }

    public class ModuleUI : Modulight.Modules.Client.RazorComponents.UI.ModuleUI
    {
        public ModuleUI(IJSRuntime jsRuntime, ILogger<Modulight.Modules.Client.RazorComponents.UI.ModuleUI> logger) : base(jsRuntime, logger)
        {
            Resources = new UIResource[]
            {
                new UIResource(UIResourceType.StyleSheet,"_content/AntDesign/css/ant-design-blazor.css"),
                new UIResource(UIResourceType.Script,"_content/AntDesign/js/ant-design-blazor.js"),
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
