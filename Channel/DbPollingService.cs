using Microsoft.Extensions.Hosting;
using System.Threading.Channels;

public class DbPollingService : BackgroundService
{
    private readonly Channel<string> _channel;

    public DbPollingService(Channel<string> channel)
    {
        _channel = channel; // DI から Channel を受け取る
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var data = await FetchDataFromDbAsync();
            if (data != null)
            {
                await _channel.Writer.WriteAsync(data, stoppingToken); // Channel にデータを書き込む
            }
            await Task.Delay(1000, stoppingToken); // 負荷を抑えるために適切に待機
        }
    }

    private async Task<string> FetchDataFromDbAsync()
    {
        await Task.Delay(100); // 仮の非同期処理（DB アクセス）
        return $"Data at {DateTime.Now}";
    }
}
