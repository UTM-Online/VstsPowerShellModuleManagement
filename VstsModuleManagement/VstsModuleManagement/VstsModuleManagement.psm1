function Add-VstsPackageSource
{
	param
	(
		[string]$AccountName,
		[string]$PackageRepositoryName,
		[switch]$IsTrusted
	)

	$Location = "https://$($AccountName).pkgs.visualstudio.com/_packaging/$($PackageRepositoryName)/nuget/v2"

	$ProviderName = "$($AccountName)-$($PackageRepositoryName)-VstsModules"

	$SplatArray = @{}

	$SplatArray.Add("Name",$ProviderName)
	$SplatArray.Add("Location",$Location)
	$SplatArray.Add("ProviderName","PowerShellGet")
	$SplatArray.Add("ErrorAction","Stop")

	if($IsTrusted)
	{
		$SplatArray.Add("Trusted", $True)
	}

	Register-PackageSource @SplatArray

	$VstsMMS.KnownVstsProviders.Add($ProviderName,[string]::Empty)

	SaveModuleSettings
}

function Remove-VstsPackageSource
{
	param
	(
		[string]$AccountName,
		[string]$PackageRepositoryName
	)

	$ProviderName = "$($AccountName)-$($PackageRepositoryName)-VstsModules"

	Unregister-PackageSource -Name $ProviderName -Confirm:$false

	if($VstsMMS.KnownVstsProviders.ContainsKey($ProviderName))
	{
		$VstsMMS.KnownVstsProviders.Remove($ProviderName)
	}
	else
	{
		Write-Warning "A package source matching the input parameters was not found and was not removed from the known providers list."
	}
}

function Get-VstsPackageSources
{
	Get-PackageSource -Name "*-VstsModules"
}

function Join-VstsPackageSourceWithCredential
{
	param
	(
		[string]$PackageSourceName,
		[string]$CredentialName
	)

	if($VstsMMS.KnownVstsProviders.ContainsKey($PackageSourceName))
	{
		$VstsMMS.KnownVstsProviders[$PackageSourceName] = $CredentialName
	}
	else
	{
		$VstsMMS.KnownVstsProviders.Add($PackageSourceName, $CredentialName)
	}

	SaveModuleSettings
}

function Find-VstsModule
{
	param
	(
		[string]$Name,
		[string]$Repository
	)

	$cred = Import-Clixml -Path "$PSScriptRoot\$($VstsMMS.KnownVstsProviders | ?{$_.Name -eq $Repository} | Select -ExpandProperty Value)"

	$SplatArray = @{}

	if($Name)
	{
		$SplatArray.Add("Name",$Name)
	}

	$SplatArray.Add("Repository",$Repository)

	$SplatArray.Add("Credentials",$creds)


	Find-Module @SplatArray
}

function Save-VstsPat
{
	param
	(
		[string]$UserUpn,
		[string]$PAT,
		[string]$AccountName
	)

	$password = ConvertTo-SecureString $PAT -AsPlainText -Force
	$vstsCredential = New-Object System.Management.Automation.PSCredential $UserUpn, $password

	$FileSystemSafeUpn = $UserUpn.Replace('@','_')
	$TargetPath = "$($PSScriptRoot)\$($FileSystemSafeUpn)-$($AccountName).vstscreds"

	if((Test-Path -Path $TargetPath) -eq $true)
	{
		Remove-Item -Path $TargetPath -Confirm:$false -Force:$true
	}

	$vstsCredential | Export-Clixml -Path $TargetPath
}

function Get-VstsCredential
{
	param
	(
		[string]$UserUpn,
		[string]$AccountName,
		[switch]$ReturnCredentalBlob
	)

	if($ReturnCredentalBlob)
	{
		$FileSystemSafeUpn = $UserUpn.Replace('@','_')
		$TargetPath = "$($PSScriptRoot)\$($FileSystemSafeUpn)-$($AccountName).vstscreds"

		if((Test-Path -Path $TargetPath) -eq $true)
		{
			return Import-Clixml -Path $TargetPath
		}
		else
		{
			Write-Error -Message "The Specified combination of Account Name and User Upn weren't found.  Please run Save-VstsPat to save credentials."
		}
	}
	else
	{
		Get-ChildItem -Path $PSScriptRoot -Filter "*.vstscreds" | FT Name,LastWriteTime
	}
}

function SaveModuleSettings
{
	ConvertTo-Json $VstsMMS | Out-File $PSScriptRoot\ModuleSettings.json
}

