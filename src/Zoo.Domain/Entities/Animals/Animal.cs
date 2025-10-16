using Zoo.Domain.Abstractions;

namespace Zoo.Domain.Entities.Animals;

public abstract class Animal : IAlive, IInventory
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; }
    public int Number { get; }
    public int FoodKgPerDay { get; }

    protected Animal(string name, int number, int foodKgPerDay)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name required");
        }

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(number);

        ArgumentOutOfRangeException.ThrowIfNegative(foodKgPerDay);

        Name = name; Number = number; FoodKgPerDay = foodKgPerDay;
    }

    public abstract string Species { get; }
}
