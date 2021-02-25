using Cake.Common;
using Cake.Common.Build;
using Cake.Common.Tools.DotNetCore.MSBuild;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.Frosting;
using System;
using System.Collections.Generic;

namespace Build
{
    public enum SolutionType
    {
        None,
        All,
    }

    public class BuildContext : FrostingContext
    {
        const string Version = "0.0.2";

        const int BuildRunNumberOffset = 0;

        public string CommitMessage { get; set; }

        public bool EnableDocument { get; set; }

        public bool EnableNuGetPackage { get; set; }

        public string BuildConfiguration { get; set; }

        public string BuildVersionSuffix { get; set; }

        public bool Release { get; set; }

        public bool AutoUpdate { get; set; } = false;

        public SolutionType Solution { get; set; }


        public IEnumerable<FilePath> SolutionFiles => Solution switch
        {
            _ => Paths.Solutions,
        };

        public DotNetCoreMSBuildSettings GetMSBuildSettings()
        {
            return new DotNetCoreMSBuildSettings().SetVersionSuffix(BuildVersionSuffix)
                                                  .SetConfiguration(BuildConfiguration);
        }

        public BuildContext(ICakeContext context)
            : base(context)
        {
            Release = context.HasArgument("release");

            CommitMessage = context.Argument("commit", "");
            if (CommitMessage is "")
            {
                CommitMessage = context.EnvironmentVariable("COMMIT_MESSAGE", "");
            }

            Solution = SolutionType.All;
            Solution = context.Argument("solution", "").ToLowerInvariant() switch
            {
                "all" => SolutionType.All,
                _ => Solution,
            };

            foreach (var item in SolutionFiles)
            {
                context.Log.Information($"Selected solution: {item.FullPath}");
            }

            BuildConfiguration = context.Argument("configuration", "Release");
            EnableDocument = CommitMessage.Contains("/docs");
            EnableNuGetPackage = CommitMessage.Contains("/pkgs");

            BuildVersionSuffix = context.Argument("build-version-suffix", "");
            if (BuildVersionSuffix is "")
            {
                BuildVersionSuffix = context.EnvironmentVariable("BUILD_VERSION_SUFFIX", Version);
            }
            {
                var actions = context.GitHubActions();
                if (actions.IsRunningOnGitHubActions)
                {
                    if (actions.Environment.Workflow.Workflow == "CI")
                    {
                        BuildVersionSuffix += $"-preview.{Math.Max(1, actions.Environment.Workflow.RunNumber - BuildRunNumberOffset)}";
                    }
                    else if (actions.Environment.Workflow.Workflow == "Release")
                    {
                        Release = true;
                    }
                    else if (actions.Environment.Workflow.Workflow == "AutoUpdate")
                    {
                        AutoUpdate = true;
                    }
                }
            }

            EnableDocument = EnableDocument || Release;
            EnableNuGetPackage = EnableNuGetPackage || Release;
        }
    }
}