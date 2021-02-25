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
    [IsDependentOn(typeof(RestoreTask))]
    public sealed class AutoUpdateTask : FrostingTask<BuildContext>
    {
        const string NugetSource = "https://api.nuget.org/v3/index.json";

        BuildContext Context { get; set; }

        public override void Run(BuildContext context)
        {
            Context = context;

            MaterialDesignIcons();
            //AntDesigns();
        }

        string GetNugetPackageVersion(FilePath project, string packageName)
        {
            var line = File.ReadAllLines(project.FullPath).Where(x => x.Contains($"<PackageReference Include=\"{packageName}\"")).FirstOrDefault() ?? "";
            if (line is "")
                return "";
            var l2 = line.Split("Version=\"")[1];
            var idx = l2.IndexOf("\"");
            return l2.Substring(0, idx);
        }

        string GetNpmPackageVersion(FilePath project, string packageName)
        {
            var line = File.ReadAllLines(project.GetDirectory().CombineWithFilePath("package.json").FullPath).Where(x => x.Contains($"\"{packageName}\":")).FirstOrDefault() ?? "";
            if (line is "")
                return "";
            var l2 = line.Split(":")[1];
            return l2.Trim().Trim('\"', ',', '^');
        }

        (string, string) UpdateNugetPackage(FilePath project, string packageName, bool prerelease = false)
        {
            Context.Log.Information($"Updating {packageName} in {project}");

            var oldVersion = GetNugetPackageVersion(project, packageName);

            Context.Log.Information($"Old version: {oldVersion}");

            var arguments = new ProcessArgumentBuilder();
            arguments.Append(new TextArgument("package"));
            arguments.Append(new TextArgument("-s"));
            arguments.Append(new TextArgument(NugetSource));
            if (prerelease)
                arguments.Append(new TextArgument("--prerelease"));
            arguments.Append(new TextArgument(packageName));
            Context.DotNetCoreTool(project, "add", arguments);

            var newVersion = GetNugetPackageVersion(project, packageName);

            Context.Log.Information($"New version: {newVersion}");

            return (oldVersion, newVersion);
        }

        (string, string) UpdateNpmPackage(FilePath project, string packageName)
        {
            Context.Log.Information($"Updating {packageName} in {project}");

            var oldVersion = GetNpmPackageVersion(project, packageName);

            Context.Log.Information($"Old version: {oldVersion}");

            Context.NpmUpdate(s =>
            {
                s.WorkingDirectory = project.GetDirectory();
                s.ArgumentCustomization = builder =>
                {
                    builder.Append(new TextArgument(packageName));
                    return builder;
                };
            });

            Context.NpmInstall(s =>
            {
                s.WorkingDirectory = project.GetDirectory();
            });

            var newVersion = GetNpmPackageVersion(project, packageName);

            Context.Log.Information($"New version: {newVersion}");

            Context.Gulp().Local.Execute(s =>
            {
                s.WorkingDirectory = project.GetDirectory();
            });

            return (oldVersion, newVersion);
        }

        void BuildPackDeploy(FilePath project, string version, string packageName)
        {
            {
                var settings = Context.GetMSBuildSettings();
                settings.SetVersion(version);

                Context.DotNetCoreBuild(project.FullPath, new Cake.Common.Tools.DotNetCore.Build.DotNetCoreBuildSettings
                {
                    MSBuildSettings = settings
                });

                Context.DotNetCorePack(project.FullPath, new Cake.Common.Tools.DotNetCore.Pack.DotNetCorePackSettings
                {
                    OutputDirectory = Paths.Dist.Packages,
                    MSBuildSettings = settings,
                });
            }

            if (Context.AutoUpdate)
            {
                var settings = new DotNetCoreNuGetPushSettings
                {
                    SkipDuplicate = true,
                };

                Context.Log.Information("Publish to NuGet.");

                string nugetToken = Context.EnvironmentVariable("NUGET_AUTH_TOKEN", "");

                if (nugetToken is "")
                {
                    throw new Exception("No NUGET_AUTH_TOKEN environment variable setted.");
                }

                settings.ApiKey = nugetToken;
                settings.Source = "https://api.nuget.org/v3/index.json";

                Context.DotNetCoreNuGetPush(Paths.Dist.Packages.CombineWithFilePath($"StardustDL.RazorComponents.{packageName}.{version}.nupkg").FullPath, settings);
            }
        }

        void Vditors()
        {
            var rootPath = Paths.Source.Combine(nameof(Vditors)).CombineWithFilePath($"{nameof(Vditors)}.csproj");

            var (v1, v2) = UpdateNpmPackage(rootPath, "vditor");

            if (v1 != v2)
            {
                BuildPackDeploy(rootPath, v2, nameof(Vditors));
            }
        }

        void MaterialDesignIcons()
        {
            var rootPath = Paths.Source.Combine(nameof(MaterialDesignIcons)).CombineWithFilePath($"{nameof(MaterialDesignIcons)}.csproj");

            var (v1, v2) = UpdateNpmPackage(rootPath, "@mdi/font");

            if (v1 != v2)
            {
                BuildPackDeploy(rootPath, v2, nameof(MaterialDesignIcons));
            }
        }

        void AntDesigns()
        {
            var rootPath = Paths.Source.Combine(nameof(AntDesigns)).CombineWithFilePath($"{nameof(AntDesigns)}.csproj");

            var (v1, v2) = UpdateNugetPackage(rootPath, "AntDesign");

            if (v1 != v2)
            {
                BuildPackDeploy(rootPath, v2, nameof(AntDesigns));
            }
        }
    }
}