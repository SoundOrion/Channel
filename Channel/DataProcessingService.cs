using Microsoft.Extensions.Hosting;

using System.Threading.Channels;

/// <summary>
/// `Channel<T>` からデータを受信し、処理するサービス
/// </summary>
public class DataProcessingService : BackgroundService
{
    private readonly Channel<string> _channel;
    private readonly SemaphoreSlim _semaphore = new(5); // 並列数制御

    public DataProcessingService(Channel<string> channel)
    {
        _channel = channel;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _semaphore.WaitAsync(stoppingToken);

                _ = ProcessDataAsync(stoppingToken).ContinueWith(_ =>
                {
                    _semaphore.Release();
                }, TaskScheduler.Default);
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("DataProcessingService: 処理をキャンセルしました。");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"DataProcessingService: エラー - {ex.Message}");
        }
    }

    private async Task ProcessDataAsync(CancellationToken stoppingToken)
    {
        try
        {
            var data = await _channel.Reader.ReadAsync(stoppingToken);
            Console.WriteLine($"処理中: {data}");
            await Task.Delay(500, stoppingToken); // 処理のシミュレーション
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("DataProcessingService: キャンセルされました。");
        }
    }
}