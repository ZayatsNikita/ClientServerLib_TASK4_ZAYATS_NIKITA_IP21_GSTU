using System;
using System.Net.Sockets;
using System.Net;


namespace ClientServerLib
{
    /// <summary>
    /// A class that describes the client.
    /// </summary>
    public class Client : InternetObject
    {
        private Socket _innerSocket;

        private IPEndPoint _ipPoint;

        private string _clientName;

        private string _messageForSendingDataToTheServer;

        private event ProcessingStringOnClient _clientStringProcessing;

        private event ShowData _show;

        /// <summary>
        /// Property describing the ability to send messages by the client.
        /// </summary>
        private bool _sendingMessagesIsProhibited { get; set; } = false;


        /// <summary>
        /// Subscribe and unsubscribe from an event ClientStringProcessing.
        /// </summary>
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
        /// <summary>
        /// Subscribe and unsubscribe from an event Show.
        /// </summary>
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

        /// <summary>
        /// Property describing the message that the client sends to the server
        /// </summary>
        /// <exception cref="NullReferenceException">Thrown if you specify zero as the message.</exception>
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
        /// <summary>
        /// Property describing the name of client
        /// </summary>
        /// <exception cref="NullReferenceException">Thrown if you specify zro as the name</exception>
        /// <exception cref="ArgumentException">Thrown if you specify zro length string as the name</exception>
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
        /// <summary>
        /// Constructor with parameters for creating a client.
        /// </summary>
        /// <param name="clientName">The name of the object.</param>
        /// <param name="serverPort">port of the server you will be connecting to.</param>
        /// <param name="serverIpPoint">ip address of the server you will be connecting to.</param>
        /// <param name="messageForSendingDataToTheServer">Message to send to the server.</param>
        /// <param name="show">Handlers for the event that will occur when a message arrives.</param>
        /// <param name="clientStringProcessing">Handlers for the event that will occur when a message arrives.</param>
        /// <exception cref="ArgumentException">Thrown if you specify value of port greater then 65536 or less then 1024</exception>
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

        /// <summary>
        /// A method that performs the connection to the server.<para></para> Need to complete before sending messages.
        /// </summary>
        /// <remarks>If a connection error occurs, you will not be able to send messages.</remarks>
        ///<exception cref="SocketException"></exception>
        ///<exception cref="ObjectDisposedException"></exception>
        ///<exception cref="System.Security.SecurityException"></exception>
        ///<exception cref="InvalidOperationException">Thrown when trying to reconnect to the server.</exception>
        public void ConnectToTheServer()
        {
            if (_innerSocket.Connected)
            {
                throw new InvalidOperationException("The client is already connected to the server");
            }
            try
            {
                _innerSocket.Connect(_ipPoint);
            }
            catch(Exception ex)
            {
                _sendingMessagesIsProhibited = true;
                throw ex;
            }
        }

        /// <summary>
        /// A method that performs data exchange with the server<para></para>(sends a message to the server and calls an event responsible for processing the incoming message).
        /// </summary>
        /// <exception cref="">It is thrown out if you are disconnected from the server or sending messages is not possible.</exception>
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

        /// <summary>
        /// A method that disconnects from the server and makes sending messages impossible.
        /// </summary>
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

        /// <summary>
        /// Destructor of the client class
        /// </summary>
        ~Client()
        {
            if (_innerSocket.Connected)
            {
                DisconnectFromTheServer();
            }
        }

    }
}
