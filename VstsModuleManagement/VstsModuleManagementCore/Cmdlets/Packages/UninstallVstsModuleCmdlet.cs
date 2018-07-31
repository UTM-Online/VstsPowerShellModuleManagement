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

using System.Diagnostics.CodeAnalysis;

namespace VstsModuleManagementCore.Cmdlets.PrivateCommands
{
    using System;
    using System.Management.Automation;

    using VstsModuleManagementCore.Extensions;
    using VstsModuleManagementCore.Utilities;

    /// <inheritdoc />
    /// <summary>
    /// Class UninstallVstsModuleCmdlet.
    /// </summary>
    /// <seealso cref="T:VstsModuleManagementCore.Cmdlets.AbstractBaseCmdlet" />
    [Cmdlet(VerbsLifecycle.Uninstall, "VstsModule", DefaultParameterSetName = "All")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class UninstallVstsModuleCmdlet : AbstractBaseCmdlet
    {
        [Parameter(Mandatory = true, ParameterSetName = "All")]
        [Parameter(Mandatory = true, ParameterSetName = "Range")]
        [Parameter(Mandatory = true, ParameterSetName = "Single")]
        public string Name { get; set; }

        [Parameter(ParameterSetName = "All")]
        public SwitchParameter AllVersions { get; set; }

        [Parameter(ParameterSetName = "Range")]
        public Version MinimumVersion { get; set; }

        [Parameter(ParameterSetName = "Range")]
        public Version MaximumVersion { get; set; }

        [Parameter(ParameterSetName = "Single")]
        public Version RequiredVersion { get; set; }

        private int DeterminedCodePath;

        protected override bool CredentialsRequired => false;

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
                        // ReSharper disable once HeapView.BoxingAllocation
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
                        param["Name"] = "Uninstall-VstsModule";
                        this.WriteObject(PSUtils.InvokePSCommand<object>("Get-Help", param));
                        break;
                    }
            }
        }
    }
}