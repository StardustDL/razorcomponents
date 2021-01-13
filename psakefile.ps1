properties {
    $NUGET_AUTH_TOKEN = $env:NUGET_AUTH_TOKEN
    $build_version = $env:build_version
}

Task default -depends Restore, Build

Task CI -depends Restore, Build, Pack, Test, Benchmark, Report

Task CD -depends Restore, Build, Pack

Task Deploy -depends publish-packages

Task Restore -depends Restore-UI {
    Exec { dotnet tool restore }
    Exec { dotnet restore }
}

Task Build {
    Exec -maxRetries 3 { dotnet build -c Release /p:Version=$build_version }
}

Task Test {
    # if (-not (Test-Path -Path "reports/test")) {
    #     New-Item -Path "reports/test" -ItemType Directory
    # }
    # Exec { dotnet test -c Release --logger GitHubActions /p:CollectCoverage=true /p:CoverletOutput=../../reports/test/coverage.json /p:MergeWith=../../reports/test/coverage.json /maxcpucount:1 }
    # Exec { dotnet test -c Release ./test/Test.Base --logger GitHubActions /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput=../../reports/test/coverage.xml /p:MergeWith=../../reports/test/coverage.json }
}

Task Benchmark {
    # Exec { dotnet run -c Release --project ./test/Benchmark.Base }
}

Task Report {
    # Exec { ./tools/reportgenerator -reports:./reports/test/coverage.xml -targetdir:./reports/test }
    # if (-not (Test-Path -Path "reports/benchmark")) {
    #     New-Item -Path "reports/benchmark" -ItemType Directory
    # }
    # Copy-Item ./BenchmarkDotNet.Artifacts/* ./reports/benchmark -Recurse
}

Task Pack {
    if (-not (Test-Path -Path "packages")) {
        New-Item -Path "packages" -ItemType Directory
    }

    Exec -maxRetries 10 { dotnet pack -c Release /p:Version=$build_version -o ./packages }
}

Task publish-packages {
    Exec { dotnet nuget add source https://sparkshine.pkgs.visualstudio.com/StardustDL/_packaging/feed/nuget/v3/index.json -n ownpkgs }
    Exec { dotnet nuget update source ownpkgs -u sparkshine -p $NUGET_AUTH_TOKEN --store-password-in-clear-text }
    Exec { dotnet nuget push ./packages/StardustDL.RazorComponents.AntDesigns.$build_version.nupkg -s ownpkgs -k az --skip-duplicate }
    Exec { dotnet nuget push ./packages/StardustDL.RazorComponents.MaterialDesignIcons.$build_version.nupkg -s ownpkgs -k az --skip-duplicate }
}

Task publish-packages-release {
    Exec { dotnet nuget push ./packages/StardustDL.RazorComponents.AntDesigns.$build_version.nupkg  -s https://api.nuget.org/v3/index.json -k $NUGET_AUTH_TOKEN --skip-duplicate }
    Exec { dotnet nuget push ./packages/StardustDL.RazorComponents.MaterialDesignIcons.$build_version.nupkg  -s https://api.nuget.org/v3/index.json -k $NUGET_AUTH_TOKEN --skip-duplicate }
}

Task NPMUP? {
    Set-Location src/MaterialDesignIcons
    Exec { ncu }
    Set-Location ../..
}

Task NPMUP {
    Set-Location src/MaterialDesignIcons
    Exec { ncu -u }
    Exec { npm i }
    Set-Location ../..
}

Task Restore-UI {
    Set-Location src/MaterialDesignIcons
    Exec { npm ci }
    Exec { gulp }
    Set-Location ../..
}