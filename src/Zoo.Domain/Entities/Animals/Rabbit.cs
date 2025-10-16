namespace Zoo.Domain.Entities.Animals;
public sealed class Rabbit(string name, int number, int foodKgPerDay, int kindness) : Herbivore(name, number, foodKgPerDay, kindness)
{
    public override string Species => "Rabbit";
}