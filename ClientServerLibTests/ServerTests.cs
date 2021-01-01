using ClientServerLib;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ClientServerLibTests
{

    [TestFixture]
    class ServerTests
    {
        public static string localIp = "127.0.0.1";
        public static int correctServerPort = 8001;
        [Test]
        public void TestOfDataExchangeBetweenTheServerAndTheClient_CorrectMessage_MessageFromServerInClient()
        {
            List<string> vs = new List<string>();
            List<string> list2 = new List<string>();
            Client client = new Client("Valera", correctServerPort, localIp, "Я пришёл с миром", (str)=> { vs.Add(str); }, null);
            Server server = new Server(correctServerPort, localIp, "привет", null, null);
            Thread serverThread = new Thread(new ThreadStart(() => { server.Start(300); }));
            serverThread.Start();
            Thread.Sleep(3000);
            client.ConnectToTheServer();
            client.ExchangeDataWithTheServer();

            bool actual = vs.Count == 1 && vs[0] == "привет";
            Assert.IsTrue(actual);

        }




    [Test]
        public void AddingShowMessageHandlersTests_SubscribedToSeveralMethodsUsingAnonimusMethodsAndLambdaExpressions_RecivedMessageWillBePlaceToListAndRepository()
        {
            MessageRepository repository = new MessageRepository();
            List<string> list2 = new List<string>();
            Client client = new Client("Valera", correctServerPort, localIp, "Я пришёл с миром", null, null);
            Server server = new Server(correctServerPort, localIp, "привет", null, null);
            Thread serverThread = new Thread(new ThreadStart(() => { server.Start(300); }));
            serverThread.Start();
            Thread.Sleep(3000);

            client.ConnectToTheServer();

            client.ExchangeDataWithTheServer();
            bool actual = repository.Count == 0 && list2.Count == 0;

            server.Show += delegate (string str) {
                string clientName = WorkWithString.GetClientName(str);
                repository.Add(clientName, str.Remove(0, clientName.Length + 2));
            };
            client.ExchangeDataWithTheServer();

            list2 = repository.GetMessageHistoryOf("Valera");

            actual = actual && list2.Count == 1 && list2[0] == "Я пришёл с миром" && repository.Count == 1;

            client.MessageForSendingDataToTheServer = "сообщение номер 2";

            list2 = repository.GetMessageHistoryOf("Valera");

            actual = actual && list2.Count == 1 && list2[0] == "Я пришёл с миром" && repository.Count == 1;

            server.Show += (str) => {
                string clientName = WorkWithString.GetClientName(str);
                repository.Add(clientName, str.Remove(0, clientName.Length + 2));
            };
            client.MessageForSendingDataToTheServer = "сообщение номер 3";
            client.ExchangeDataWithTheServer();

            list2 = repository.GetMessageHistoryOf("Valera");

            actual = actual && list2.Count == 3 && list2[0] == "Я пришёл с миром" && list2[1] == "сообщение номер 3" && list2[2] == "сообщение номер 3" && repository.Count == 1;

            Assert.IsTrue(actual);
        }
        [Test]
        public void AddingServerStringProcessingHandlersTests_SubscribedToSeveralMethodsUsingAnonimusMethodsAndLambdaExpressions_RecivedMessageWillBePlaceToListAndRepository()
        {
            MessageRepository repository = new MessageRepository();
            List<string> list2 = new List<string>();
            Client client = new Client("Valera", correctServerPort, localIp, "Я пришёл с миром", null, null);
            Server server = new Server(correctServerPort, localIp, "привет", null, null);
            Thread serverThread = new Thread(new ThreadStart(() => { server.Start(300); }));
            serverThread.Start();
            Thread.Sleep(3000);

            client.ConnectToTheServer();

            client.ExchangeDataWithTheServer();
            bool actual = repository.Count == 0 && list2.Count == 0;

            server.ServerStringProcessing += delegate (string str) {
                string clientName = WorkWithString.GetClientName(str);
                repository.Add(clientName, str.Remove(0, clientName.Length + 2));
            };
            client.ExchangeDataWithTheServer();

            list2 = repository.GetMessageHistoryOf("Valera");

            actual = actual && list2.Count == 1 && list2[0] == "Я пришёл с миром" && repository.Count == 1;

            client.MessageForSendingDataToTheServer = "сообщение номер 2";

            list2 = repository.GetMessageHistoryOf("Valera");

            actual = actual && list2.Count == 1 && list2[0] == "Я пришёл с миром" && repository.Count == 1;

            server.ServerStringProcessing += (str) => {
                string clientName = WorkWithString.GetClientName(str);
                repository.Add(clientName, str.Remove(0, clientName.Length + 2));
            };
            client.MessageForSendingDataToTheServer = "сообщение номер 3";
            client.ExchangeDataWithTheServer();

            list2 = repository.GetMessageHistoryOf("Valera");

            actual = actual && list2.Count == 3 && list2[0] == "Я пришёл с миром" && list2[1] == "сообщение номер 3" && list2[2] == "сообщение номер 3" && repository.Count == 1;

            Assert.IsTrue(actual);
        }
        [Test]
        public void SetRepliesForMessages_NullIsSetAsRepLaceMessage_NullReferenceExceptionThrown()
        {
            bool actual = false;
            try
            {
                Server server = new Server(correctServerPort, localIp, null, null, null);
            }
            catch (NullReferenceException)
            {
                actual = true;
            }
            Assert.IsTrue(actual);
        }
        [TestCase(1023)]
        [TestCase(65537)]
        public void CreateServertTest_SetWrongPort_ArgumentException(int port)
        {
            bool actual = false;
            try
            {
                Server server = new Server(port, localIp, "hello", null, null);

            }
            catch (ArgumentException)
            {
                actual = true;
            }
            Assert.IsTrue(actual);
        }
        [TestCase(0)]
        [TestCase(-3)]
        public void StartTest_IncorrectOperationTime_ArgumentException(int expected)
        {
            bool actual = false;
            Server server = new Server(correctServerPort, localIp, "hello", null, null);

            try
            {
                server.Start(expected);
            }
            catch (ArgumentException)
            {
                actual = true;
            }
            Assert.IsTrue(actual);
        }
    }
}
