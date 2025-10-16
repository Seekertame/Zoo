using Zoo.Domain.Abstractions;

namespace Zoo.Domain.Entities.Things;
public abstract class Thing : IInventory
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Title { get; }
    public int Number { get; }
    protected Thing(string title, int number)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title required");
        }

        if (number <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(number));
        }

        Title = title; Number = number;
    }
}