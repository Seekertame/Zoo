using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Zoo.Application.Interfaces;
using Zoo.Application.UseCases.Reports;
using Zoo.Domain.Entities.Animals;
using Zoo.Domain.Entities.Things;
using Xunit;

namespace Zoo.Tests.Application;

public class ReportServiceTests
{
    [Fact]
    public async Task TotalFood_is_sum_of_food_over_all_animals()
    {
        var animals = new ListAnimalRepo(new Animal[]
        {
            new Rabbit("Bunny", 1, 1, 7),
            new Monkey("Abu", 2, 2, 6),
            new Tiger("Sher", 3, 5)
        });
        var things = new ListThingRepo(); // пусто не мешает

        var sut = new ReportService(animals, things);

        var total = await sut.GetTotalFoodKgPerDayAsync();
        total.Should().Be(1 + 2 + 5);
    }

    [Fact]
    public async Task ContactZoo_returns_only_herbivores_with_kindness_gt_5()
    {
        var animals = new ListAnimalRepo(new Animal[]
        {
            new Rabbit("Kind", 1, 1, 6),   // ок
            new Monkey("Neutral", 2, 1, 5),// не ок
            new Tiger("Pred", 3, 5)        // не ок
        });
        var things = new ListThingRepo();

        var sut = new ReportService(animals, things);

        var list = await sut.GetContactZooAsync();
        list.Select(a => a.Name).Should().BeEquivalentTo(new[] { "Kind" });
    }

    [Fact]
    public async Task Inventory_includes_animals_and_things()
    {
        var animals = new ListAnimalRepo(new Animal[]
        {
            new Rabbit("Bunny", 1001, 1, 8),
        });
        var things = new ListThingRepo(new Thing[]
        {
            new Table("Стол", 2001),
            new Computer("ПК", 2002)
        });

        var sut = new ReportService(animals, things);

        var items = await sut.GetInventoryAsync();
        items.Should().HaveCount(3);
        items.Any(i => i.Title.Contains("Bunny")).Should().BeTrue();
        items.Any(i => i.Title.Contains("Стол")).Should().BeTrue();
    }

    // ===== fakes =====
    private sealed class ListAnimalRepo : IAnimalRepository
    {
        private readonly List<Animal> _list;
        public ListAnimalRepo(IEnumerable<Animal>? seed = null) => _list = seed?.ToList() ?? [];
        public Task AddAsync(Animal animal) { _list.Add(animal); return Task.CompletedTask; }
        public Task<IReadOnlyList<Animal>> ListAsync() => Task.FromResult((IReadOnlyList<Animal>)_list);
    }

    private sealed class ListThingRepo : IThingRepository
    {
        private readonly List<Thing> _list;
        public ListThingRepo(IEnumerable<Thing>? seed = null) => _list = seed?.ToList() ?? [];
        public Task AddAsync(Thing thing) { _list.Add(thing); return Task.CompletedTask; }
        public Task<IReadOnlyList<Thing>> ListAsync() => Task.FromResult((IReadOnlyList<Thing>)_list);
    }
}
