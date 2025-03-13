using Microsoft.Extensions.Hosting;

using System.Threading.Channels;

/// <summary>
/// DB からデータを取得し、`Channel<T>` に送信するサービス
/// </summary>
public class DbPollingService : BackgroundService
{
    private readonly Channel<string> _channel;

    public DbPollingService(Channel<string> channel)
    {
        _channel = channel;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var data = $"データ: {DateTime.Now}";
                await _channel.Writer.WriteAsync(data, stoppingToken);
                Console.WriteLine($"DB取得: {data}");

                await Task.Delay(1000, stoppingToken); // 1秒ごとにデータ取得
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("DbPollingService: 処理をキャンセルしました。");
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DbPollingService: エラー - {ex.Message}");
            }
        }
    }
}