namespace Zoo.Domain.Entities.Animals;
public sealed class Monkey : Herbivore
{
    public Monkey(string name, int number, int foodKgPerDay, int kindness)
        : base(name, number, foodKgPerDay, kindness) { }
    public override string Species => "Monkey";
}