namespace VstsModuleManagementCore.Cmdlets
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Management.Automation;

    using VstsModuleManagementCore.Extensions;
    using VstsModuleManagementCore.Models;
    using VstsModuleManagementCore.Utilities;

    [Cmdlet(VerbsCommon.Get, "VstsCredential")]
    public class GetVstsCredential : PSCmdlet
    {
        [Parameter]
        public string UserUpn { get; set; }

        [Parameter]
        public string AccountName { get; set; }

        [Parameter]
        public SwitchParameter ReturnCredentialBlob { get; set; }

        protected override void BeginProcessing()
        {
            if (!string.IsNullOrEmpty(this.AccountName))
            {
                this.AccountName = this.AccountName.ToUpper();
            }
        }

        protected override void ProcessRecord()
        {
            this.WriteVerbose("Entering ProcessRecord");
            if (this.ReturnCredentialBlob)
            {
                this.WriteVerbose($"{nameof(this.ReturnCredentialBlob)} was specified.  Retrieving specified credential blob from disk.");
                var fileSystemSafeUpn = this.UserUpn.Replace('@', '_');
                this.WriteDebug($"File system Safe UPN is {fileSystemSafeUpn}");

                var targetPath = $"{ModuleRunTimeState.ModuleBasePath}\\{fileSystemSafeUpn}-{this.AccountName}.vstscreds";
                this.WriteDebug($"Target path was determined to be {targetPath}");

                if (File.Exists(targetPath))
                {
                    this.WriteVerbose("The target file was found on the file system, importing the credential blob and returning it to the caller");
                    this.WriteObject(PSUtils.InvokePSCommand<PSCredential>("Import-Clixml", this.CreateParameterDictionary("Path", targetPath)), true);
                }
                else
                {
                    this.WriteError(new ErrorRecord(new InvalidOperationException("The Specified combination of Account Name and User Upn weren't found.  Please run Save-VstsPat to save credentials."), $"CredentialNotFoundException,{this.GetType().FullName}", ErrorCategory.InvalidOperation, this));
                }
            }
            else
            {
                this.WriteVerbose($"{nameof(this.ReturnCredentialBlob)} was not specified, enumerating all available credential blobs.");
                var parameters = new ParameterCollection("Path", ModuleRunTimeState.ModuleBasePath).AddParameter("Filter", "*.vstscreds");

                this.WriteVerbose("Executing Get-ChildItem to enumerate all credential blobs");
                var result = PSUtils.InvokePSCommand<FileInfo>("Get-ChildItem", parameters);

                if (result.Count > 0)
                {
                    this.WriteVerbose("Results were found.  Parsing results and returning an anonymous type based on the incoming type");
                    var filterResults = new List<object>();

                    foreach (FileInfo o in result)
                    {
                        filterResults.Add(new { Name = o.Name, LastWriteTime = o.LastWriteTime });
                    }

                    this.WriteObject(filterResults, true);
                }
                else
                {
                    this.WriteWarning("No credential objects were found!");
                }
            }
        }
    }
}