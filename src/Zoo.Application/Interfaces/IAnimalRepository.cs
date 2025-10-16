using Zoo.Domain.Entities.Animals;

namespace Zoo.Application.Interfaces;
public interface IAnimalRepository
{
    Task AddAsync(Animal animal);
    Task<IReadOnlyList<Animal>> ListAsync();
}