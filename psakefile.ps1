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

Task NPMUP? {
    Set-Location src/MaterialDesignIcons
    Exec { ncu }
    Set-Location ../..
    Set-Location src/Vditors
    Exec { ncu }
    Set-Location ../..
}

Task NPMUP {
    Set-Location src/MaterialDesignIcons
    Exec { ncu -u }
    Exec { npm i }
    Set-Location ../..
    Set-Location src/Vditors
    Exec { ncu -u }
    Exec { npm i }
    Set-Location ../..
}

Task Restore-UI {
    Set-Location src/MaterialDesignIcons
    Exec { npm ci }
    Exec { gulp }
    Set-Location ../..
    Set-Location src/Vditors
    Exec { npm ci }
    Exec { gulp }
    Set-Location ../..
}