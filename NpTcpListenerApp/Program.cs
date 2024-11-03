// TcpListener
using NpTcpListenerApp;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

List<Flight> flights = new List<Flight>()
{
    new(){ Id = 1, Name = "DSE-923", FromCity = "Moscow", ToCity = "St. Peterburg"},
    new(){ Id = 3, Name = "AW-801", FromCity = "Kaliningrad", ToCity = "Kazan"},
    new(){ Id = 9, Name = "FRGQ-89", FromCity = "Omsk", ToCity = "Sochi"},
};

TcpListener server = new(IPAddress.Loopback, 5000);

try
{
    server.Start();
    Console.WriteLine($"Server {server.LocalEndpoint} Started");

    while( true )
    {
        TcpClient client = await server.AcceptTcpClientAsync();
        Task.Run(async () => await ClientTask(client));

    }
}
catch(Exception ex)
{
    Console.WriteLine(ex.Message);
}
finally
{
    server.Stop();
}

async Task ClientTask(TcpClient client)
{
    int port = ((IPEndPoint)client.Client.RemoteEndPoint).Port;
    Console.WriteLine($"Client {port} connected");

    NetworkStream stream = client.GetStream();

    List<byte> request = new();
    int requestSize;
    byte[] buffer = new byte[1024];
    
    while(true)
    {
        do
        {
            requestSize = await stream.ReadAsync(buffer);
            request.AddRange(buffer);
        } while (requestSize > 0);

        int id = Int32.Parse(Encoding.UTF8.GetString(request.ToArray()));

        Console.WriteLine($"Client {port} request {id}");

        Flight? flight = flights.FirstOrDefault(f => f.Id == id);

        byte[] response;
        string responseMessage;

        if (flight is not null)
        {
            responseMessage = JsonSerializer.Serialize<Flight>(flight);
        }
        else
        {
            responseMessage = "Not found";
        }
        response = Encoding.UTF8.GetBytes(responseMessage);
        await stream.WriteAsync(response);

        request.Clear();
    }
}