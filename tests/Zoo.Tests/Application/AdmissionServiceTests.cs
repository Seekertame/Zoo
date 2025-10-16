using System.Threading.Tasks;
using FluentAssertions;
using Zoo.Application.Interfaces;
using Zoo.Application.UseCases.Admission;
using Zoo.Domain.Entities.Animals;

namespace Zoo.Tests.Application;

public class AdmissionServiceTests
{
    [Fact]
    public async Task AdmitAsync_accepts_and_persists_on_positive_decision()
    {
        var animals = new ListAnimalRepo();
        var uow = new NoopUow();
        var clinic = new FixedClinic(AdmissionDecision.Accept);
        var policy = new NoopPolicy();

        var sut = new AdmissionService(clinic, animals, uow, policy);

        var rabbit = new Rabbit("Bunny", 1001, 1, 8);
        var decision = await sut.AdmitAsync(rabbit);

        decision.Should().Be(AdmissionDecision.Accept);
        (await animals.ListAsync()).Should().ContainSingle(a => a.Number == 1001);
    }

    [Fact]
    public async Task AdmitAsync_rejects_and_does_not_persist_on_negative_decision()
    {
        var animals = new ListAnimalRepo();
        var uow = new NoopUow();
        var clinic = new FixedClinic(AdmissionDecision.Reject);
        var policy = new NoopPolicy();

        var sut = new AdmissionService(clinic, animals, uow, policy);

        var tiger = new Tiger("Sher", 2001, 10);
        var decision = await sut.AdmitAsync(tiger);

        decision.Should().Be(AdmissionDecision.Reject);
        (await animals.ListAsync()).Should().BeEmpty();
    }

    // ===== fakes =====
    private sealed class FixedClinic(AdmissionDecision d) : IVeterinaryClinic
    {
        private readonly AdmissionDecision _d = d;

        public AdmissionDecision Inspect(Animal candidate) => _d;
    }

    private sealed class ListAnimalRepo : IAnimalRepository
    {
        private readonly List<Animal> _list = [];
        public Task AddAsync(Animal animal) { _list.Add(animal); return Task.CompletedTask; }
        public Task<IReadOnlyList<Animal>> ListAsync() => Task.FromResult((IReadOnlyList<Animal>)_list);
    }

    private sealed class NoopUow : IUnitOfWork
    {
        public Task SaveChangesAsync() => Task.CompletedTask;
    }

    public sealed class NoopPolicy : IInventoryPolicy
    {
        public Task EnsureUniqueAsync(int number) => Task.CompletedTask;
    }
}
