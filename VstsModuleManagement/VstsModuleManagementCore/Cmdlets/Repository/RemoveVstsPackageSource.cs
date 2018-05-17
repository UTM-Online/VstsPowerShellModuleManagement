namespace VstsModuleManagementCore.Cmdlets
{
    using System.Collections.Generic;
    using System.Management.Automation;

    using VstsModuleManagementCore.Extensions;
    using VstsModuleManagementCore.Resources;
    using VstsModuleManagementCore.Utilities;

    [Cmdlet(VerbsCommon.Remove,"VstsPackageSource")]
    public class RemoveVstsPackageSource : AbstractBaseCmdlet
    {
        [Parameter]
        public string AccountName { get; set; }

        [Parameter]
        public string PackageRepositoryName { get; set; }

        protected override void ProcessCommand()
        {
            var providerName = $"{this.AccountName}-{this.PackageRepositoryName}-{CommonStrings.VstsProviderSufix}";

            var parameters = new Dictionary<string, object>
                                 {
                                     { "Name", providerName }
                                 };

            PSUtils.InvokePSCommand("Unregister-PackageSource", parameters);

            var moduleSettings = this.GetRunTimeModuleSettings();

            if (moduleSettings.KnownVstsProviders.ContainsKey(providerName))
            {
                moduleSettings.KnownVstsProviders.Remove(providerName);
                moduleSettings.SaveSettings();
            }
            else
            {
                this.WriteWarning("A package source matching the input parameters was not found and was not removed from the known providers list.");
            }
        }
    }
}