namespace VstsModuleManagementCore.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Security;

    public static class MSCoreLibExtentions
    {
        public static SecureString ConvertToSecureString(this string inputString)
        {
            var charArray = inputString.ToCharArray();

            SecureString outputSecureString = new SecureString();

            foreach (var a in charArray)
            {
                outputSecureString.AppendChar(a);
            }

            outputSecureString.MakeReadOnly();

            return outputSecureString;
        }

        public static Dictionary<string, object> AddParameter(
            this Dictionary<string, object> parameters,
            string key,
            object value)
        {
            parameters.Add(key, value);
            return parameters;
        }
    }
}