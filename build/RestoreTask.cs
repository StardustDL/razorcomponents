using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Tool;
using Cake.Core.IO.Arguments;
using Cake.Frosting;
using Cake.Common.Tools.Chocolatey;
using System;
using Cake.Core.Diagnostics;
using Cake.Common.IO;
using System.IO;
using Cake.Core.IO;
using Cake.Common;
using Cake.Npm;
using Cake.Npm.Ci;
using Cake.Npm.Install;
using Cake.Gulp;

namespace Build
{
    [TaskName("Restore")]
    public class RestoreTask : FrostingTask<BuildContext>
    {
        public const string CustomSourceName = "ownpkgs";

        public override void Run(BuildContext context)
        {
            try
            {
                context.DotNetCoreNuGetAddSource(CustomSourceName, new Cake.Common.Tools.DotNetCore.NuGet.Source.DotNetCoreNuGetSourceSettings
                {
                    Source = "https://sparkshine.pkgs.visualstudio.com/StardustDL/_packaging/feed/nuget/v3/index.json",
                });
            }
            catch (Exception ex)
            {
                context.Log.Error(ex.Message);
            }

            context.DotNetCoreTool("tool", new DotNetCoreToolSettings
            {
                ArgumentCustomization = builder =>
                {
                    builder.Append(new TextArgument("restore"));
                    return builder;
                }
            });

            foreach (var solution in context.SolutionFiles)
            {
                context.DotNetCoreRestore(solution.FullPath);
            }

            foreach (var item in context.GetSubDirectories(Paths.Source))
            {
                if (File.Exists(item.CombineWithFilePath("package.json").FullPath))
                {
                    context.NpmInstall(new NpmInstallSettings
                    {
                        WorkingDirectory = item
                    });
                    context.Gulp().Local.Execute(s =>
                    {
                        s.WorkingDirectory = item;
                    });
                }
            }
        }
    }
}