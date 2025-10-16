namespace Zoo.Domain.Entities.Animals;
public sealed class Tiger(string name, int number, int foodKgPerDay) : Predator(name, number, foodKgPerDay)
{
    public override string Species => "Tiger";
}