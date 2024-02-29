using System.Net.Sockets;
using System.Net;

class Server
{
    static void Main(string[] args)
    {
        IPEndPoint iep = new IPEndPoint(IPAddress.Parse("192.168.1.9"), 2010);
        Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
        ProtocolType.Tcp);
        server.Bind(iep);
        server.Listen(10);
        Console.WriteLine("Cho ket noi tu client");
        Socket client = server.Accept();
        //***********************************
        NetworkStream ns = new NetworkStream(client);
        StreamReader sr = new StreamReader(ns);
        StreamWriter sw = new StreamWriter(ns);
        Console.WriteLine("Chap nhan ket noi tu:{0}", client.RemoteEndPoint.ToString());
        string s = "Chao ban den voi Server";
        sw.WriteLine(s);
        sw.Flush();
        while (true)
        {
            s = sr.ReadLine();
            Console.WriteLine("Client gui len:{0}", s);
            //Neu chuoi nhan duoc la Thoat thi thoat
            if (s.ToUpper().Equals("THOAT")) break;
            //Gui tra lai cho client chuoi s
            s = Console.ReadLine();
            sw.WriteLine(s);
            sw.Flush();

        }
        client.Shutdown(SocketShutdown.Both);
        client.Close();
        server.Close();
    }
}
