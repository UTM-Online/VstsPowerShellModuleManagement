function Repair-ModuleSettingsFile
{
	param
	(
		[string]$SettingsPath
	)

    $ErrorActionPreference = "Stop"

	$RawSettings = Get-Content -Path $SettingsPath | ConvertFrom-Json

	try
	{
		[bool]$HasCompleatedFirstRun = $RawSettings.HasCompleatedFirstRun
	}
	catch
	{
		Write-Warning "Unable to parse your setting for Compleating First Run.  Returning Value to False."
		[bool]$HasCompleatedFirstRun = $false
	}

	try
	{
		if($RawSettings.KnownVstsProviders.Count -gt 0)
		{
		  [System.Collections.Generic.Dictionary[[string],[string]]]$KnownProviders = $RawSettings.KnownVstsProviders
		}
		else
		{
			[System.Collections.Generic.Dictionary[[string],[string]]]$KnownProviders = [System.Collections.Generic.Dictionary[[string],[string]]]::new()
		}
	}
	catch
	{
		Write-Warning "Unfortunitly we seem to be unable to restore your previous list of known providers and we've had to start over with an empty list.  Please run the Join-VstsPackageSourceWithCredential cmdlet for your registered module providers to remap your credentials to their repositories"
		$KnownProviders = [System.Collections.Generic.Dictionary[[string],[string]]]::new()
	}

	[ModuleSettings]$tempSettingsObject = [ModuleSettings]::New()

	$tempSettingsObject.HasCompleatedFirstRun = $HasCompleatedFirstRun
	$tempSettingsObject.KnownVstsProviders = $KnownProviders

	$tempSettingsObject | ConvertTo-Json | Out-File -FilePath $SettingsPath -Force

	return $tempSettingsObject
}