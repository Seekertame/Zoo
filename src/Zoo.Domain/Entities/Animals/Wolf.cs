namespace Zoo.Domain.Entities.Animals;
public sealed class Wolf(string name, int number, int foodKgPerDay) : Predator(name, number, foodKgPerDay)
{
    public override string Species => "Wolf";
}