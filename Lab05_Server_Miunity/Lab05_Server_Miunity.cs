using System.Net;
using System.Net.Sockets;
using System.Text;


class Server
{
    static List<StreamWriter> clients = new List<StreamWriter>();
    static void Main(string[] args)
    {
        IPEndPoint iep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2024);
        Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        server.Bind(iep);
        server.Listen(10);
        Console.WriteLine("Cho ket noi tu client");

        while (true)
        {
            Socket client = server.Accept();
            Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
            clientThread.Start(client);
        }
    }

    static void HandleClient(object obj)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Socket client = (Socket)obj;
        NetworkStream ns = new NetworkStream(client);
        StreamReader sr = new StreamReader(ns);
        StreamWriter sw = new StreamWriter(ns);
        sw.WriteLine("Nhap ten cua ban ben duoi\n");
        sw.Flush();
        string clientName = sr.ReadLine();
        Console.WriteLine("Chap nhan ket noi tu: {0}", clientName);
        string s = "Chao ban den voi Server cua nhom";
        sw.WriteLine(s);
        sw.Flush();
        clients.Add(sw);

        Thread readThread = new Thread(() =>
        {
            while (true)
            {
                s = sr.ReadLine();
                Console.WriteLine("{0} gui len: {1}", clientName, s);
                if (s.ToUpper().Equals("THOAT")) break;
                foreach (var clientSw in clients)
                {
                    clientSw.WriteLine("\n{0}: {1}", clientName, s);
                    clientSw.Flush();
                }
            }
        });
        readThread.Start();

        Thread writeThread = new Thread(() =>
        {
            while (true)
            {
                s = Console.ReadLine();
                foreach (var clientSw in clients)
                {
                    clientSw.WriteLine("Server: {0}", s);
                    clientSw.Flush();
                }
                if (s.ToUpper().Equals("THOAT")) break;
            }
        });
        writeThread.Start();
        readThread.Join();
        writeThread.Join();
        clients.Remove(sw);
        client.Shutdown(SocketShutdown.Both);
        client.Close();
    }
}
