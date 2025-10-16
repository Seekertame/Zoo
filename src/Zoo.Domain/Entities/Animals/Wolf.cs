namespace Zoo.Domain.Entities.Animals;
public sealed class Wolf : Predator
{
    public Wolf(string name, int number, int foodKgPerDay)
        : base(name, number, foodKgPerDay) { }
    public override string Species => "Wolf";
}