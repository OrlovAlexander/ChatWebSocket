using System.Net.WebSockets;
using ChatWebSocket.Server.HandlersModels;
using Microsoft.Extensions.DependencyInjection;

namespace ChatWebSocket.Server.Infrastructure.Handlers;

/// <summary>
/// Реализация цепочки обработчиков
/// </summary>
internal class MessageChainHandlers : IMessageChainHandlers
{
    /// <summary>
    /// DI
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Коллекция обработчиков
    /// </summary>
    private readonly List<IMessageHandler> _handlers;

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="serviceProvider">DI</param>
    public MessageChainHandlers(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        foreach (var messageHandler in _serviceProvider.GetServices<IMessageHandler>())
        {
            _handlers ??= new List<IMessageHandler>();
            _handlers.Add(messageHandler);
        }
    }

    /// <summary>
    /// Обработать сообщение
    /// </summary>
    /// <param name="webSocket">Контекст сообщения</param>
    public async Task Process(WebSocket webSocket)
    {
        // Задаем Scope для DI
        using IServiceScope serviceScope = _serviceProvider.CreateScope();
        IServiceProvider provider = serviceScope.ServiceProvider;
        
        var _requestMessage = provider.GetRequiredService<IRequestMessage>();
        _requestMessage.WebSocket = webSocket;
        foreach (var messageHandler in _handlers)
        {
            await messageHandler.IncomingHandler(_requestMessage);
        }

        // Разворот и идем обратно
        
        var _responseMessage = provider.GetRequiredService<IResponseMessage>();
        _responseMessage.WebSocket = webSocket;
        _handlers.Reverse();
        foreach (var messageHandler in _handlers)
        {
            await messageHandler.OutgoingHandler(_responseMessage);
        }
        _handlers.Reverse();
    }
}