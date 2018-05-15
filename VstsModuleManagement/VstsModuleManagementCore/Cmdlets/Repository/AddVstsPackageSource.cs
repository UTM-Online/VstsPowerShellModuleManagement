namespace VstsModuleManagementCore.Cmdlets
{
    using System;
    using System.Management.Automation;

    using VstsModuleManagementCore.Extensions;

    [Cmdlet("Get", "VstsPackageSource")]
    public class AddVstsPackageSource : PSCmdlet
    {
        [Parameter]
        public string AccountName { get; set; }

        [Parameter]
        public string PackageRepositoryName { get; set; }

        [Parameter]
        public SwitchParameter IsTrusted { get; set; }

        protected override void ProcessRecord()
        {
            string location = $"https://{this.AccountName}.pkgs.visualstudio.com/_packaging/{this.PackageRepositoryName}/nuget/v2";

            string providerName = $"{this.AccountName}-{this.PackageRepositoryName}-VstsModules";

            var settings = this.GetModuleSettings();

            if (settings.KnownVstsProviders.ContainsKey(providerName))
            {
                this.WriteError(new ErrorRecord(new PSInvalidOperationException("Object with the provided configured name already exists"), $"ProviderAlreadyExistsException,{this.GetType().FullName}", ErrorCategory.WriteError, providerName));
                return;
            }

            PowerShell ps = PowerShell.Create();

            ps.AddCommand("Register-PackageSource")
              .AddParameter("Name", providerName)
              .AddParameter("Location", location)
              .AddParameter("ProviderName", "PowerShellGet")
              .AddParameter("ErrorAction", "Stop")
              .AddParameter("Trusted", this.IsTrusted);

            this.WriteObject(ps.Invoke(), true);

            settings.KnownVstsProviders.Add(providerName, string.Empty);
            this.SaveModuleConfiguration(settings);
        }
    }
}