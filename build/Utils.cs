using Cake.Common;
using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.MSBuild;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.IO.Arguments;
using Cake.Gulp;
using Cake.Npm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Build
{
    static class Utils
    {
        public static string GetNugetPackageVersion(this BuildContext context, FilePath project, string packageName)
        {
            project = project.MakeAbsolute(context.Environment);
            var line = File.ReadAllLines(project.FullPath).Where(x => x.Contains($"<PackageReference Include=\"{packageName}\"")).FirstOrDefault() ?? "";
            if (line is "")
                return "";
            var l2 = line.Split("Version=\"")[1];
            var idx = l2.IndexOf("\"");
            return l2.Substring(0, idx);
        }

        public static string GetNpmPackageVersion(this BuildContext context, FilePath project, string packageName)
        {
            project = project.MakeAbsolute(context.Environment);
            var line = File.ReadAllLines(project.GetDirectory().CombineWithFilePath("package.json").FullPath).Where(x => x.Contains($"\"{packageName}\":")).FirstOrDefault() ?? "";
            if (line is "")
                return "";
            var l2 = line.Split(":")[1];
            return l2.Trim().Trim('\"', ',', '^');
        }

        public static (string, string) UpdateNugetPackage(this BuildContext context, FilePath project, string packageName, bool prerelease = false)
        {
            const string NugetSource = "https://api.nuget.org/v3/index.json";

            project = project.MakeAbsolute(context.Environment);

            context.Log.Information($"Updating {packageName} in {project}");

            var oldVersion = context.GetNugetPackageVersion(project, packageName);

            context.Log.Information($"Old version: {oldVersion}");

            var arguments = new ProcessArgumentBuilder();
            arguments.Append(new TextArgument("package"));
            arguments.Append(new TextArgument("-s"));
            arguments.Append(new TextArgument(NugetSource));
            if (prerelease)
                arguments.Append(new TextArgument("--prerelease"));
            arguments.Append(new TextArgument(packageName));
            context.DotNetCoreTool(project, "add", arguments);

            var newVersion = context.GetNugetPackageVersion(project, packageName);

            context.Log.Information($"New version: {oldVersion} -> {newVersion}");

            return (oldVersion, newVersion);
        }

        public static (string, string) UpdateNpmPackage(this BuildContext context, FilePath project, string packageName)
        {
            project = project.MakeAbsolute(context.Environment);

            context.Log.Information($"Updating {packageName} in {project}");

            var oldVersion = context.GetNpmPackageVersion(project, packageName);

            context.Log.Information($"Old version: {oldVersion}");

            context.NpmUpdate(s =>
            {
                s.WorkingDirectory = project.GetDirectory();
                s.ArgumentCustomization = builder =>
                {
                    builder.Append(new TextArgument(packageName));
                    return builder;
                };
            });

            context.NpmInstall(s =>
            {
                s.WorkingDirectory = project.GetDirectory();
            });

            if (File.Exists(project.GetDirectory().CombineWithFilePath("gulpfile.js").FullPath))
            {
                context.Gulp().Local.Execute(s =>
                {
                    s.WorkingDirectory = project.GetDirectory();
                });
            }

            var newVersion = context.GetNpmPackageVersion(project, packageName);

            context.Log.Information($"New version: {oldVersion} -> {newVersion}");

            return (oldVersion, newVersion);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="project"></param>
        /// <param name="version"></param>
        /// <param name="packageName">Package name without StardustDL.RazorComponents prefix.</param>
        /// <returns></returns>
        public static FilePath BuildAndPack(this BuildContext context, FilePath project, string version, string packageName)
        {
            project = project.MakeAbsolute(context.Environment);

            var settings = context.GetMSBuildSettings();
            settings.SetVersion(version);

            context.DotNetCoreBuild(project.FullPath, new Cake.Common.Tools.DotNetCore.Build.DotNetCoreBuildSettings
            {
                MSBuildSettings = settings
            });

            context.DotNetCorePack(project.FullPath, new Cake.Common.Tools.DotNetCore.Pack.DotNetCorePackSettings
            {
                OutputDirectory = Paths.Dist.Packages,
                MSBuildSettings = settings,
            });
            return Paths.Dist.Packages.CombineWithFilePath($"StardustDL.RazorComponents.{packageName}.{version}.nupkg");
        }

        public static void Deploy(this BuildContext context, FilePath package)
        {
            package = package.MakeAbsolute(context.Environment);

            var settings = new Cake.Common.Tools.DotNetCore.NuGet.Push.DotNetCoreNuGetPushSettings
            {
                SkipDuplicate = true,
            };

            if (context.AutoUpdate || context.EnableNuGetPackage)
            {
                context.Log.Information($"Publish {package} to NuGet.");

                string nugetToken = context.EnvironmentVariable("NUGET_AUTH_TOKEN", "");

                if (nugetToken is "")
                {
                    throw new Exception("No NUGET_AUTH_TOKEN environment variable setted.");
                }

                settings.ApiKey = nugetToken;
                settings.Source = "https://api.nuget.org/v3/index.json";
            }
            else if (context.InnerVersion is not null)
            {
                context.Log.Information($"Publish {package} to Azure.");

                string nugetToken = context.EnvironmentVariable("AZ_AUTH_TOKEN", "");

                if (nugetToken is "")
                {
                    throw new Exception("No AZ_AUTH_TOKEN environment variable setted.");
                }

                try
                {
                    context.DotNetCoreNuGetAddSource(RestoreTask.CustomSourceName, new Cake.Common.Tools.DotNetCore.NuGet.Source.DotNetCoreNuGetSourceSettings
                    {
                        Source = "https://sparkshine.pkgs.visualstudio.com/StardustDL/_packaging/feed/nuget/v3/index.json",
                    });
                }
                catch (Exception ex)
                {
                    context.Log.Error(ex.Message);
                }

                context.DotNetCoreNuGetUpdateSource(RestoreTask.CustomSourceName, new Cake.Common.Tools.DotNetCore.NuGet.Source.DotNetCoreNuGetSourceSettings
                {
                    Source = "https://sparkshine.pkgs.visualstudio.com/StardustDL/_packaging/feed/nuget/v3/index.json",
                    UserName = "sparkshine",
                    StorePasswordInClearText = true,
                    Password = nugetToken,
                });

                settings.ApiKey = "az";
                settings.Source = RestoreTask.CustomSourceName;
            }
            else
            {
                context.Log.Information("Disabled deploy package.");
            }

            context.DotNetCoreNuGetPush(package.FullPath, settings);
        }

        public static void AntDesigns(this BuildContext context, bool deploy = false)
        {
            var project = Paths.ProjectFile(nameof(AntDesigns));

            var version = context.GetNugetPackageVersion(project, "AntDesign");

            var package = context.BuildAndPack(project, version + (context.InnerVersion is null ? "" : $".{context.InnerVersion}"), nameof(AntDesigns));

            if (deploy) context.Deploy(package);
        }

        public static void Vditors(this BuildContext context, bool deploy = false)
        {
            var project = Paths.ProjectFile(nameof(Vditors));

            var version = context.GetNpmPackageVersion(project, "vditor");

            var package = context.BuildAndPack(project, version + (context.InnerVersion is null ? "" : $".{context.InnerVersion}"), nameof(Vditors));

            if (deploy) context.Deploy(package);
        }

        public static void MaterialDesignIcons(this BuildContext context, bool deploy = false)
        {
            var project = Paths.ProjectFile(nameof(MaterialDesignIcons));

            var version = context.GetNpmPackageVersion(project, "@mdi/font");

            var package = context.BuildAndPack(project, version + (context.InnerVersion is null ? "" : $".{context.InnerVersion}"), nameof(MaterialDesignIcons));

            if (deploy) context.Deploy(package);
        }

        public static void JQuerys(this BuildContext context, bool deploy = false)
        {
            var project = Paths.ProjectFile(nameof(JQuerys));

            var version = context.GetNpmPackageVersion(project, "jquery");

            var package = context.BuildAndPack(project, version + (context.InnerVersion is null ? "" : $".{context.InnerVersion}"), nameof(JQuerys));

            if (deploy) context.Deploy(package);
        }

        public static void Bootstraps(this BuildContext context, bool deploy = false)
        {
            var project = Paths.ProjectFile(nameof(Bootstraps));

            var version = context.GetNpmPackageVersion(project, "bootstrap");

            var package = context.BuildAndPack(project, version + (context.InnerVersion is null ? "" : $".{context.InnerVersion}"), nameof(Bootstraps));

            if (deploy) context.Deploy(package);
        }
    }
}
