// ***********************************************************************
// Assembly         : VstsModuleManagementCore
// Author           : joirwi
// Created          : 5/17/2018 7:01:19 PM
//
// Last Modified By : joirwi
// Last Modified On : 5/17/2018 7:01:19 PM
// ***********************************************************************
// <copyright file="Install-VstsModuleCmdlet.cs" company="Microsoft">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// **********************************************************************

namespace VstsModuleManagementCore.Cmdlets.Packages
{
    using System;
    using System.Collections.Generic;
    using System.Management.Automation;
    using System.Security.Cryptography.X509Certificates;

    using VstsModuleManagementCore.Extensions;
    using VstsModuleManagementCore.Utilities;

    /// <summary>
    /// Class InstallVstsModuleCmdlet.
    /// </summary>
    /// <seealso cref="AbstractBaseCmdlet" />
    [Cmdlet(VerbsLifecycle.Install, "VstsModule")]
    public class InstallVstsModuleCmdlet : AbstractBaseCmdlet
    {
        [Parameter(ParameterSetName = "Default")]
        [Parameter(ParameterSetName = "VersionRange")]
        public string Name { get; set; }

        [Parameter(ParameterSetName = "Default")]
        [Parameter(ParameterSetName = "VersionRange")]
        public SwitchParameter AllowClobber { get; set; }

        [Parameter(ParameterSetName = "VersionRange")]
        public Version MinimumVersion { get; set; }

        [Parameter(ParameterSetName = "VersionRange")]
        public Version MaximumVersion { get; set; }

        [Parameter(ParameterSetName = "Default")]
        [Parameter(ParameterSetName = "VersionRange")]
        public string Repository { get; set; }

        [Parameter(ParameterSetName = "Default")]
        public Version RequiredVersion { get; set; }

        [Parameter(ParameterSetName = "Default")]
        [Parameter(ParameterSetName = "VersionRange")]
        [ValidateSet("CurrentUser", "AllUsers")]
        public string Scope { get; set; }

        [Parameter(ParameterSetName = "Default")]
        [Parameter(ParameterSetName = "VersionRange")]
        public SwitchParameter SkipPublisherCheck { get; set; }

        [Parameter(ParameterSetName = "Default")]
        [Parameter(ParameterSetName = "VersionRange")]
        public SwitchParameter Confirm { get; set; }

        [Parameter(ParameterSetName = "Default")]
        [Parameter(ParameterSetName = "VersionRange")]
        public SwitchParameter Force { get; set; }

        protected override bool CredentialsRequired => true;

        protected override void BeginProcessingCommand()
        {
            var data = this.CreateParameterDictionary("Name", this.Name).AddParameter("Repository", this.Repository).AddParameter("Credential", this.Creds);

            if (this.AllowClobber)
            {
                data.Add("AllowClobber", this.AllowClobber);
            }

            if (!string.IsNullOrEmpty(this.Scope))
            {
                data.Add("Scope", this.Scope);
            }

            if (this.SkipPublisherCheck)
            {
                data.Add("SkipPublisherCheck", this.SkipPublisherCheck);
            }

            if (this.Confirm && !this.Force)
            {
                data.Add("Confirm", this.Confirm);
            }
            else if (this.Force && !this.Confirm)
            {
                data.Add("Force", this.Force);
            }
            else if (this.Confirm && this.Force)
            {
                this.WriteWarning("Both the \"Confirm\" and the \"Force\" switches were used together and both will be ignored.");
            }

            this.PrivateCmdletData.Add("Parameters", data);
        }

        /// <summary>
        /// Processes the command.
        /// </summary>
        protected override void ProcessCommand()
        {
            var data = this.PrivateCmdletData.GetValue<Dictionary<string, object>>("Parameters");

            switch (this.ParameterSetName)
            {
                case "Default":

                    {
                        data.Add("RequiredVersion", this.RequiredVersion);
                        break;
                    }

                case "VersionRange":

                    {
                        data.AddParameter("MinimumVersion", this.MinimumVersion)
                            .AddParameter("MaximumVersion", this.MaximumVersion);
                        break;
                    }

                default:

                    {
                        this.WriteObject("Your command was not understood.");
                        this.StopProcessing();
                        break;
                    }
            }

            PSUtils.InvokePSCommand("Install-Module", data);
        }
    }
}