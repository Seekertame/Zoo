using Zoo.Domain.Entities.Things;

namespace Zoo.Application.Interfaces;

public interface IThingService
{
    Task AddAsync(Thing thing);
}
