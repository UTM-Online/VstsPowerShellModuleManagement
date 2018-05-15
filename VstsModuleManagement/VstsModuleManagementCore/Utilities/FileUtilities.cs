namespace VstsModuleManagementCore.Utilities
{
    using System;
    using System.IO;
    using System.Linq;

    public static class FileUtilities
    {
        public static string DiscoverModuleBasePath()
        {
            var currentUser = Environment.GetEnvironmentVariable("USERNAME");

            var pathToTest = Environment.GetEnvironmentVariable("PSModulePath")
                                        .Split(';')
                                        .First(s => s.Contains(currentUser));

            string tempModulePath = null;

            if (!string.IsNullOrEmpty(pathToTest))
            {
                if (Directory.Exists(pathToTest))
                {
                    tempModulePath = $"{pathToTest}\\VstsModuleManagement";
                }
                else if (Directory.Exists($"{Environment.GetEnvironmentVariable("USERPROFILE")}\\Documents\\WindowsPowerShell\\Modules"))
                {
                    tempModulePath = $"{Environment.GetEnvironmentVariable("USERPROFILE")}\\Documents\\WindowsPowerShell\\Modules\\VstsModuleManagement";
                }
                else
                {
                    throw new InvalidOperationException("Unable to locate the directory hosting the modules files");
                }
            }
            else
            {
                if (Directory.Exists($"{Environment.GetEnvironmentVariable("USERPROFILE")}\\Documents\\WindowsPowerShell\\Modules"))
                {
                    tempModulePath = $"{Environment.GetEnvironmentVariable("USERPROFILE")}\\Documents\\WindowsPowerShell\\Modules\\VstsModuleManagement";
                }
                else
                {
                    throw new InvalidOperationException("Unable to locate the directory hosting the modules files");
                }
            }

            return tempModulePath;
        }
    }
}