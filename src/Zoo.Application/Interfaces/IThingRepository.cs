using Zoo.Domain.Entities.Things;

namespace Zoo.Application.Interfaces;
public interface IThingRepository
{
    Task AddAsync(Thing thing);
    Task<IReadOnlyList<Thing>> ListAsync();
}