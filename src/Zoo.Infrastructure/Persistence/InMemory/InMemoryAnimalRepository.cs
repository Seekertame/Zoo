using Zoo.Application.Interfaces;
using Zoo.Domain.Entities.Animals;

namespace Zoo.Infrastructure.Persistence.InMemory;

public sealed class InMemoryAnimalRepository : IAnimalRepository
{
    private readonly List<Animal> _data = [];
    public Task AddAsync(Animal animal) { _data.Add(animal); return Task.CompletedTask; }
    public Task<IReadOnlyList<Animal>> ListAsync() => Task.FromResult((IReadOnlyList<Animal>)_data);
}
