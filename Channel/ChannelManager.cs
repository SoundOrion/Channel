using Microsoft.Extensions.Hosting;
using System.Threading.Channels;

/// <summary>
/// `Channel<T>` を `IHostedService` として管理する
/// </summary>
public class ChannelManager : IHostedService
{
    public Channel<string> TaskChannel { get; } = Channel.CreateUnbounded<string>();
    public Channel<bool> CompletionChannel { get; } = Channel.CreateUnbounded<bool>();

    public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public Task StopAsync(CancellationToken cancellationToken)
    {
        TaskChannel.Writer.Complete(); // アプリ終了時に `Channel<T>` を適切に閉じる
        CompletionChannel.Writer.Complete();
        return Task.CompletedTask;
    }
}