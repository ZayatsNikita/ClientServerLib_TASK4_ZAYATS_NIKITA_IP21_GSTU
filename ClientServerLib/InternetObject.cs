using System.Net.Sockets;
using System.Text;


namespace ClientServerLib
{
    public class InternetObject
    {
        protected string GetMessage(Socket socket)
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


        protected void SendMessage(Socket socket, string message)
        {
            byte[] data;
            data = Encoding.Unicode.GetBytes(message);
            socket.Send(data);
        }
    }
}
