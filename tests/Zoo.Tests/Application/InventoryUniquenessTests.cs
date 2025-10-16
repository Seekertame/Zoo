using FluentAssertions;
using Zoo.Application.Exceptions;
using Zoo.Application.Interfaces;
using Zoo.Application.UseCases.Admission;
using Zoo.Application.UseCases.Inventory;
using Zoo.Domain.Entities.Animals;
using Zoo.Domain.Entities.Things;

namespace Zoo.Tests.Application;

public class InventoryUniquenessTests
{
    [Fact]
    public async Task Adding_thing_with_existing_animal_number_should_fail()
    {
        var animals = new MemAnimalRepo();
        var things = new MemThingRepo();
        var uow = new NoopUow();
        var policy = new Policy(animals, things);
        var clinic = new AlwaysAcceptClinic();

        var admission = new AdmissionService(clinic, animals, uow, policy);
        var thingSvc = new ThingService(things, policy, uow);

        await admission.AdmitAsync(new Rabbit("Bunny", 1001, 1, 8));

        var act = () => thingSvc.AddAsync(new Table("Стол", 1001));
        await act.Should().ThrowAsync<InventoryConflictException>();
    }

    // fakes
    private sealed class MemAnimalRepo : IAnimalRepository
    {
        private readonly List<Animal> _list = [];
        public Task AddAsync(Animal a) { _list.Add(a); return Task.CompletedTask; }
        public Task<IReadOnlyList<Animal>> ListAsync() => Task.FromResult((IReadOnlyList<Animal>)_list);
    }
    private sealed class MemThingRepo : IThingRepository
    {
        private readonly List<Thing> _list = [];
        public Task AddAsync(Thing t) { _list.Add(t); return Task.CompletedTask; }
        public Task<IReadOnlyList<Thing>> ListAsync() => Task.FromResult((IReadOnlyList<Thing>)_list);
    }
    private sealed class NoopUow : IUnitOfWork { public Task SaveChangesAsync() => Task.CompletedTask; }
    private sealed class AlwaysAcceptClinic : IVeterinaryClinic
    { public AdmissionDecision Inspect(Animal _) => AdmissionDecision.Accept; }
    private sealed class Policy : IInventoryPolicy
    {
        private readonly IAnimalRepository _a; private readonly IThingRepository _t;
        public Policy(IAnimalRepository a, IThingRepository t) { _a = a; _t = t; }
        public async Task EnsureUniqueAsync(int number)
        {
            if ((await _a.ListAsync()).Any(x => x.Number == number) ||
                (await _t.ListAsync()).Any(x => x.Number == number))
            {
                throw new InventoryConflictException(number);
            }
        }
    }
}
