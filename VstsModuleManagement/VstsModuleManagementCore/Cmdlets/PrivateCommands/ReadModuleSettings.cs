namespace VstsModuleManagementCore.Cmdlets
{
    using System.Management.Automation;

    using VstsModuleManagementCore.Extensions;
    using VstsModuleManagementCore.Models;
    using VstsModuleManagementCore.Resources;
    using VstsModuleManagementCore.Utilities;

    [Cmdlet(VerbsCommunications.Read, "ModuleSettings")]
    public class ReadModuleSettings : AbstractBaseCmdlet
    {
        protected override void ProcessCommand()
        {
            var moduleSettings = ModuleSettings.LoadSettings();

            this.SetPsVariable("VstsMMS", moduleSettings);

            this.WriteInformation("Setting File Has Been Loaded", null);
        }
    }
}