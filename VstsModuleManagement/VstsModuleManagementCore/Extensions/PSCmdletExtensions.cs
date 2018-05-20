namespace VstsModuleManagementCore.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Management.Automation;

    using VstsModuleManagementCore.Models;
    using VstsModuleManagementCore.Resources;
    using VstsModuleManagementCore.Utilities;

    public static class PSCmdletExtensions
    {
        public static ModuleSettings GetRunTimeModuleSettings(this PSCmdlet cmdlet)
        {
            return cmdlet.GetPsVariable<ModuleSettings>(ModuleVariables.ModuleSettings);
        }

        public static Dictionary<string, object> CreateParameterDictionary(
            this PSCmdlet cmdlet,
            string key = null,
            object value = null)
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

        public static T GetValue<T>(this Dictionary<string, object> data, string key)
        {
            return (T)data[key];
        }

        public static T GetPsVariable<T>(this PSCmdlet cmdlet, string name, object defaultValue = null)
        {
            return (T)cmdlet.GetVariableValue(name, defaultValue);
        }

        public static void SetPsVariable(this PSCmdlet cmdlet, string name, object value)
        {
            cmdlet.SessionState.PSVariable.Set(name, value);
        }
    }
}