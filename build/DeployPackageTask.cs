using Cake.Common.IO;
using Cake.Common.Tools.DotNetCore;
using Cake.Core.IO;
using Cake.Frosting;
using Cake.Common;
using System.Collections.Generic;
using System.Linq;
using Cake.Core.Diagnostics;
using Cake.Common.Tools.DotNetCore.NuGet.Push;
using System;

namespace Build
{
    [TaskName("Deploy-Packages")]
    [IsDependentOn(typeof(PackTask))]
    public sealed class DeployPackageTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            context.Vditors(true);
            context.AntDesigns(true);
            context.MaterialDesignIcons(true);
            context.JQuerys(true);
            context.Bootstraps(true);
        }
    }
}