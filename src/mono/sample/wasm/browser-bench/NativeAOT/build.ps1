param
(
    [ValidateSet("build", "buildLocal", "run", "runLocal", "clean")][string]$target = "build",
    [string]$localPackageLocation
)

function ConfigureLocal
{

}

function Clean
{
    Write-Output "Cleaning artifacts..."
    Remove-Item -Recurse -Force -ErrorAction SilentlyContinue .\bin
    Remove-Item -Recurse -Force -ErrorAction SilentlyContinue .\obj
    Remove-Item -Recurse -Force -ErrorAction SilentlyContinue .\packages
}

function Build
{
    param
    (
        [bool]$local = 0,
        [bool]$run = 0
    )

    if ($local)
    {
        if ($run)
        {
            Write-Output "Running local..."
        }
        else
        {
            Write-Output "Building local..."
        }
        
        $localRestoreDir = New-Item -Path . -Name "packages" -ItemType "directory"
        Write-Output "Created local restore dir at: $(($localRestoreDir).FullName)"

        $env:NUGET_PACKAGES=($localRestoreDir).FullName
        Write-Output "NUGET_PACKAGES='$($env:NUGET_PACKAGES)'"

        $localBuildArgs = "-p:BuildWithLocalPackage=true -p:LocalPackageLocation=$localPackageLocation"
    }
    else
    {
        if ($run)
        {
            Write-Output "Running..."
        }
        else
        {
            Write-Output "Building..."
        }
    }

    $dotnetFullPath = Resolve-Path "..\..\..\..\..\..\dotnet.cmd"
    $buildArgs = "publish -c Release -r browser-wasm $($localBuildArgs) Wasm.Console.Bench.Sample.csproj -bl:NativeAOT.binlog"
    
    Write-Output "Build cmd: $($dotnetFullPath.Path) $($buildArgs)"
    Start-Process -PassThru -FilePath $dotnetFullPath.Path -NoNewWindow -ArgumentList $buildArgs | Wait-Process

    if ($run)
    {
        Write-Output "Copying index.html ..."
        $publishDir = "bin\Release\net8.0\browser-wasm\publish\"
        $dotnetServeArgs = "-o -d $($publishDir)"
        Copy-Item -Path "$($publishDir)Wasm.Console.Bench.Sample.html" -Destination "$($publishDir)index.html"
        
        Write-Output "Serving with dotnet-serve.exe ..." 
        Write-Output "Build cmd: dotnet-serve.exe $($dotnetServeArgs)"
        Start-Process -PassThru -FilePath "dotnet-serve.exe" -NoNewWindow -ArgumentList $dotnetServeArgs | Wait-Process
    }
}

function CheckLocalPackageLocation
{
    try
    {
        if (!(Test-Path -Path $localPackageLocation))
        {
            throw "Path doesn't exist at: '$($localPackageLocation)'"
        }
        Return 1
    }
    catch
    {
        Write-Error "localPackageLocation parameter is invalid - $($_.Exception.Message)";
        Return 0
    }
}

if (($target -eq "buildLocal") -or ($target -eq "runLocal"))
{
    if (!(CheckLocalPackageLocation))
    {
        Exit
    }
}

try
{
    Push-Location $PSScriptRoot
    switch ($target)
    {
        "build"
        { 
            Clean
            Build 0 0
        }
        "buildLocal"
        {
            Clean
            Build 1 0
        }
        "run"
        {
            Clean
            Build 0 1
        }
        "runLocal"
        {
            Clean
            Build 1 1
        }
        "clean"
        {
            Clean
        }
    }
}
catch
{
    Write-Output "$($_.Exception.Message)"
}
finally
{
    $env:NUGET_PACKAGES=''
    Pop-Location
}