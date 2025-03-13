using Microsoft.Extensions.Hosting;
using System.Threading.Channels;

public class DataProcessingService : BackgroundService
{
    private readonly Channel<string> _channel;

    public DataProcessingService(Channel<string> channel)
    {
        _channel = channel; // DI から Channel を受け取る
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var data in _channel.Reader.ReadAllAsync(stoppingToken)) // Channel からデータを読み取る
        {
            ProcessData(data);
        }
    }

    private void ProcessData(string data)
    {
        Console.WriteLine($"Processing: {data}");
    }
}
