using Microsoft.Extensions.Hosting;

using System.Threading.Channels;

/// <summary>
/// データ処理の完了を監視し、終了を通知するサービス
/// </summary>
public class ProcessingCompletionNotifierService : BackgroundService
{
    private readonly Channel<bool> _completionChannel;

    public ProcessingCompletionNotifierService(Channel<bool> completionChannel)
    {
        _completionChannel = completionChannel;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await _completionChannel.Reader.ReadAsync(stoppingToken);
                Console.WriteLine("ProcessingCompletionNotifierService: 処理完了通知を受信しました。");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("ProcessingCompletionNotifierService: キャンセルされました。");
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ProcessingCompletionNotifierService: エラー - {ex.Message}");
            }
        }
    }
}