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

        protected override void ProcessCommand()
        {
            var creds = PSUtils.ImportCredential(this.Repository);

            var parameters = new Dictionary<string, object>()
                                 {
                                     { "Repository", this.Repository },
                                     { "Credentials", creds }
                                 };

            if (this.Name != null)
            {
                parameters.Add("Name", this.Name);
            }

            this.WriteObject(PSUtils.InvokePSCommand<object>("Find-Module", parameters));
        }
    }
}