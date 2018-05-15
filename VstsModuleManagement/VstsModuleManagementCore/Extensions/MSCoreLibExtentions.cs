namespace VstsModuleManagementCore.Extensions
{
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
    }
}