using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ClientServerLib
{
    /// <summary>
    /// A class that describes the server.
    /// </summary>
    public class Server : InternetObject
    {
        private event ProcessingStringOnServer _serverStringProcessing;

        /// <summary>
        /// Subscribe and unsubscribe from an event ServerStringProcessing.
        /// </summary>
        public event ProcessingStringOnServer ServerStringProcessing
        {
            add
            {
                _serverStringProcessing += value;
            }
            remove
            {
                _serverStringProcessing -= value;
            }
        }

        private event ShowData _show;
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

        private IPEndPoint ipPoint;

        private string _repliesToMessages;
        /// <summary>
        /// Properties that present message that is send by server to client.
        /// </summary>
        /// <exception cref="NullReferenceException">Thrown if you specify null reference as replace message.</exception>
        public string RepliesForMessages
        {
            get => _repliesToMessages;
            set
            {
                if (value == null)
                {
                    throw new NullReferenceException();
                }
                _repliesToMessages = value;
            }

        }


        private Socket _listenSocket;
        /// <summary>
        /// Constructor for creating a server.
        /// </summary>
        /// <param name="port">The port that the server will listen on.</param>
        /// <param name="ipPoint">The ip-address that the server will listen on</param>
        /// <param name="rerliesToMessages">Message that is send by server to client</param>
        /// <param name="show">Handlers for the event that will occur when a message arrives</param>
        /// <param name="processingStringOnServer">Handlers for the event that will occur when a message arrives</param>
        /// <exception cref="ArgumentException">Thrown if you specify value of port greater then 65536 or less then 1024</exception>
        public Server(int port, string ipPoint, string rerliesToMessages, ShowData show, ProcessingStringOnServer processingStringOnServer)
        {

            if (port < 1024 || port > 65536)
            {
                throw new ArgumentException();
            }

            RepliesForMessages = rerliesToMessages;

            this.ipPoint = new IPEndPoint(IPAddress.Parse(ipPoint), port);

            _listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            ServerStringProcessing += processingStringOnServer;

            Show += show;
        }
        /// <summary>
        /// Method that starts the server for the specified amount of time.
        /// </summary>
        /// <param name="workTimeInSecond">Server running time in seconds.</param>
        /// <exception cref="ArgumentException">Thrown if you set value that less that one as work time.</exception>
        ///<remarks>The specified time is used only if clients connect to the server, otherwise the server will not stop working.</remarks>
        public void Start(int workTimeInSecond)
        {
            if (workTimeInSecond < 1)
            {
                throw new ArgumentException();
            }
            if (!_listenSocket.Connected)
            {
                Connect();
            }
            Processing(workTimeInSecond);
        }

        /// <summary>
        /// A method that connects the server to the destination address and puts the socket in listening mode.
        /// </summary>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="SocketException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        private void Connect()
        {
            _listenSocket.Bind(ipPoint);
            _listenSocket.Listen(10);
        }

        /// <summary>
        /// The method which handles the client connections.
        /// </summary>
        /// <param name="workTimeInSecond">Server running time.</param>
        private void Processing(int workTimeInSecond)
        {
            DateTime startTime = DateTime.Now;
            long acceptableTimeToWorkInTicks = (long)workTimeInSecond * 10000000;
            while (DateTime.Now.Ticks - startTime.Ticks < acceptableTimeToWorkInTicks)
            { 
                try
                {
                    Socket handler = _listenSocket.Accept();
                    Thread clientProcessing = new Thread(new ThreadStart(() => { ChildProcessing(handler); }));
                    clientProcessing.Start();
                }
                catch (Exception)
                {
                    break;
                }
            }
        }
        /// <summary>
        /// Method that performs processing of the connected client
        /// </summary>
        /// <param name="handler">The client socket.</param>
        /// <exception cref="SocketException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="System.ObjectDisposedException"></exception>
        private void ChildProcessing(Socket handler)
        {
            string receivedMessage;
            try
            {
                while (true)
                {
                    receivedMessage = GetMessage(handler);

                    if (receivedMessage == "final message: EXIT")
                    {
                        break;
                    }

                    _show?.Invoke(receivedMessage);
                    _serverStringProcessing?.Invoke(receivedMessage);


                    SendMessage(handler, RepliesForMessages);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
        }

        /// <summary>
        /// Destructor of the client class.
        /// </summary>
        ~Server()
        {
            Stop();
        }
        /// <summary>
        /// Method that stops the server
        /// </summary>
        private void Stop()
        {
            _listenSocket.Shutdown(SocketShutdown.Both);
            _listenSocket.Close();
            if(_listenSocket.Connected)
            {
                _listenSocket.Disconnect(true);
            }
        }

    }
}
