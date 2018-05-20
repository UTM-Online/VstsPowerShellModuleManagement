namespace VstsModuleManagementCore.Cmdlets
{
    using System;
    using System.Linq;
    using System.Management.Automation;
    using System.Text.RegularExpressions;

    using VstsModuleManagementCore.Extensions;
    using VstsModuleManagementCore.Utilities;

    [Cmdlet(VerbsCommon.Add, "VstsPackageSource", SupportsShouldProcess = true)]
    public class AddVstsPackageSource : AbstractBaseCmdlet
    {
        [Parameter]
        public string AccountName { get; set; }

        [Parameter]
        public string PackageRepositoryName { get; set; }

        [Parameter]
        public SwitchParameter IsTrusted { get; set; }

        [Parameter]
        public SwitchParameter Force { get; set; }

        private string regexPattern = @"(?:Package Source)\s'(.*)'\s(?:exists\.)";

        protected override bool CredentialsRequired => false;

        protected override void ProcessCommand()
        {
            string location = $"https://{this.AccountName}.pkgs.visualstudio.com/_packaging/{this.PackageRepositoryName}/nuget/v2";

            string providerName = $"{this.AccountName}-{this.PackageRepositoryName}-VstsModules";

            var settings = this.GetRunTimeModuleSettings();

            if (settings.KnownVstsProviders.ContainsKey(providerName))
            {
                if (this.Force && this.ShouldContinue("Forcibly overwrite the existing entry?", $"Provider Name: {providerName}"))
                {
                    settings.KnownVstsProviders.Remove(providerName);
                }
                else
                {
                    this.WriteError(new ErrorRecord(new PSInvalidOperationException("Object with the provided configured name already exists"), $"ProviderAlreadyExistsException,{this.GetType().FullName}", ErrorCategory.WriteError, providerName));
                    return;
                }
            }

            var parameters = this.CreateParameterDictionary("Name", providerName)
                                 .AddParameter("Location", location)
                                 .AddParameter("ProviderName", "PowerShellGet")
                                 .AddParameter("ErrorAction", "Stop")
                                 .AddParameter("Trusted", this.IsTrusted);

            try
            {
                var result = PSUtils.InvokePSCommand<object>("Register-PackageSource", parameters);

                this.WriteObject(result, true);
            }
            catch (Exception ex) when (this.FilterIncomingException(ex) && this.Force)
            {
                this.WriteObject("An existing package source was discovered and the requested package source will not be re-added");
            }

            settings.KnownVstsProviders.Add(providerName, string.Empty);
            settings.SaveSettings();
        }

        private bool FilterIncomingException(Exception ex)
        {
            var regex = new Regex(this.regexPattern);

            MatchCollection matches = regex.Matches(ex.Message);

            if (matches.Count > 0)
            {
                return true;
            }

            return false;
        }
    }
}