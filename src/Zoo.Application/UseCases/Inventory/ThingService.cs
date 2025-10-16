using Zoo.Application.Interfaces;
using Zoo.Domain.Entities.Things;

namespace Zoo.Application.UseCases.Inventory;

public sealed class ThingService : IThingService
{
    private readonly IThingRepository _things;
    private readonly IInventoryPolicy _policy;
    private readonly IUnitOfWork _uow;

    public ThingService(IThingRepository things, IInventoryPolicy policy, IUnitOfWork uow)
    { _things = things; _policy = policy; _uow = uow; }

    public async Task AddAsync(Thing thing)
    {
        await _policy.EnsureUniqueAsync(thing.Number);
        await _things.AddAsync(thing);
        await _uow.SaveChangesAsync();
    }
}
