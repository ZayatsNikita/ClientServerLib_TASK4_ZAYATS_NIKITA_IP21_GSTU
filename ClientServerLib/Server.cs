using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ClientServerLib
{
    public class Server : InternetObject
    {
        private event ProcessingStringOnServer _serverStringProcessing;

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
        public string RepliesToMessages
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
        public Server(int port, string ipPoint, string rerliesToMessages, ShowData show, ProcessingStringOnServer processingStringOnServer)
        {

            if (port < 1024 || port > 65536)
            {
                throw new ArgumentException();
            }

            RepliesToMessages = rerliesToMessages;

            this.ipPoint = new IPEndPoint(IPAddress.Parse(ipPoint), port);

            _listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            ServerStringProcessing += processingStringOnServer;

            Show += show;
        }

        public void Start(int workTimeInSecond)
        {
            if (workTimeInSecond < 1)
            {
                throw new ArgumentException();
            }
            Connect();
            Processing(workTimeInSecond);
        }


        private void Connect()
        {
            _listenSocket.Bind(ipPoint);
            _listenSocket.Listen(10);
        }

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
            Stop();


        }

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


                    SendMessage(handler, RepliesToMessages);
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

        private void Stop()
        {
            _listenSocket.Shutdown(SocketShutdown.Both);
            _listenSocket.Close();
        }

    }
}
