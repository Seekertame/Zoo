namespace Zoo.Application.Interfaces;

public interface IInventoryPolicy
{
    Task EnsureUniqueAsync(int number);
}
