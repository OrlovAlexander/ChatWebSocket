using System.Net.WebSockets;

namespace ChatWebSocket.Server.HandlersModels;

public interface IResponseMessage
{
    WebSocket WebSocket { get; set; }
}