using ChatWebSocket.Server.HandlersModels;
using ChatWebSocket.Server.Infrastructure.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ChatWebSocket.Server;

class Program
{
    static async Task Main(string[] args)
    {
        // // Непосредственное использование configuration builder, т.е.
        // // не используя Hosting
        // var config = new ConfigurationBuilder()
        //     .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        //     .AddEnvironmentVariables()
        //     .Build();
        // var settings = 
        //     config.GetRequiredSection("NameSection").Get<object>();
        // settings.KeyOne;

        // // Непосредственное использование logger factory, т.е.
        // // не используя Hosting
        // NUGET: Microsoft.Extensions.Logging
        // NUGET: Microsoft.Extensions.Logging.Console
        // using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        // var logger = factory.CreateLogger(nameof(Program));
        // logger.LogInformation("Starting websocket server");
        
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
        
        builder.Services.AddScoped(typeof(IRequestMessage), typeof(RequestMessage));
        builder.Services.AddScoped(typeof(IResponseMessage), typeof(ResponseMessage));
        builder.Services.AddSingleton(typeof(IMessageHandler), typeof(AutoHandler));
        builder.Services.AddSingleton(typeof(IMessageChainHandlers), typeof(MessageChainHandlers));
        builder.Services.AddSingleton(typeof(Listener));

        using IHost host = builder.Build();
        
        // // Предполагается наличие файла appsettings.json в проекте
        // var config = host.Services.GetRequiredService<IConfiguration>();
        // config.GetValue<type>("key:subkey");
        // config["key:subkey"];

        // // NUGET: Microsoft.Extensions.Hosting
        // // NUGET: Microsoft.Extensions.Logging
        // var logger = host.Services.GetRequiredService<ILogger<Program>>();
        // logger.LogInformation("Host created.");

        // Запуск Слушателя
        var listener = host.Services.GetRequiredService<Listener>();
        if (listener != null)
        {
            listener.Listen();
        }
        
        await host.RunAsync();
    }
}