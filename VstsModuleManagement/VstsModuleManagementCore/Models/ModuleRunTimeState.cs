namespace VstsModuleManagementCore.Models
{
    using System.Collections.Generic;
    using System.IO;

    using Newtonsoft.Json;

    using VstsModuleManagementCore.Resources;
    using VstsModuleManagementCore.Utilities;

    public static class ModuleRunTimeState
    {
        static ModuleRunTimeState()
        {
            ModuleBasePath = FileUtilities.DiscoverModuleBasePath();
            ModuleDataDirectory = new DirectoryInfo(FileUtilities.DiscoverModuleDataPath());

            BlackListedCommands = JsonUtilities.GetBlackListedCommands();
            ModuleCredentialDirectory = new DirectoryInfo($"{ModuleDataDirectory.FullName}\\Credentials");
        }

        public static string ModuleBasePath { get; }

        internal static DirectoryInfo ModuleDataDirectory { get; }

        internal static DirectoryInfo ModuleCredentialDirectory { get; }

        public static bool CredentialsDirectoryExists => ModuleCredentialDirectory.Exists;

        public static string CredentialsDirectoryPath => ModuleCredentialDirectory.FullName;

        public static string ModuleDataPath => ModuleDataDirectory.FullName;

        public static bool ConfigDirectoryExsists => ModuleDataDirectory.Exists;

        internal static List<BlackListedCommandCollection.BlackListedCommand> BlackListedCommands { get; }
    }
}