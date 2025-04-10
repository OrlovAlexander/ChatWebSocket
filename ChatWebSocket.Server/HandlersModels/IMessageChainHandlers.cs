using System.Net.WebSockets;

namespace ChatWebSocket.Server.HandlersModels;

internal interface IMessageChainHandlers
{
    Task Process(WebSocket webSocket);
}