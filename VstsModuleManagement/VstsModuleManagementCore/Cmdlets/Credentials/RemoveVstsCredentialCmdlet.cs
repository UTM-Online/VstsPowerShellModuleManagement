// ***********************************************************************
// Assembly         : VstsModuleManagementCore
// Author           : joirwi
// Created          : 5/17/2018 5:49:28 PM
//
// Last Modified By : joirwi
// Last Modified On : 5/17/2018 5:49:28 PM
// ***********************************************************************
// <copyright file="RemoveVstsCredentialCmdlet.cs" company="Microsoft">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// **********************************************************************

namespace VstsModuleManagementCore.Cmdlets.Credentials
{
    using System.IO;
    using System.Management.Automation;

    using VstsModuleManagementCore.Models;

    /// <summary>
    /// Class RemoveVstsCredentialCmdlet.
    /// </summary>
    /// <seealso cref="AbstractBaseCmdlet" />
    [Cmdlet(VerbsCommon.Remove, "VstsCredential", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High)]
    public class RemoveVstsCredentialCmdlet : AbstractBaseCmdlet
    {
        [Parameter]
        public string UserUpn { get; set; }

        [Parameter]
        public string AccountName { get; set; }

        protected override void BeginProcessingCommand()
        {
            this.UserUpn = this.UserUpn.Replace('@', '_');
        }

        /// <summary>
        /// Processes the command.
        /// </summary>
        protected override void ProcessCommand()
        {
            string targetFile = $"{this.UserUpn}-{this.AccountName}.vstscreds";

            if (File.Exists($"{ModuleRunTimeState.ModuleBasePath}\\{targetFile}"))
            {
                if (this.ShouldProcess(targetFile, "Pertinently Delete Credential File"))
                {
                    File.Delete($"{ModuleRunTimeState.ModuleBasePath}\\{targetFile}"); 
                }
            }
            else
            {
                this.WriteWarning("No action has been taken because either the credential file wasn't found on your hard drive or you declined at the confirmation prompt");
            }
        }
    }
}