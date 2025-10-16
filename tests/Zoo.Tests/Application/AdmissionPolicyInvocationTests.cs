//using System.Threading.Tasks;
using FluentAssertions;
using Zoo.Application.Interfaces;
using Zoo.Application.UseCases.Admission;
using Zoo.Domain.Entities.Animals;
//using Xunit;

namespace Zoo.Tests.Application;

public class AdmissionPolicyInvocationTests
{
    [Fact]
    public async Task Policy_is_called_when_clinic_accepts()
    {
        var animals = new MemAnimals();
        var uow = new NoopUow();
        var clinic = new FixedClinic(AdmissionDecision.Accept);
        var policy = new SpyPolicy();

        var sut = new AdmissionService(clinic, animals, uow, policy);

        var r = new Rabbit("Bunny", 1001, 1, 8);
        _ = await sut.AdmitAsync(r);

        policy.Called.Should().BeTrue();
        policy.LastNumber.Should().Be(1001);
    }

    [Fact]
    public async Task Policy_is_not_called_when_clinic_rejects()
    {
        var animals = new MemAnimals();
        var uow = new NoopUow();
        var clinic = new FixedClinic(AdmissionDecision.Reject);
        var policy = new SpyPolicy();

        var sut = new AdmissionService(clinic, animals, uow, policy);

        var t = new Tiger("Sher", 2001, 10);
        _ = await sut.AdmitAsync(t);

        policy.Called.Should().BeFalse();
        (await animals.ListAsync()).Should().BeEmpty();
    }

    // ---- fakes/spies ----
    private sealed class MemAnimals : IAnimalRepository
    {
        private readonly List<Animal> _list = new();
        public Task AddAsync(Animal a) { _list.Add(a); return Task.CompletedTask; }
        public Task<IReadOnlyList<Animal>> ListAsync() => Task.FromResult((IReadOnlyList<Animal>)_list);
    }
    private sealed class NoopUow : IUnitOfWork { public Task SaveChangesAsync() => Task.CompletedTask; }
    private sealed class FixedClinic(AdmissionDecision d) : IVeterinaryClinic
    {
        public AdmissionDecision Inspect(Animal _) => d;
    }
    private sealed class SpyPolicy : IInventoryPolicy
    {
        public bool Called { get; private set; }
        public int LastNumber { get; private set; }
        public Task EnsureUniqueAsync(int number) { Called = true; LastNumber = number; return Task.CompletedTask; }
    }
}
