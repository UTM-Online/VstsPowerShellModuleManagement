namespace VstsModuleManagementCore.Cmdlets
{
    using System.Collections.Generic;
    using System.Management.Automation;

    using VstsModuleManagementCore.Utilities;

    [Cmdlet(VerbsCommon.Find, "VstsModule")]
    public class FindVstsModule : AbstractBaseCmdlet
    {
        [Parameter]
        public string Name { get; set; }

        [Parameter]
        public string Repository { get; set; }

        protected override bool CredentialsRequired => true;

        protected override void ProcessCommand()
        {
            var parameters = new Dictionary<string, object>()
                                 {
                                     { "Repository", this.Repository },
                                     { "Credential", this.Creds }
                                 };

            if (this.Name != null)
            {
                parameters.Add("Name", this.Name);
            }

            this.WriteObject(PSUtils.InvokePSCommand<object>("Find-Module", parameters));
        }
    }
}