namespace VstsModuleManagementCore.Utilities
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using VstsModuleManagementCore.Extensions;

    internal static class FileUtilities
    {
        private static string PsModuleRootPath => Environment.GetEnvironmentVariable("PSModulePath").Split(';').First(s => s.Contains(CurrentUser));

        private static string CurrentUser => Environment.GetEnvironmentVariable("USERNAME");

        internal static string DiscoverModuleBasePath()
        {
            string modulePath;

            if (!string.IsNullOrEmpty(PsModuleRootPath))
            {
                if (Directory.Exists(PsModuleRootPath))
                {
                    modulePath = $"{PsModuleRootPath}\\VstsModuleManagement";
                }
                else if (Directory.Exists($"{Environment.GetEnvironmentVariable("USERPROFILE")}\\Documents\\WindowsPowerShell\\Modules"))
                {
                    modulePath = $"{Environment.GetEnvironmentVariable("USERPROFILE")}\\Documents\\WindowsPowerShell\\Modules\\VstsModuleManagement";
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
                    modulePath = $"{Environment.GetEnvironmentVariable("USERPROFILE")}\\Documents\\WindowsPowerShell\\Modules\\VstsModuleManagement";
                }
                else
                {
                    throw new InvalidOperationException("Unable to locate the directory hosting the modules files");
                }
            }

            return modulePath;
        }

        internal static string DiscoverModuleDataPath()
        {
            string moduleDataPath = null;

            const string configDirectoryName = "VstsModuleManagementData";

            if (string.IsNullOrEmpty(PsModuleRootPath)) return moduleDataPath;
            if (!Directory.Exists(PsModuleRootPath)) return moduleDataPath;
            var moduleDir = new DirectoryInfo(PsModuleRootPath);
            moduleDataPath = $"{moduleDir.Parent.FullName}\\{configDirectoryName}";

            return moduleDataPath;
        }

        internal static UnmanagedMemoryStream GetUnmanagedMemoryStream(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var resourceNames = assembly.GetManifestResourceNames();

            var resourceInternalName = resourceNames.SingleOrDefault(resource => resource.EndsWith(resourceName));

            if (resourceInternalName != null)
            {
                return (UnmanagedMemoryStream)assembly.GetManifestResourceStream(resourceInternalName);
            }

            throw new ArgumentException($"A resource with the name {resourceName} was not found in the modules assembly.")
            {
                Data = { {"AvaliableResources", resourceNames.ToCommaSeperatedList()}}
            };
        }
    }
}