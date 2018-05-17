namespace VstsModuleManagementCore.Cmdlets
{
    using System.Collections.Generic;
    using System.Management.Automation;

    using VstsModuleManagementCore.Models;
    using VstsModuleManagementCore.Resources;

    public abstract class AbstractBaseCmdlet : PSCmdlet
    {
        protected override sealed void BeginProcessing()
        {
            this.BeginProcessingCommand();
        }

        protected override sealed void ProcessRecord()
        {
            this.ProcessCommand();
        }

        protected override sealed void EndProcessing()
        {
            this.EndProcessingCommand();
        }

        protected virtual void BeginProcessingCommand()
        {

        }

        protected abstract void ProcessCommand();

        protected virtual void EndProcessingCommand()
        {

        }

        protected T GetPsVariable<T>(string name, object defaultValue = null)
        {
            return (T)this.GetVariableValue(name, defaultValue);
        }

        protected void SetPsVariable(string name, object value)
        {
            this.SessionState.PSVariable.Set(name, value);
        }

        protected Dictionary<string, object> CreateParameterDictionary(string key = null, string value = null)
        {
            if (string.IsNullOrEmpty(key))
            {
                return new Dictionary<string, object>();
            }
            else
            {
                return new Dictionary<string, object>
                           {
                               { key, value }
                           };
            }
        }

        protected ModuleSettings GetRunTimeModuleSettings()
        {
            return this.GetPsVariable<ModuleSettings>(ModuleVariables.ModuleSettings);
        }
    }
}