namespace Zoo.Domain.Entities.Animals;
public sealed class Tiger : Predator
{
    public Tiger(string name, int number, int foodKgPerDay)
        : base(name, number, foodKgPerDay) { }
    public override string Species => "Tiger";
}