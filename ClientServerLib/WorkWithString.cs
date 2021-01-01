using System;
using System.Text.RegularExpressions;

namespace ClientServerLib
{
    public static class WorkWithString
    {
        static WorkWithString()
        {
            clientNameInMessages = new Regex("(?<clientName>.+[^:]).+");
        }
        static Regex clientNameInMessages;

        public static string GetClientName(string source)
        {
            Match clientNameFromSource = clientNameInMessages.Match(source);
            if (clientNameFromSource.Success)
            {
                return clientNameFromSource.Groups["clientName"].Value;
            }
            throw new ArgumentException();
        }
    }
}
