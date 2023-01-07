namespace ShoppingListApp;

using Api.Repositories;
using Api.Services;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static void Main()
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .ConfigureServices(services =>
            {
                _ = services
                    .AddTransient<ShoppingListsService>()
                    .AddSingleton<ShoppingListsRepository>();
            })
            .Build();

        host.Run();
    }
}