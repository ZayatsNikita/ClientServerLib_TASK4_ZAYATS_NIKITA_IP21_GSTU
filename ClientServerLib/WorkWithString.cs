using System;
using System.Text.RegularExpressions;

namespace ClientServerLib
{
    /// <summary>
    /// A class that is used to work with strings.
    /// </summary>
    public static class WorkWithString
    {
        static WorkWithString()
        {
            clientNameInMessages = new Regex("(?<clientName>.+?[:])");
        }
        static Regex clientNameInMessages;

        /// <summary>
        /// Method for getting the client name from messages
        /// </summary>
        /// <param name="source">String to be processed</param>
        /// <returns>The name of the client</returns>
        /// <exception cref="ArgumentException">Thrown if the string does not contain the client's name</exception>
        public static string GetClientName(string source)
        {
            Match clientNameFromSource = clientNameInMessages.Match(source);
            if (clientNameFromSource.Success)
            {
                return clientNameFromSource.Groups["clientName"].Value.TrimEnd(':');
            }
            throw new ArgumentException();
        }
    }
}
