using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Sockets;
using ClientServerLib;
using System.Threading;
using System;

namespace ClientServerTests
{
    public class ClientTests
    {
        public static string localIp = "127.0.0.1";
        public static int correctServerPort = 8001;

        [Test]
        public void ConnectToTheServerTest_AttempToDoubleConnectToTheServer_InvalidOperationExceptionThrown()
        {

            Client client = new Client("Valera", correctServerPort, localIp, "Я пришёл с миром", null, null);
            Server server = new Server(correctServerPort, localIp, "dfsgdf", null, null);
            Thread serverThread = new Thread(new ThreadStart(() => { server.Start(300); }));
            serverThread.Start();
            Thread.Sleep(5000);
            client.ConnectToTheServer();
            bool actual = false;
            try
            {
                client.ConnectToTheServer();
            }
            catch (InvalidOperationException)
            {
                actual = true;
            }
            Assert.IsTrue(actual);
        }

        [Test]
        public void ExchangeDataWithTheServerTest_AttemptToSendMessageToTheServerAfterDisconnectFromTheServer_InvalidOperationExceptionThrown()
        {
            Client client = new Client("Valera", correctServerPort, localIp, "Я пришёл с миром", null, null);
            Server server = new Server(correctServerPort, localIp, "dfsgdf", null, null);
            Thread serverThread = new Thread(new ThreadStart(() => { server.Start(3000); }));
            serverThread.Start();
            Thread.Sleep(5000);
            client.ConnectToTheServer();
            client.DisconnectFromTheServer();
            bool actual = false;
            try
            {
                client.ExchangeDataWithTheServer();
            }
            catch (InvalidOperationException)
            {
                actual = true;
            }
            Assert.IsTrue(actual);
        }

        [Test]
        public void ExchangeDataWithTheServerTest_AttemptToSendMessageToTheServerWithOutConnectionToTheServer_InvalidOperationExceptionThrown()
        {
            Client client = new Client("Valera", correctServerPort, localIp, "Я пришёл с миром", null, null);
            Server server = new Server(correctServerPort, localIp, "dfsgdf", null, null);
            Thread serverThread = new Thread(new ThreadStart(() => { server.Start(300); }));
            serverThread.Start();

            bool actual = false;
            try
            {
                client.ExchangeDataWithTheServer();
            }
            catch (InvalidOperationException)
            {
                actual = true;
            }
            Assert.IsTrue(actual);
        }

        [TestCase("Привет,привет", "Privet,privet")]
        [TestCase("покА", "pokA")]
        [TestCase("Всё понял. Остовайтесь на связи, i'll be back", "Vse ponyal. Ostovajtes na svyazi, i'll be back")]
        public void ClientExchangeDataWithTheServerTest_RecivedMessagesIsProcessedInClientAndConvertedMessageIsSendToLisForProcessingIsUsedLambdaExpression_CorrectMessageFromTheServerWillBePlacedInTheList(string retMessage, string expected)
        {
            List<string> list = new List<string>();
            Client client = new Client("Valera", correctServerPort, localIp, "Я пришёл с миром", null, (str) => { list.Add(MessageConverter.ConvertRussianToTranslit(str)); });
            Server server = new Server(correctServerPort, localIp, retMessage, null, null);
            Thread serverThread = new Thread(new ThreadStart(() => { server.Start(300); }));
            serverThread.Start();
            Thread.Sleep(5000);
            client.ConnectToTheServer();
            client.ExchangeDataWithTheServer();

            string actual = list[0];
            Assert.AreEqual(expected, actual);
        }

