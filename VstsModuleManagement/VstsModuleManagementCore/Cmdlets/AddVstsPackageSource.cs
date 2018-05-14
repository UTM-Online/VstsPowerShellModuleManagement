namespace VstsModuleManagementCore.Cmdlets
{
    using System.Management.Automation;

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

            PowerShell ps = PowerShell.Create();

            ps.AddCommand("Register-PackageSource")
              .AddParameter("Name", providerName)
              .AddParameter("Location", location)
              .AddParameter("ProviderName", "PowerShellGet")
              .AddParameter("ErrorAction", "Stop")
              .AddParameter("Trusted", this.IsTrusted);

            this.WriteObject(ps.Invoke(), true);
        }
    }
}