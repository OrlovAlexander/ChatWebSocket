using System.Net.WebSockets;
using System.Text;

namespace ChatWebSocket;

public class Client
{
    internal async Task Send(string input)
    {
        using var ws = new ClientWebSocket();

        var uri = new Uri("ws://localhost:9006");
        await ws.ConnectAsync(uri, CancellationToken.None);

        Console.WriteLine("Connected");

        var message = input;
        var buffer = Encoding.UTF8.GetBytes(message);
        await ws.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);

        var receiveBuffer = new byte[1024];
        var receiveResult = await ws.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
        var receivedMessage = Encoding.UTF8.GetString(receiveBuffer, 0, receiveResult.Count);

        Console.WriteLine($"Received message: {receivedMessage}");    
    }
}