using Microsoft.Extensions.DependencyInjection;
using Zoo.Presentation.Console.UI;
using Zoo.Presentation.Console.CompositionRoot;
using System.Text;
Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

var services = new ServiceCollection();
Bootstrapper.AddZoo(services);

var sp = services.BuildServiceProvider();
await sp.GetRequiredService<ConsoleApp>().RunAsync();
