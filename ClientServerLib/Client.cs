using System;
using System.Net.Sockets;
using System.Net;

namespace ClientServerLib
{
    public class Client : InternetObject
    {
        private Socket _innerSocket;

        private IPEndPoint _ipPoint;

        private string _clientName;

        private string _messageForSendingDataToTheServer;

        private event ProcessingStringOnClient _clientStringProcessing;

        private event ShowData _show;

        private bool _sendingMessagesIsProhibited { get; set; } = false;


        public event ProcessingStringOnClient ClientStringProcessing
        {
            add
            {
                _clientStringProcessing += value;
            }
            remove
            {
                _clientStringProcessing -= value;
            }
        }

        public event ShowData Show
        {
            add
            {
                _show += value;
            }
            remove
            {
                _show -= value;
            }
        }

        public string MessageForSendingDataToTheServer
        {
            get => _messageForSendingDataToTheServer;
            set
            {
                if (value == null)
                {
                    throw new NullReferenceException();
                }
                _messageForSendingDataToTheServer = value;
            }
        }

        public string ClientName
        {
            get => _clientName;
            set
            {
                if (value == null)
                {
                    throw new NullReferenceException();
                }
                if (value.Length == 0)
                {
                    throw new ArgumentException();
                }
                _clientName = value;
            }
        }

        public Client(string clientName, int serverPort, string serverIpPoint, string messageForSendingDataToTheServer, ShowData show, ProcessingStringOnClient clientStringProcessing)
        {
            if (serverPort < 1024 || serverPort > 65536)
            {
                throw new ArgumentException();
            }

            ClientName = clientName;

            MessageForSendingDataToTheServer = messageForSendingDataToTheServer;

            _ipPoint = new IPEndPoint(IPAddress.Parse(serverIpPoint), serverPort);

            _innerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            ClientStringProcessing += clientStringProcessing;

            Show += show;
        }


        public void ConnectToTheServer()
        {
            if (_innerSocket.Connected)
            {
                throw new InvalidOperationException("The client is already connected to the server");
            }
            _innerSocket.Connect(_ipPoint);



        }


        public void ExchangeDataWithTheServer()
        {
            if (!_sendingMessagesIsProhibited && _innerSocket.Connected)
            {
                try
                {
                    string messageFromServer;
                    {

                        SendMessage(_innerSocket, $"{ClientName}: {MessageForSendingDataToTheServer}");

                        messageFromServer = GetMessage(_innerSocket);

                        _clientStringProcessing?.Invoke(messageFromServer);

                        _show?.Invoke(messageFromServer);

                    }
                }
                catch (Exception ex)
                {
                    DisconnectFromTheServer();
                    throw ex;
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public void DisconnectFromTheServer()
        {
            if (!_sendingMessagesIsProhibited)
            {
                _sendingMessagesIsProhibited = true;
                SendMessage(_innerSocket, "final message: EXIT");
                _innerSocket.Shutdown(SocketShutdown.Both);
                if (_innerSocket.Connected)
                {
                    _innerSocket.Close();
                }
            }
        }

        ~Client()
        {
            if (_innerSocket.Connected)
            {
                DisconnectFromTheServer();
            }
        }

    }
}
