#
# VstsModuleManagement.ps1
#

#Load Objects
foreach($File in (Get-ChildItem -Path "$PSScriptRoot\Objects" -Filter "*.ps1"))
{
	. $($File.FullName)
}

#Load Helpers
foreach($File in (Get-ChildItem -Path "$PSScriptRoot\Helpers" -Filter "*.ps1"))
{
	. $($File.FullName)
}

try
{
	[ModuleSettings]$Global:VstsMMS = Load-ModuleSettings -SettingsPath "$PSScriptRoot\ModuleSettings.json"
}
catch
{
	[ModuleSettings]$Global:VstsMMS = Repair-ModuleSettingsFile -SettingsPath "$PSScriptRoot\ModuleSettings.json"
}