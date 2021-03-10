properties {
    $NUGET_AUTH_TOKEN = $env:NUGET_AUTH_TOKEN
    $build_version = $env:build_version
}

Task NPMUP? {
    Set-Location src/MaterialDesignIcons
    Exec { ncu }
    Set-Location ../..
    Set-Location src/Vditors
    Exec { ncu }
    Set-Location ../..
    Set-Location src/JQuerys
    Exec { ncu }
    Set-Location ../..
    Set-Location src/Bootstraps
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
    Set-Location src/JQuerys
    Exec { ncu -u }
    Exec { npm i }
    Set-Location ../..
    Set-Location src/Bootstraps
    Exec { ncu -u }
    Exec { npm i }
    Set-Location ../..
}