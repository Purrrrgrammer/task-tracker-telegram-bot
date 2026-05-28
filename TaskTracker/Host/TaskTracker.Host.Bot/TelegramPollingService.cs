using System.Net;
using System.Net.Security;
using System.Security.Authentication;
using Telegram.Bot;
namespace TaskTracker.Hos.Bot;

public class TelegramPollingService(ITelegramBotClient botClient, ILogger<TelegramPollingService> logger) : BackgroundService
{
    private int _testTries = 3;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_testTries != 0)
                {
                    if (logger.IsEnabled(LogLevel.Information))
                    {
                        logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    }

                    var me = await botClient.GetMe();
                    logger.LogInformation($"Hello, World! I am user {me.Id} and my name is {me.FirstName}.");
                    _testTries--;
                }

                await Task.Delay(5000, stoppingToken);
            }
        }
        catch (Exception e)
        {
            if (logger.IsEnabled(LogLevel.Critical))
            {
                logger.LogCritical($"Worker stopped at: {DateTimeOffset.Now}; {e.Message}");
                
                if(e.InnerException is not null)
                    logger.LogCritical($"InnerException: {e.InnerException.Message}");
            }
        }
    }
}