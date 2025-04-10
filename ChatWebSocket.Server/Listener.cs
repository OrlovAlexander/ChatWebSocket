using System.Net.WebSockets;
using System.Net;
using ChatWebSocket.Server.HandlersModels;

namespace ChatWebSocket.Server;

class Listener
{
    private readonly IMessageChainHandlers _chainHandlers;

    public Listener(IMessageChainHandlers chainHandlers)
    {
        _chainHandlers = chainHandlers;
    }
    
    internal async Task Listen()
    {
        var httpListener = new HttpListener();
        httpListener.Prefixes.Add("http://localhost:9006/");
        httpListener.Start();

        Console.WriteLine("Listening for WebSocket connections...");

        while (true)
        {
            var context = await httpListener.GetContextAsync();
            if (context.Request.IsWebSocketRequest)
            {
                await ProcessWebSocketRequest(context);
            }
            else
            {
                context.Response.StatusCode = 400;
                context.Response.Close();
            }
        }
    }

    private async Task ProcessWebSocketRequest(HttpListenerContext context)
    {
        try
        {
            var webSocketContext = await context.AcceptWebSocketAsync(subProtocol: null);
            var webSocket = webSocketContext.WebSocket;

            Console.WriteLine("Client connected");

            if (webSocket.State == WebSocketState.Open)
            {
                await _chainHandlers.Process(webSocket);
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }
}