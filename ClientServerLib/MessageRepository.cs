using System;
using System.Collections.Generic;
namespace ClientServerLib
{
    /// <summary>
    /// A class that represents a string store.
    /// </summary>
    public class MessageRepository
    {
        public MessageRepository()
        {
            repository = new Dictionary<string, List<string>>();
        }
        Dictionary<string, List<string>> repository;

        public void Add(string clientName, string message)
        {
            if (!repository.ContainsKey(clientName))
            {
                repository.Add(clientName, new List<string>());
            }
            repository[clientName].Add(message);
        }

        public List<string> GetMessageHistoryOf(string clientName)
        {
            if (!repository.ContainsKey(clientName))
            {
                throw new ArgumentException();
            }
            else
            {
                return repository.GetValueOrDefault(clientName);
            }
        }

        public int Count { get => repository.Count; }
    }
}
