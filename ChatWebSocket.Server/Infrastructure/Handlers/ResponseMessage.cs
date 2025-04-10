using System.Net.WebSockets;
using ChatWebSocket.Server.HandlersModels;

namespace ChatWebSocket.Server.Infrastructure.Handlers;

public class ResponseMessage : IResponseMessage
{
    public WebSocket WebSocket { get; set; }
}