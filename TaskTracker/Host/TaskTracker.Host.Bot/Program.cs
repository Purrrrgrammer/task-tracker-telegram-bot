using Telegram.Bot;

namespace TaskTracker.Hos.Bot;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);
        builder.Configuration.AddEnvironmentVariables();
            
        var botToken = builder.Configuration["BotToken"]
                       ?? throw new InvalidOperationException("BotToken not configured");

        builder.Services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(botToken));
        
        builder.Services.AddHostedService<TelegramPollingService>();
        
        var host = builder.Build();
        host.Run();
    }
}