        [TestCase("Привет,привет", "Privet,privet")]
        [TestCase("покА", "pokA")]
        [TestCase("Всё понял. Остовайтесь на связи, i'll be back", "Vse ponyal. Ostovajtes na svyazi, i'll be back")]
        public void ClientExchangeDataWithTheServerTest_RecivedMessagesIsProcessedInClientAndConvertedMessageIsSendToListForProcessingIsUsedAnonimusMetods_CorrectMessageFromTheServerWillBePlacedInTheList(string retMessage, string expected)
        {
            List<string> list = new List<string>();
            Client client = new Client("Valera", correctServerPort, localIp, "Я пришёл с миром", null, delegate (string str) { list.Add(MessageConverter.ConvertRussianToTranslit(str)); });
            Server server = new Server(correctServerPort, localIp, retMessage, null, null);
            Thread serverThread = new Thread(new ThreadStart(() => { server.Start(300); }));
            serverThread.Start();
            Thread.Sleep(5000);
            client.ConnectToTheServer();
            client.ExchangeDataWithTheServer();

            string actual = list[0];
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AddingShowMessageHandlersTests_SubscribedToSeveralMethodsUsingAnonimusMethodsAndLambdaExpressions_RecivedMessageWillBePlaceToList()
        {
            List<string> list = new List<string>();
            List<string> list2 = new List<string>();
            Client client = new Client("Valera", correctServerPort, localIp, "Я пришёл с миром", null, null);
            Server server = new Server(correctServerPort, localIp, "привет", null, null);
            Thread serverThread = new Thread(new ThreadStart(() => { server.Start(300); }));
            serverThread.Start();
            Thread.Sleep(5000);
            client.ConnectToTheServer();
            client.ExchangeDataWithTheServer();
            bool actual = list.Count == 0 && list2.Count == 0;

            client.Show += delegate (string str) { list.Add(str); };
            client.ExchangeDataWithTheServer();

            actual = actual && list.Count == 1 && list[0] == "привет";

            client.Show += (str) => { list2.Add(str); };
            client.ExchangeDataWithTheServer();

            actual = actual && list2.Count == 1 && list2[0] == "привет" && list.Count == 2 && list[1] == "привет";

            Assert.IsTrue(actual);
        }

        [Test]
        public void AddingProcessingStringOnClientHandlersTests_SubscribedToSeveralMethodsUsingAnonimusMethodsAndLambdaExpression_RecivedMessageWillBePlaceToList()
        {
            List<string> list = new List<string>();
            List<string> list2 = new List<string>();
            Client client = new Client("Valera", correctServerPort, localIp, "Я пришёл с миром", null, null);
            Server server = new Server(correctServerPort, localIp, "привет", null, null);
            Thread serverThread = new Thread(new ThreadStart(() => { server.Start(300); }));
            serverThread.Start();
            Thread.Sleep(5000);
            client.ConnectToTheServer();
            client.ExchangeDataWithTheServer();
            bool actual = list.Count == 0 && list2.Count == 0;

            client.ClientStringProcessing += delegate (string str) { list.Add(str); };
            client.ExchangeDataWithTheServer();

            actual = actual && list.Count == 1 && list[0] == "привет";

            client.ClientStringProcessing += (str) => { list2.Add(str); };
            client.ExchangeDataWithTheServer();

            actual = actual && list2.Count == 1 && list2[0] == "привет" && list.Count == 2 && list[1] == "привет";

            Assert.IsTrue(actual);
        }




        [TestCase(3001)]
        [TestCase(8000)]
        [TestCase(15000)]
        public void ClientExchangeDataWithTheServerTest_TheServerIsRunningOnDifferentPort_SocketExceptionThrown(int port)
        {
            List<string> list = new List<string>();
            bool actual = false;

            Client client = new Client("Valera", port, localIp, "Я пришёл с миром", null, (str) => { list.Add(MessageConverter.ConvertRussianToTranslit(str)); });
            Server server = new Server(port + 1, localIp, "Всё понял. Остовайтесь на связи, i'll be back", null, null);
            Thread serverThread = new Thread(new ThreadStart(() => { server.Start(3000); }));
            serverThread.Start();

            try
            {
                client.ConnectToTheServer();
                client.ExchangeDataWithTheServer();
            }
            catch (SocketException)
            {
                actual = true;
            }

            Assert.IsTrue(actual);
        }
        [Test]
        public void SetClientNameTest_NullIsSetAsTheName_NullReferenceExceptionThrown()
        {
            bool actual = false;
            try
            {
                Client client = new Client(null, 56000, localIp, "Я пришёл с миром", null, null);

            }
            catch (NullReferenceException)
            {
                actual = true;
            }
            Assert.IsTrue(actual);
        }
        [Test]
        public void SetClientNameTest_EmptyStringIsSetAsTheName_ANullStringIsSetAsTheName_NullReferenceExceptionThrown()
        {
            bool actual = false;
            try
            {
                Client client = new Client("", 56000, localIp, "Я пришёл с миром", null, null);

            }
            catch (ArgumentException)
            {
                actual = true;
            }
            Assert.IsTrue(actual);
        }
        [Test]
        public void SetMessageForSendingTest_NullIsSetAsMessageForSending_NullReferenceExceptionThrown()
        {
            bool actual = false;
            try
            {
                Client client = new Client("sdfsfd", 56000, localIp, null, null, null);

            }
            catch (NullReferenceException)
            {
                actual = true;
            }
            Assert.IsTrue(actual);
        }
        [TestCase(1023)]
        [TestCase(65537)]
        public void CreateClientTest_SetWrongPort_ArgumentException(int port)
        {
            bool actual = false;
            try
            {
                Client client = new Client("sdfsfd", port, localIp, null, null, null);

            }
            catch (ArgumentException)
            {
                actual = true;
            }
            Assert.IsTrue(actual);
        }


    }
}