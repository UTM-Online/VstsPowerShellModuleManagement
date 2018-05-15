namespace VstsModuleManagementCore.Cmdlets
{
    using System.Management.Automation;

    using VstsModuleManagementCore.Extensions;
    using VstsModuleManagementCore.Models;
    using VstsModuleManagementCore.Resources;
    using VstsModuleManagementCore.Utilities;

    [Cmdlet("Join", "VstsPackageSourceWithCredential")]
    public class JoinVstsPackageSourceWithCredential : PSCmdlet
    {
        [Parameter]
        public string PackageSourceName { get; set; }

        [Parameter]
        public string CredentialName { get; set; }

        protected override void ProcessRecord()
        {
            var settings = PSUtils.GetPSVariable<ModuleSettings>(ModuleVariables.ModuleSettings);

            if (settings.KnownVstsProviders.ContainsKey(this.PackageSourceName))
            {
                settings.KnownVstsProviders[this.PackageSourceName] = this.CredentialName;
            }
            else
            {
                settings.KnownVstsProviders.Add(this.PackageSourceName, this.CredentialName);
            }

            this.SaveModuleConfiguration(settings);
        }
    }
}