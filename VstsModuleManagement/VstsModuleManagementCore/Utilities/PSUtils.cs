namespace VstsModuleManagementCore.Utilities
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Management.Automation;
    using System.Management.Automation.Runspaces;

    using VstsModuleManagementCore.Models;

    public static class PSUtils
    {
        public static PowerShell CreateShell()
        {
            return PowerShell.Create(RunspaceMode.CurrentRunspace);
        }

        public static void InvokePSCommand(string cmdletName, Dictionary<string, object> parameters = null, IEnumerable input = null)
        {
            using (var ps = CreateShell())
            {
                ps.AddCommand(cmdletName);

                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        ps.AddParameter(param.Key, param.Value);
                    }
                }

                if (input == null)
                {
                    ps.Invoke();
                }
                else
                {
                    ps.Invoke(input);
                }
            }
        }

        public static Collection<T> InvokePSCommand<T>(
            string cmdletName,
            Dictionary<string, object> parameters = null,
            IEnumerable input = null)
        {
            using (var ps = CreateShell())
            {
                ps.AddCommand(cmdletName);

                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        ps.AddParameter(param.Key, param.Value);
                    }
                }

                if (input == null)
                {
                    return ps.Invoke<T>();
                }
                else
                {
                    return ps.Invoke<T>(input);
                }
            }
        }

        public static PSCredential ImportCredential(string repositoryName)
        {
            using (var ps = CreateShell())
            {
                var config = ModuleSettings.LoadSettings();
                ps.AddCommand("Import-Clixml");
                ps.AddParameter("Path", $"{ModuleRunTimeState.CredentialsDirectoryPath}\\{config.KnownVstsProviders[repositoryName]}");
                return ps.Invoke<PSCredential>().First();
            }
        }
    }
}