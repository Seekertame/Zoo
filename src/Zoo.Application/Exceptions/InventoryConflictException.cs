namespace Zoo.Application.Exceptions;

public sealed class InventoryConflictException : Exception
{
    public int Number { get; }
    public InventoryConflictException(int number)
        : base($"Inventory number already exists: {number}") => Number = number;
}
