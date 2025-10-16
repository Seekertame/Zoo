using Microsoft.Extensions.DependencyInjection;
// using Zoo.Infrastructure;
// using Zoo.Infrastructure.DI;
using Zoo.Presentation.Console.UI;
using Zoo.Presentation.Console.UI.Printers;

namespace Zoo.Presentation.Console.CompositionRoot;

public static class Bootstrapper
{
    public static void AddZoo(IServiceCollection services)
    {
        services.AddZooCore()
                .AddZooInfra();

        services.AddSingleton<ConsoleApp>();
        services.AddSingleton<Menu>();
        services.AddSingleton<InputReader>();
        services.AddSingleton<InventoryPrinter>();
        services.AddSingleton<TotalFoodPrinter>();
        services.AddSingleton<ContactZooPrinter>();
    }
}
