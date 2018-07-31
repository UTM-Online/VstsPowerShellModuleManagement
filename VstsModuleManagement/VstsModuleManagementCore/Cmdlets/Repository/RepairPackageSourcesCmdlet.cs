using System.Linq;
using System.Management.Automation;
using VstsModuleManagementCore.Models.ReturnObjects;

namespace VstsModuleManagementCore.Cmdlets
{
    [Cmdlet(VerbsDiagnostic.Repair, "VstsPackageSources")]
    public class RepairPackageSourcesCmdlet : AbstractBaseCmdlet
    {
        protected override bool CredentialsRequired => false;
        
        protected override void ProcessCommand()
        {
            var knownSources = this.GetPsVariable<PackageSource[]>("Get-PackageSource");

            var moduleSettings = this.GetRunTimeModuleSettings();

            foreach (var item in knownSources)
            {
                if (!moduleSettings.KnownVstsProviders.ContainsKey(item.Name))
                {
                    this.WriteObject($"Adding missing provider {item.Name}");
                    moduleSettings.KnownVstsProviders.Add(item.Name, item.Location);
                }
            }

            foreach (var item in moduleSettings.KnownVstsProviders)
            {
                if (!knownSources.Any(kp => kp.Name == item.Key))
                {
                    this.WriteObject($"Removing unknown provider {item.Key}");
                    moduleSettings.KnownVstsProviders.Remove(item.Key);
                }
            }
            
            moduleSettings.SaveSettings();
        }
    }
}