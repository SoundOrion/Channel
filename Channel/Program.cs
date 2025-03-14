using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSingleton<ChannelManager>();
        services.AddSingleton(provider => provider.GetRequiredService<ChannelManager>().TaskChannel);
        services.AddSingleton(provider => provider.GetRequiredService<ChannelManager>().CompletionChannel);
        services.AddHostedService(provider => provider.GetRequiredService<ChannelManager>());
        services.AddHostedService<DbPollingService>();
        services.AddHostedService<DataProcessingService>();
        services.AddHostedService<ProcessingCompletionNotifierService>(); // 通知用サービス追加
    })
    .Build();

await host.RunAsync();