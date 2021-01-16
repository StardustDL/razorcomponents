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
    public static class ModuleExtensions
    {
        public static IModuleHostBuilder AddVditorModule(this IModuleHostBuilder modules, Action<ModuleOption>? setupOptions = null, Action<ModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.TryAddModule<Module, ModuleOption>(setupOptions, configureOptions);
            return modules;
        }
    }

    public class Module : RazorComponentClientModule<ModuleService, ModuleOption, ModuleUI>
    {
        public Module() : base()
        {
            Manifest = Manifest with
            {
                Name = "Vditor",
                DisplayName = "Vditor",
                Description = "Vditor Razor components.",
                Url = "https://b3log.org/vditor/",
                Author = "B3log",
            };
        }
    }

    public class ModuleUI : Modulight.Modules.Client.RazorComponents.UI.ModuleUI
    {
        public ModuleUI(IJSRuntime jsRuntime, ILogger<Modulight.Modules.Client.RazorComponents.UI.ModuleUI> logger) : base(jsRuntime, logger)
        {
            Resources = new UIResource[]
            {
                new UIResource(UIResourceType.StyleSheet,"https://cdn.jsdelivr.net/npm/vditor@3.4.7/dist/index.css"),
                new UIResource(UIResourceType.Script,"https://cdn.jsdelivr.net/npm/vditor@3.4.7/dist/index.min.js"),
                new UIResource(UIResourceType.Script, "_content/Vditor/vditor-blazor.js"),
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
