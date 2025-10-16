using Zoo.Domain.Entities.Animals;

namespace Zoo.Application.Interfaces;
public interface IReportService
{
    Task<int> GetTotalFoodKgPerDayAsync();
    Task<IReadOnlyList<Herbivore>> GetContactZooAsync(); // Kindness > 5
    Task<IReadOnlyList<(string Title, int Number)>> GetInventoryAsync();
}