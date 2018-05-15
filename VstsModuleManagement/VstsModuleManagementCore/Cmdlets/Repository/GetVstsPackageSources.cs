namespace VstsModuleManagementCore.Cmdlets
{
    using System.Collections.Generic;
    using System.Management.Automation;

    using VstsModuleManagementCore.Utilities;

    [Cmdlet("Get", "VstsPackageSources")]
    public class GetVstsPackageSources : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            var parameters = new Dictionary<string, object> { { "Name", "*-VstsModules" } };

            var results = PSUtils.InvokePSCommand<object>("Get-PackageSource", parameters);

            this.WriteObject(results, true);
        }
    }
}