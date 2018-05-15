namespace VstsModuleManagementCore.Models
{
    using System;
    using System.Collections.Generic;
    public class ParameterCollection : Dictionary<string, object>
    {
        public ParameterCollection()
        {
        }

        public ParameterCollection(string parameter, object value)
        {
            this.Add(parameter, value);
        }

        public ParameterCollection AddParameter(string parameter, object value)
        {
            this.Add(parameter, value);
            return this;
        }
    }
}