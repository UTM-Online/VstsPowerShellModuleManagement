namespace VstsModuleManagementCore.Models.ReturnObjects
{
    public class PackageSource
    {
        public PackageSource(string name, string location)
        {
            this.Name = name;
            this.Location = location;
        }

        public string Name { get; set; }

        public string Location { get; set; }
    }
}