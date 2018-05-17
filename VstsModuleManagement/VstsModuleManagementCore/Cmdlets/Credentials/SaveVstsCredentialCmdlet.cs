namespace VstsModuleManagementCore.Cmdlets
{
    using System.IO;
    using System.Management.Automation;
    using System.Security;

    using VstsModuleManagementCore.Extensions;
    using VstsModuleManagementCore.Models;
    using VstsModuleManagementCore.Utilities;

    [Cmdlet(VerbsData.Save, "VstsCredential")]
    public class SaveVstsCredentialCmdlet : AbstractBaseCmdlet
    {
        [Parameter]
        public string UserUpn { get; set; }

        [Parameter]
        public string PAT { get; set; }

        [Parameter]
        public string AccountName { get; set; }

        protected override void BeginProcessingCommand()
        {
            this.AccountName = this.AccountName.ToUpper();
        }

        protected override void ProcessCommand()
        {
            SecureString password = this.PAT.ConvertToSecureString();

            PSCredential vstsCredential = new PSCredential(this.UserUpn, password);

            string fileSystemSafeUpn = this.UserUpn.Replace('@', '_');
            string targetPath = $"{ModuleRunTimeState.ModuleBasePath}\\{fileSystemSafeUpn}-{this.AccountName}.vstscreds";

            if (File.Exists(targetPath))
            {
                File.Delete(targetPath);
            }

            var credArray = new[] { vstsCredential };

            var cmdletParameters = this.CreateParameterDictionary("Path", targetPath);

            PSUtils.InvokePSCommand("Export-Clixml", cmdletParameters, credArray);
        }
    }
}