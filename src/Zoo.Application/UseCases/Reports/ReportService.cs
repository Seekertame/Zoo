using Zoo.Application.Interfaces;
using Zoo.Domain.Entities.Animals;

namespace Zoo.Application.UseCases.Reports;

public sealed class ReportService(IAnimalRepository animals, IThingRepository things) : IReportService
{
    private readonly IAnimalRepository _animals = animals;
    private readonly IThingRepository _things = things;

    public async Task<int> GetTotalFoodKgPerDayAsync()
        => (await _animals.ListAsync()).Sum(a => a.FoodKgPerDay);

    public async Task<IReadOnlyList<Herbivore>> GetContactZooAsync()
        => [.. (await _animals.ListAsync()).OfType<Herbivore>().Where(h => h.IsContactZooAllowed)];

    public async Task<IReadOnlyList<(string Title, int Number)>> GetInventoryAsync()
    {
        var animals = (await _animals.ListAsync()).Select(a => ($"{a.Species} {a.Name}", a.Number));
        var things = (await _things.ListAsync()).Select(t => ($"{t.Title}", t.Number));
        return animals.Concat(things).ToList();
    }
}
