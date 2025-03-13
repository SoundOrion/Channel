using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSingleton(Channel.CreateUnbounded<string>()); // Channel を Singleton にする
        services.AddHostedService<DbPollingService>();
        services.AddHostedService<DataProcessingService>();
    })
    .Build();

//await host.StartAsync();


//var provider = services.BuildServiceProvider();
//var host = provider.GetRequiredService<IHost>();

await host.RunAsync();
