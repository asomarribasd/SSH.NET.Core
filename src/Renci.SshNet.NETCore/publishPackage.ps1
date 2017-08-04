$prjName = Split-Path $PSScriptRoot -Leaf

dotnet build $PSScriptRoot -c Release
dotnet pack $PSScriptRoot -c Release --include-symbols

$packName = $prjName -replace "\.", ""
$filename = gci $PSScriptRoot\bin\Release\$packName.*.symbols.nupkg | sort Date | select -last 1


gci $PSScriptRoot\bin\Release\$packName.*.symbols.nupkg
Write-Output $filename.FullName
return

#C:\Crypttech\NugetPackages\nuget.exe add "$($filename.FullName)" -Source C:\Crypttech\NugetPackages
\\172.17.7.17\Yazilim\NugetPackages\nuget.exe add "$($filename.FullName)" -Source \\172.17.7.17\Yazilim\NugetPackages