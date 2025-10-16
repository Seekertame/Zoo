using Zoo.Application.Exceptions;
using Zoo.Application.Interfaces;
using Zoo.Application.UseCases.Inventory;
using Zoo.Domain.Entities.Animals;
using Zoo.Domain.Entities.Things;
using Zoo.Presentation.Console.UI.Printers;

namespace Zoo.Presentation.Console.UI;

public sealed class ConsoleApp
{
    private readonly Menu _menu;
    private readonly InputReader _in;
    private readonly IAdmissionService _admission;
    private readonly IReportService _reports;
    private readonly IThingService _thingService;
    private readonly InventoryPrinter _invPrinter;
    private readonly TotalFoodPrinter _foodPrinter;
    private readonly ContactZooPrinter _contactPrinter;

    public ConsoleApp(
        Menu menu,
        InputReader input,
        IAdmissionService admission,
        IReportService reports,
        IThingService thingsService,
        InventoryPrinter invPrinter,
        TotalFoodPrinter foodPrinter,
        ContactZooPrinter contactPrinter)
    {
        _menu = menu; _in = input;
        _admission = admission; _reports = reports; _thingService = thingsService;
        _invPrinter = invPrinter; _foodPrinter = foodPrinter; _contactPrinter = contactPrinter;
    }

    public async Task RunAsync()
    {
        await SeedAsync(); // немного данных для старта

        while (true)
        {
            _menu.Show();
            var choice = _in.ReadInt("Ваш выбор");
            switch (choice)
            {
                case 1: await AddAnimalAsync(); break;
                case 2: await ShowTotalFoodAsync(); break;
                case 3: await ShowContactZooAsync(); break;
                case 4: await ShowInventoryAsync(); break;
                case 0: return;
                default: System.Console.WriteLine("Нет такого пункта."); break;
            }
        }
    }

    private async Task SeedAsync()
    {
        // вещи на баланс
        await _thingService.AddAsync(new Table("Стол для вольера", 2001));
        await _thingService.AddAsync(new Computer("ПК кассира", 2002));

        // одно животное через приём (пройдёт через «ветклинику»)
        await _admission.AdmitAsync(new Rabbit("Bunny", 1001, foodKgPerDay: 1, kindness: 8));
    }

    private async Task AddAnimalAsync()
    {
        System.Console.WriteLine("Вид: 1 - Rabbit, 2 - Monkey, 3 - Tiger, 4 - Wolf");
        var t = _in.ReadInt("Введите номер вида", 1, 4);
        var name = _in.ReadRequired("Имя/кличка");
        var number = _in.ReadPositive("Инв. номер");
        var food = _in.ReadNonNegative("Корм (кг/сут)");

        Animal a = t switch
        {
            1 => new Rabbit(name, number, food, _in.ReadInt("Доброта", 0, 10)),
            2 => new Monkey(name, number, food, _in.ReadInt("Доброта", 0, 10)),
            3 => new Tiger(name, number, food),
            4 => new Wolf(name, number, food),
            _ => throw new InvalidOperationException()
        };

        try
        {
            var decision = await _admission.AdmitAsync(a);
            System.Console.WriteLine(decision == AdmissionDecision.Accept
                ? "Животное принято."
                : "Отклонено ветклиникой.");
        }
        catch (InventoryConflictException ex)
        {
            System.Console.WriteLine($"Инвентарный номер уже занят: {ex.Number}");
        }
        Pause();
    }


    private async Task ShowTotalFoodAsync()
    {
        var total = await _reports.GetTotalFoodKgPerDayAsync();
        _foodPrinter.Print(total);
        Pause();
    }

    private async Task ShowContactZooAsync()
    {
        var list = await _reports.GetContactZooAsync();
        _contactPrinter.Print(list.Select(h => $"{h.Species} {h.Name} (Kindness {h.Kindness})"));
        Pause();
    }

    private async Task ShowInventoryAsync()
    {
        var items = await _reports.GetInventoryAsync();
        _invPrinter.Print(items.Select(i => $"{i.Title} — №{i.Number}"));
        Pause();
    }

    private static void Pause()
    {
        System.Console.WriteLine("Нажмите Enter для продолжения...");
        System.Console.ReadLine();
    }
}
