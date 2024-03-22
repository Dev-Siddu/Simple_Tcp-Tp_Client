using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace TCP_Client
{
    internal class Program
    {
        // Main Progream start
        #region Main Program
        static void Main(string[] args)
        {
            Client client = null;
            // Connect to host
            do
            {
                string ipAddress = getIpAddress();
                int portNum = getPortNum();

                client = new Client(ipAddress, portNum);
                Tuple<bool,string> isConnected = client.Connect();

                // if failed to connect to host
                if (!isConnected.Item1)
                {
                    Console.WriteLine(isConnected.Item2);
                    continue;
                }
                // if connected to host
                Console.WriteLine("------------------ {0} -------------", isConnected.Item2);
                break;
            } while (true);

            do
            {
                string? messageToSend = string.Empty;
                Console.WriteLine("------------------------------------------");
                Console.Write("Enter Message to send to server : ");
                messageToSend = Console.ReadLine();
                // if entered string is "" || null
                if (string.IsNullOrEmpty(messageToSend))
                {
                    continue;
                }
                // sending message to server
                Tuple<bool,string> isSent = client.SendToServer(messageToSend);
                if (isSent.Item1 == false)
                {
                    Console.WriteLine(isSent.Item2);    
                    continue;
                }
                Console.WriteLine(isSent.Item2);

                // getting response from server
                Tuple<bool,string> response = client.ResponseFromServer();
                if(response.Item1 == false)
                {
                    Console.WriteLine(response.Item2);
                    continue;
                }
                Console.WriteLine(response.Item2);

            } while (client.IsConnectedToServer());
        }
        #endregion
        // Main Progream end

        // static method for getting the ipaddress
        #region Get IP_Address
        static string getIpAddress()
        {
            string ipAddress = string.Empty;
            Regex ipRegex = new Regex("^((25[0-5]|(2[0-4]|1\\d|[1-9]|)\\d)\\.?\\b){4}$");
            while (true)
            {
                Console.Write("Enter the IpAddress of server : ");
                string? enteredIP = Console.ReadLine();

                if (!ipRegex.Match(enteredIP).Success)
                {
                    Console.WriteLine("Entered IP address seems invalid\n");
                    Console.WriteLine();
                    continue;
                }
                ipAddress = enteredIP;
                break;
            }
            return ipAddress;
        }
        #endregion

        // static method for getting the port number
        #region get Port_Number
        static int getPortNum()
        {
            int portNum;
            while (true)
            {
                Console.Write("Enter the port number : ");
                string? enteredPort = Console.ReadLine();
                if (!int.TryParse(enteredPort, out portNum))
                {
                    Console.WriteLine("Entered port numbers seems to be invalid");
                    Console.WriteLine();
                    continue;
                }
                break;
            }
            return portNum;
        }
        #endregion
    }
}
