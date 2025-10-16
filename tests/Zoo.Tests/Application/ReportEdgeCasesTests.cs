//using System.Threading.Tasks;
using FluentAssertions;
using Zoo.Application.Interfaces;
using Zoo.Application.UseCases.Reports;
using Zoo.Domain.Entities.Animals;
//using Xunit;

namespace Zoo.Tests.Application;

public class ReportEdgeCasesTests
{
    [Fact]
    public async Task TotalFood_is_zero_when_no_animals()
    {
        var sut = new ReportService(new MemAnimals(), new MemThings());
        (await sut.GetTotalFoodKgPerDayAsync()).Should().Be(0);
    }

    [Fact]
    public async Task ContactZoo_is_empty_when_no_herbivores_with_kindness_gt5()
    {
        var a = new MemAnimals();
        await a.AddAsync(new Monkey("Abu", 2, 1, 5)); // ровно 5 — не проходит
        await a.AddAsync(new Tiger("Sher", 3, 5));

        var sut = new ReportService(a, new MemThings());
        (await sut.GetContactZooAsync()).Should().BeEmpty();
    }

    private sealed class MemAnimals : IAnimalRepository
    {
        private readonly List<Animal> _list = new();
        public Task AddAsync(Animal animal) { _list.Add(animal); return Task.CompletedTask; }
        public Task<IReadOnlyList<Animal>> ListAsync() => Task.FromResult((IReadOnlyList<Animal>)_list);
    }
    private sealed class MemThings : IThingRepository
    {
        private readonly List<Zoo.Domain.Entities.Things.Thing> _list = new();
        public Task AddAsync(Zoo.Domain.Entities.Things.Thing thing) { _list.Add(thing); return Task.CompletedTask; }
        public Task<IReadOnlyList<Zoo.Domain.Entities.Things.Thing>> ListAsync() => Task.FromResult((IReadOnlyList<Zoo.Domain.Entities.Things.Thing>)_list);
    }
}
