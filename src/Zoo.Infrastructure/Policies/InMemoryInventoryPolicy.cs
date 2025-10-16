using System.Linq;
using Zoo.Application.Exceptions;
using Zoo.Application.Interfaces;

namespace Zoo.Infrastructure.Policies;

public sealed class InMemoryInventoryPolicy(IAnimalRepository animals, IThingRepository things) : IInventoryPolicy
{
    private readonly IAnimalRepository _animals = animals;
    private readonly IThingRepository _things = things;

    public async Task EnsureUniqueAsync(int number)
    {
        if ((await _animals.ListAsync()).Any(a => a.Number == number)
         || (await _things.ListAsync()).Any(t => t.Number == number))
        {
            throw new InventoryConflictException(number);
        }
    }
}
