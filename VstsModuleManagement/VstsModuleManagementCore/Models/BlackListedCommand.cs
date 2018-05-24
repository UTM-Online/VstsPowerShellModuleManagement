namespace VstsModuleManagementCore.Models
{
    using System.Collections.Generic;

    public class BlackListedCommandCollection
    {
        public List<BlackListedCommand> BlackListedCommands { get; set; }

        public class BlackListedCommand
        {
            public string Name { get; set; }

            public string Reason { get; set; }
        }
    }
}