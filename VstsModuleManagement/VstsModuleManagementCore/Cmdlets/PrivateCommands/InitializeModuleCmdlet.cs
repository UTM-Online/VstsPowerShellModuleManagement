// ***********************************************************************
// Assembly         : VstsModuleManagementCore
// Author           : joirwi
// Created          : 5/24/2018 11:49:46 AM
//
// Last Modified By : joirwi
// Last Modified On : 5/24/2018 11:49:46 AM
// ***********************************************************************
// <copyright file="InitializeModuleCmdlet.cs" company="Microsoft">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// **********************************************************************

namespace VstsModuleManagementCore.Cmdlets.PrivateCommands
{
    using System.Linq;
    using System.Management.Automation;

    using Newtonsoft.Json;

    using VstsModuleManagementCore.Models;
    using VstsModuleManagementCore.Resources;
    using VstsModuleManagementCore.Utilities;

    /// <summary>
    /// Class InitializeModuleCmdlet.
    /// </summary>
    /// <seealso cref="AbstractBaseCmdlet" />
    [Cmdlet(VerbsData.Initialize, "Module")]
    public class InitializeModuleCmdlet : AbstractBaseCmdlet
    {
        /// <summary>
        /// Gets a value indicating whether [credentials required].
        /// </summary>
        /// <value><c>true</c> if [credentials required]; otherwise, <c>false</c>.</value>
        protected override bool CredentialsRequired => false;

        protected override void BeginProcessingCommand()
        {
            if (!ModuleRunTimeState.ConfigDirectoryExsists)
            {
                ModuleRunTimeState.ModuleDataDirectory.Create();
            }

            if (!ModuleRunTimeState.CredentialsDirectoryExists)
            {
                ModuleRunTimeState.ModuleCredentialDirectory.Create();
            }
        }

        /// <summary>
        /// Processes the command.
        /// </summary>
        protected override void ProcessCommand()
        {
            var commands = PSUtils.InvokePSCommand<CmdletInfo>(
                                                               "Get-Command",
                                                               new ParameterCollection(
                                                                                       "Module",
                                                                                       "VstsModuleManagement"));

            foreach (var cmdlet in ModuleRunTimeState.BlackListedCommands)
            {
                commands.First(c => c.Name == cmdlet.Name).Visibility = SessionStateEntryVisibility.Private;
            }
        }
    }
}