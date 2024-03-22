using System.Data.SqlTypes;
using System.Net.Sockets;
using System.Text;

namespace TCP_Client
{
    public class Client
    {
        private readonly string _IPAddress;
        private readonly int _Port;

        private TcpClient? host { get; set; } = null;
        public NetworkStream? netWorkStream { get; set; } = null;
        public Client(string iPAddress, int port)
        {
            _IPAddress = iPAddress;
            _Port = port;   
        }

        public Tuple<bool,string> Connect()
        {
            Tuple<bool, string> result;
            try
            {
                host = new TcpClient(_IPAddress,_Port);
                netWorkStream = host.GetStream();
                result = new Tuple<bool, string>(true,$"Connected to host successfully to {_IPAddress}:{_Port}");
            }
            catch (SocketException socketExce)
            {
                result = new Tuple<bool, string>(false, socketExce.Message);
            }
            catch (Exception ex)
            {
                result = new Tuple<bool, string>(false, ex.Message);
            }
            return result;
        }

        public bool IsConnectedToServer()
        {
            return host.Connected;
        } 

        public Tuple<bool,string> SendToServer(string message)
        {
            if (!IsConnectedToServer()) return new Tuple<bool,string>(false, "Not connected to server/host");

            byte[] messageBytes = Encoding.UTF8.GetBytes(message,0,message.Length);
            netWorkStream?.Write(messageBytes,0,messageBytes.Length);
            return new Tuple<bool, string>(true,$"sent to server : {message}");
        }

        public Tuple<bool,string> ResponseFromServer()
        {
            if (!IsConnectedToServer()) return new Tuple<bool, string>(false, "Not connected to server/host");

            try
            {
                // if client connected to host / server
                string message = string.Empty;
                
                // reading from network stream untill the data is available in network stream
                do
                {
                    byte[] buffer = new byte[10];
                    int noOfBytesRead = 0;
                    noOfBytesRead = netWorkStream.Read(buffer, 0, buffer.Length);
                    if (noOfBytesRead > 0)
                    {
                        message += Encoding.UTF8.GetString(buffer, 0, noOfBytesRead);
                        noOfBytesRead = 0;
                    }
                    else
                    {
                        break;
                    }
                } while (netWorkStream.DataAvailable);

                if (string.IsNullOrEmpty(message)) return new Tuple<bool, string>(false, "no Content received");
                return new Tuple<bool, string>(true, $"Response from server {message}");
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false,ex.Message);
            }
        }
    }
}
