using ChatWebSocket.Server.HandlersModels;
using ChatWebSocket.Server.Infrastructure.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChatWebSocket.Server;

class Program
{
    static async Task Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddScoped(typeof(IRequestMessage), typeof(RequestMessage));
        builder.Services.AddScoped(typeof(IResponseMessage), typeof(ResponseMessage));
        builder.Services.AddSingleton(typeof(IMessageHandler), typeof(AutoHandler));
        builder.Services.AddSingleton(typeof(IMessageChainHandlers), typeof(MessageChainHandlers));
        builder.Services.AddSingleton(typeof(Listener));
        
        using IHost host = builder.Build();

        // Запуск Слушателя
        var listener = host.Services.GetRequiredService<Listener>();
        if (listener != null)
        {
            listener.Listen();
        }
        
        await host.RunAsync();
    }
}