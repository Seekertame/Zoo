namespace Zoo.Domain.Entities.Animals;
public sealed class Rabbit : Herbivore
{
    public Rabbit(string name, int number, int foodKgPerDay, int kindness)
        : base(name, number, foodKgPerDay, kindness) { }
    public override string Species => "Rabbit";
}