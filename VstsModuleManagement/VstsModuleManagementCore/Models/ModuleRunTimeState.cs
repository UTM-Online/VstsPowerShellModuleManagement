namespace VstsModuleManagementCore.Models
{
    using VstsModuleManagementCore.Utilities;

    public static class ModuleRunTimeState
    {
        static ModuleRunTimeState()
        {
            ModuleBasePath = FileUtilities.DiscoverModuleBasePath();
        }

        public static string ModuleBasePath { get; }
    }
}