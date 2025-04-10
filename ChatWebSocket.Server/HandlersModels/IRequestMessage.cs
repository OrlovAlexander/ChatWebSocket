using System.Net.WebSockets;

namespace ChatWebSocket.Server.HandlersModels;

internal interface IRequestMessage
{
    WebSocket WebSocket { get; set; }
}