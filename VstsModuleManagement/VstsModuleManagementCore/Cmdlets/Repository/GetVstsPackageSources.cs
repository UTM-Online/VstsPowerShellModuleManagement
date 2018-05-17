namespace VstsModuleManagementCore.Cmdlets
{
    using System.Collections.Generic;
    using System.Management.Automation;

    using VstsModuleManagementCore.Extensions;
    using VstsModuleManagementCore.Models.ReturnObjects;
    using VstsModuleManagementCore.Utilities;

    [Cmdlet(VerbsCommon.Get, "VstsPackageSources")]
    public class GetVstsPackageSources : AbstractBaseCmdlet
    {
        protected override void ProcessCommand()
        {
            var results = PSUtils.InvokePSCommand<dynamic>("Get-PackageSource", this.CreateParameterDictionary("Name", "*-VstsModules"));

            List<PackageSource> sourcesList = new List<PackageSource>();

            foreach (var item in results)
            {
                sourcesList.Add(new PackageSource(item.Name, item.Location));
            }

            this.WriteObject(sourcesList, true);
        }
    }
}