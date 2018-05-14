function Load-ModuleSettingsFile
{
	Param
	(
		[string]$SettingsPath
	)
	$ErrorActionPreference = "Stop"

	$TempSettings = [ModuleSettings]::New()

	$RawSettings = Get-Content -Path $SettingsPath | ConvertFrom-Json

	$TempSettings.HasCompleatedFirstRun = $RawSettings.HasCompleatedFirstRun

	foreach($provider in $RawSettings.KnownVstsProviders)
	{
		$TempSettings.KnownVstsProviders.Add($provider.Key,$provider.Value)
	}

	return $TempSettings
}