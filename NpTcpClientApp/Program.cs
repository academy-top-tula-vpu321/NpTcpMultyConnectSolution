// TcpClient

using NpTcpClientApp;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

using(TcpClient client = new())
{
    await client.ConnectAsync(IPAddress.Loopback, 5000);
    NetworkStream stream = client.GetStream();

    List<byte> response = new List<byte>();
    int responseSize;

    Console.Write("Input id flight: ");
    string? id = Console.ReadLine();

    byte[] request = Encoding.UTF8.GetBytes(id);
    await stream.WriteAsync(request);

    byte[] buffer = new byte[1024];
    int bufferSize;
    do
    {
        bufferSize = stream.Read(buffer);
        response.AddRange(buffer);

    } while (bufferSize > 0);


    string responseMessage = Encoding.UTF8.GetString(response.ToArray());

    Flight? flight = JsonSerializer.Deserialize<Flight>(responseMessage);
    
    if (flight is null)
        Console.WriteLine(responseMessage);
    else
        Console.WriteLine(flight);
    Console.WriteLine();

    response.Clear();

}