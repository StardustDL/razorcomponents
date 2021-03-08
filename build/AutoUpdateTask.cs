using Cake.Common.Tools.DotNetCore;
using Cake.Core.IO;
using Cake.Core.IO.Arguments;
using Cake.Frosting;
using Cake.Common;
using Cake.Npm;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Cake.Core.Diagnostics;
using Cake.Common.Tools.DotNetCore.MSBuild;
using Cake.Common.Tools.DotNetCore.NuGet.Push;
using System;
using Cake.Gulp;

namespace Build
{
    [TaskName("AutoUpdate")]
    [IsDependentOn(typeof(PackTask))]
    public sealed class AutoUpdateTask : FrostingTask<BuildContext>
    {
        BuildContext Context { get; set; }

        public override void Run(BuildContext context)
        {
            Context = context;

            AntDesigns();
            Vditors();
            MaterialDesignIcons();
        }

        void Vditors()
        {
            var (v1, v2) = Context.UpdateNpmPackage(Paths.ProjectFile(nameof(Vditors)), "vditor");

            if (v1 != v2)
            {
                Context.Vditors(true);
            }
            else
            {
                Context.Log.Information("No update needs");
            }
        }

        void MaterialDesignIcons()
        {
            var (v1, v2) = Context.UpdateNpmPackage(Paths.ProjectFile(nameof(MaterialDesignIcons)), "@mdi/font");

            Context.UpdateNpmPackage(Paths.ProjectFile(nameof(MaterialDesignIcons)), "@mdi/svg");

            if (v1 != v2)
            {
                Context.MaterialDesignIcons(true);
            }
            else
            {
                Context.Log.Information("No update needs");
            }
        }

        void AntDesigns()
        {
            var (v1, v2) = Context.UpdateNugetPackage(Paths.ProjectFile(nameof(AntDesigns)), "AntDesign");

            if (v1 != v2)
            {
                Context.AntDesigns(true);
            }
            else
            {
                Context.Log.Information("No update needs");
            }
        }
    }
}