namespace Zoo.Domain.Entities.Animals;

public abstract class Herbivore : Animal
{
    public int Kindness { get; } // 0..10
    public bool IsContactZooAllowed => Kindness > 5;

    protected Herbivore(string name, int number, int foodKgPerDay, int kindness)
        : base(name, number, foodKgPerDay)
    {
        if (kindness is < 0 or > 10)
        {
            throw new ArgumentOutOfRangeException(nameof(kindness));
        }

        Kindness = kindness;
    }
}
