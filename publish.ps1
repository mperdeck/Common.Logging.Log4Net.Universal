# =================================================
# Run this script to publish all versions of Common.Logging.Log4Net.Universal
# =================================================

[CmdletBinding()]
Param(
  [Parameter(Mandatory=$True,Position=1, HelpMessage="Version number of new version. Format: <Major Version>.<Minor Version>.<Bug Fix>.<Build Number>")]
  [string]$version,

  [Parameter(Mandatory=$False, HelpMessage="Only generates, does not publish to NuGet or to to site, and no push to Github")]
  [switch]$GenerateOnly
)

# include nuget key
."..\keys.ps1"

# ---------------
# Constants

$versionPlaceholder = "__Version__"

# ---------------
# Update version numbers

Function ApplyVersion([string]$templatePath)
{
	# Get file path without the ".template" extension  
	$filePath = [System.IO.Path]::Combine([System.IO.Path]::GetDirectoryName($_.FullName), [System.IO.Path]::GetFileNameWithoutExtension($_.FullName))

	# Copy template file to file with same name but without ".template"
	# Whilst coying, replace __Version__ placeholder with version
	# Must use encoding ascii. bower register (used further below) does not understand other encodings, such as utf 8.
	(Get-Content $templatePath) | Foreach-Object {$_ -replace $versionPlaceholder, $version} | Out-File -encoding ascii $filePath

    Write-Host "Updated version in : $filePath"
}

# Visit all files in current directory and its sub directories that end in ".template", and call ApplyVersion on them.
get-childitem '.' -recurse -force | ?{$_.extension -eq ".template"} | ForEach-Object { ApplyVersion $_.FullName }

# ---------------

cd Common.Logging.Log4Net.Universal

# Upload Nuget package for .Net version
nuget pack Common.Logging.Log4Net.Universal.csproj -Prop Configuration=Release -Build -OutputDirectory c:\Dev\@NuGet\GeneratedPackages
if (-Not $GenerateOnly) { nuget push c:\Dev\@NuGet\GeneratedPackages\Common.Logging.Log4Net.Universal.$version.nupkg $apiKey -Source https://api.nuget.org/v3/index.json }

cd ..

# Commit any changes and deletions (but not additions) to Github
# git commit -a -m "$version"
		
# Push to Github		
# git tag $version
# git push https://github.com/$githubUsername/Common.Logging.Serilog.StructuredText.git --tags

# git branch $version
# git push https://github.com/$githubUsername/Common.Logging.Serilog.StructuredText.git --all

Exit

		

