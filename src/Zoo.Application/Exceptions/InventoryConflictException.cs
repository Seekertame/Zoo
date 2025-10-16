namespace Zoo.Application.Exceptions;

public sealed class InventoryConflictException(int number) : Exception($"Inventory number already exists: {number}")
{
    public int Number { get; } = number;
}
