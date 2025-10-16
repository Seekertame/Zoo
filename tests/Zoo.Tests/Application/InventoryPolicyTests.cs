//using System.Threading.Tasks;
using FluentAssertions;
using Zoo.Application.Exceptions;
using Zoo.Infrastructure.Policies;
using Zoo.Domain.Entities.Animals;
using Zoo.Domain.Entities.Things;
using Zoo.Application.Interfaces;
//using Xunit;

namespace Zoo.Tests.Application;

public class InventoryPolicyTests
{
    [Fact]
    public async Task EnsureUniqueAsync_passes_when_number_is_free()
    {
        var animals = new MemAnimalRepo();
        var things = new MemThingRepo();
        var policy = new InMemoryInventoryPolicy(animals, things);

        await policy.EnsureUniqueAsync(123); // не должно бросать
    }

    [Fact]
    public async Task EnsureUniqueAsync_throws_when_number_taken_by_animal_or_thing()
    {
        var animals = new MemAnimalRepo();
        var things = new MemThingRepo();
        var policy = new InMemoryInventoryPolicy(animals, things);

        await animals.AddAsync(new Rabbit("Bunny", 1001, 1, 8));
        await policy.Invoking(p => p.EnsureUniqueAsync(1001)).Should().ThrowAsync<InventoryConflictException>();

        await things.AddAsync(new Table("Стол", 2002));
        await policy.Invoking(p => p.EnsureUniqueAsync(2002)).Should().ThrowAsync<InventoryConflictException>();
    }

    // простые in-memory фейки
    private sealed class MemAnimalRepo : IAnimalRepository
    {
        private readonly List<Animal> _list = new();
        public Task AddAsync(Animal a) { _list.Add(a); return Task.CompletedTask; }
        public Task<IReadOnlyList<Animal>> ListAsync() => Task.FromResult((IReadOnlyList<Animal>)_list);
    }
    private sealed class MemThingRepo : IThingRepository
    {
        private readonly List<Thing> _list = new();
        public Task AddAsync(Thing t) { _list.Add(t); return Task.CompletedTask; }
        public Task<IReadOnlyList<Thing>> ListAsync() => Task.FromResult((IReadOnlyList<Thing>)_list);
    }
}
