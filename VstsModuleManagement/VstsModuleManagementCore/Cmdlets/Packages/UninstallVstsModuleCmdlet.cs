// ***********************************************************************
// Assembly         : VstsModuleManagementCore
// Author           : joirwi
// Created          : 5/17/2018 7:00:02 PM
//
// Last Modified By : joirwi
// Last Modified On : 5/17/2018 7:00:02 PM
// ***********************************************************************
// <copyright file="UninstallVstsModuleCmdlet.cs" company="Microsoft">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// **********************************************************************

namespace VstsModuleManagementCore.Cmdlets.PrivateCommands
{
    using System;
    using System.Management.Automation;

    using VstsModuleManagementCore.Extensions;
    using VstsModuleManagementCore.Utilities;

    /// <summary>
    /// Class UninstallVstsModuleCmdlet.
    /// </summary>
    /// <seealso cref="AbstractBaseCmdlet" />
    [Cmdlet(VerbsLifecycle.Uninstall, "VstsModule")]
    public class UninstallVstsModuleCmdlet : AbstractBaseCmdlet
    {
        [Parameter(Mandatory = true)]
        public string Name { get; set; }

        [Parameter]
        public SwitchParameter AllVersions { get; set; }

        [Parameter]
        public Version MinimumVersion { get; set; }

        [Parameter]
        public Version MaximumVersion { get; set; }

        [Parameter]
        public Version RequiredVersion { get; set; }

        private int DeterminedCodePath;

        protected override void BeginProcessingCommand()
        {
            if (this.AllVersions)
            {
                this.DeterminedCodePath = 1;
            }
            else if (this.RequiredVersion != null)
            {
                this.DeterminedCodePath = 2;
            }
            else if (this.MinimumVersion != null && this.MaximumVersion != null)
            {
                this.DeterminedCodePath = 3;
            }
        }

        /// <summary>
        /// Processes the command.
        /// </summary>
        protected override void ProcessCommand()
        {
            var param = this.CreateParameterDictionary("Name", this.Name);

            switch (this.DeterminedCodePath)
            {
                case 1:

                    {
                        param.Add("AllVersions", this.AllVersions);
                        PSUtils.InvokePSCommand("Uninstall-Module", param);
                        break;
                    }

                case 2:

                    {
                        param.Add("RequiredVersion", this.RequiredVersion);
                        PSUtils.InvokePSCommand("Uninstall-Module", param);
                        break;
                    }

                case 3:

                    {
                        param.AddParameter("MinimumVersion", this.MinimumVersion)
                             .AddParameter("MaximumVersion", this.MaximumVersion);
                        PSUtils.InvokePSCommand("Uninstall-Module", param);
                        break;
                    }

                default:

                    {
                        this.WriteInformation("Your input is not understood.  Please see the help section on this cmdlet below.", null);
                        param.Add("Name", "Uninstall-VstsModule");
                        this.WriteObject(PSUtils.InvokePSCommand<object>("Get-Help", param));
                        break;
                    }
            }
        }
    }
}