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
            
        var botToken = builder.Configuration["BOT_TOKEN"]
                       ?? builder.Configuration["BotToken"]
                       ?? throw new InvalidOperationException("BOT_TOKEN not configured");

        builder.Services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(botToken));
        
        builder.Services.AddHostedService<TelegramPollingService>();
        
        var host = builder.Build();
        host.Run();
    }
}