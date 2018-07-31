using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Management.Automation;

namespace VstsModuleManagementCore.Cmdlets
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public abstract class AbstractRepositoryCmdlet : AbstractBaseCmdlet, IDynamicParameters
    {
        protected string Repository { get; set; }
        
        protected virtual Collection<Attribute> RepositoryParameterOverride { get; set; }

        protected sealed override void BeginProcessingCommand()
        {
            this.Repository = (string)this.GetPsVariable<RuntimeDefinedParameterDictionary>("PSBoundParameters")["Repository"].Value ?? string.Empty;
            this.BeginProcessingCmdlet();
            base.BeginProcessingCommand();
        }

        protected virtual void BeginProcessingCmdlet()
        {
        }

        public object GetDynamicParameters()
        {
            var moduleSettings = this.GetRunTimeModuleSettings();

            var vstsProviders = moduleSettings.KnownVstsProviders.Count > 0 ? moduleSettings.KnownVstsProviders : new Dictionary<string, string>();

            var runtimeParameterDictionary = new RuntimeDefinedParameterDictionary();

            if (this.RepositoryParameterOverride.Any())
            {
                this.RepositoryParameterOverride.Add(new ValidateSetAttribute(vstsProviders.Keys.ToArray()));
                
                runtimeParameterDictionary.Add("Repository", new RuntimeDefinedParameter("Repository", typeof(string),
                    new Collection<Attribute>(this.RepositoryParameterOverride)));
            }
            else
            {
                runtimeParameterDictionary.Add("Repository", new RuntimeDefinedParameter("Repository", typeof(string),
                    new Collection<Attribute>
                    {
                        new ParameterAttribute(),
                        new ValidateSetAttribute(vstsProviders.Keys.ToArray())
                    }));
            }

            return runtimeParameterDictionary;
        }
    }
}