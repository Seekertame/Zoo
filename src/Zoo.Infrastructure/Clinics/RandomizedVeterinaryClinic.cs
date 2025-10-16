using Zoo.Application.Interfaces;
using Zoo.Domain.Entities.Animals;

namespace Zoo.Infrastructure.Clinics;

// Простейшее правило: травоядных допускаем, хищников — если "рацион" не слишком велик.
public sealed class RandomizedVeterinaryClinic : IVeterinaryClinic
{
    public AdmissionDecision Inspect(Animal candidate)
    {
        if (candidate is Herbivore)
        {
            return AdmissionDecision.Accept;
        }

        return candidate.FoodKgPerDay <= 7 ? AdmissionDecision.Accept : AdmissionDecision.Reject;
    }
}
