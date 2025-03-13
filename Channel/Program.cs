using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        //services.AddSingleton<ChannelManager>(); // `IHostedService` で `Channel<T>` を管理
        //services.AddSingleton(provider => provider.GetRequiredService<ChannelManager>().TaskChannel);
        //services.AddHostedService<DbPollingService>();
        //services.AddHostedService<DataProcessingService>();

        services.AddSingleton<ChannelManager>(); // `Singleton` として登録
        services.AddSingleton(provider => provider.GetRequiredService<ChannelManager>().TaskChannel);
        services.AddHostedService(provider => provider.GetRequiredService<ChannelManager>()); // `IHostedService` にも登録
        services.AddHostedService<DbPollingService>();
        services.AddHostedService<DataProcessingService>();
    })
    .Build();

await host.RunAsync();