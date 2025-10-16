using System.Linq;
using Zoo.Application.Exceptions;
using Zoo.Application.Interfaces;

namespace Zoo.Infrastructure.Policies;

public sealed class InMemoryInventoryPolicy : IInventoryPolicy
{
    private readonly IAnimalRepository _animals;
    private readonly IThingRepository _things;

    public InMemoryInventoryPolicy(IAnimalRepository animals, IThingRepository things)
    { _animals = animals; _things = things; }

    public async Task EnsureUniqueAsync(int number)
    {
        if ((await _animals.ListAsync()).Any(a => a.Number == number)
         || (await _things.ListAsync()).Any(t => t.Number == number))
        {
            throw new InventoryConflictException(number);
        }
    }
}
