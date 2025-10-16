using Zoo.Application.Interfaces;
using Zoo.Domain.Entities.Things;

namespace Zoo.Infrastructure.Persistence.InMemory;

public sealed class InMemoryThingRepository : IThingRepository
{
    private readonly List<Thing> _data = [];
    public Task AddAsync(Thing thing) { _data.Add(thing); return Task.CompletedTask; }
    public Task<IReadOnlyList<Thing>> ListAsync() => Task.FromResult((IReadOnlyList<Thing>)_data);
}
