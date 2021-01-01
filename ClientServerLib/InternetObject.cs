using System.Net.Sockets;
using System.Text;


namespace ClientServerLib
{
    /// <summary>
    /// Wrapper class for server and client classes.
    /// </summary>
    public class InternetObject
    {
        /// <summary>
        /// Method that perform message getting from cpecify socket.
        /// </summary>
        /// <param name="socket">Socket that will get message</param>
        /// <returns>Message that was gotten</returns>
        /// <exception cref="SocketException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="System.ObjectDisposedException"></exception>
        protected string GetMessage(Socket socket)
        {
            {
                int bytes;
                StringBuilder builder = new StringBuilder(0);
                byte[] data = new byte[256];
                do
                {
                    bytes = socket.Receive(data);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                } while (socket.Available > 0);
                return builder.ToString();
            }
        }

        /// <summary>
        /// Method that perform message sandin from specify socket. 
        /// </summary>
        /// <param name="socket">Socket that will send message.</param>
        /// <param name="message">Message that will be send using specify socket</param>
        /// <exception cref="SocketException"></exception>
        protected void SendMessage(Socket socket, string message)
        {
            byte[] data;
            data = Encoding.Unicode.GetBytes(message);
            socket.Send(data);
        }
    }
}
