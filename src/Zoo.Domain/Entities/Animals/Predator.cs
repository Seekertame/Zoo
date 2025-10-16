namespace Zoo.Domain.Entities.Animals;
public abstract class Predator : Animal
{
    protected Predator(string name, int number, int foodKgPerDay)
        : base(name, number, foodKgPerDay) { }
}
