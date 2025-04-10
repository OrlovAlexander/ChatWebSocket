using System.Net.WebSockets;
using System.Text;
using ChatWebSocket.Server.HandlersModels;

namespace ChatWebSocket.Server.Infrastructure.Handlers;

internal class AutoHandler : IMessageHandler
{
    private byte[] _receiveBuffer = new byte[1024];

    private string _receivedMessage;
    
    public async Task IncomingHandler(IRequestMessage requestMessage)
    {
        var webSocket = requestMessage.WebSocket;
        
        var receiveResult =
            await webSocket.ReceiveAsync(new ArraySegment<byte>(_receiveBuffer), CancellationToken.None);

        if (receiveResult.MessageType == WebSocketMessageType.Text)
        {
            _receivedMessage = Encoding.UTF8.GetString(_receiveBuffer, 0, receiveResult.Count);
            Console.WriteLine($"Received message: {_receivedMessage}");
        }
        else if (receiveResult.MessageType == WebSocketMessageType.Close)
        {
            // await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
            Console.WriteLine("WebSocket closed.");
        }
        
        
    }

    public async Task OutgoingHandler(IResponseMessage responseMessage)
    {
        var webSocket = responseMessage.WebSocket;

        if (string.IsNullOrWhiteSpace(_receivedMessage))
        {
            return;
        }
        
        var buffer = Encoding.UTF8.GetBytes($"Hello from server");
        await webSocket.SendAsync(new ArraySegment<byte>(buffer), 
            WebSocketMessageType.Text, true, CancellationToken.None);
    }
}