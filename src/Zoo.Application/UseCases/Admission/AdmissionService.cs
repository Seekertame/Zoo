using Zoo.Application.Interfaces;
using Zoo.Domain.Entities.Animals;

namespace Zoo.Application.UseCases.Admission;

public sealed class AdmissionService(IVeterinaryClinic clinic, IAnimalRepository animals, IUnitOfWork uow, IInventoryPolicy policy) : IAdmissionService
{
    private readonly IVeterinaryClinic _clinic = clinic;
    private readonly IAnimalRepository _animals = animals;
    private readonly IUnitOfWork _uow = uow;
    private readonly IInventoryPolicy _policy = policy;

    public async Task<AdmissionDecision> AdmitAsync(Animal candidate)
    {
        var decision = _clinic.Inspect(candidate);
        if (decision == AdmissionDecision.Accept)
        {
            await _policy.EnsureUniqueAsync(candidate.Number);
            await _animals.AddAsync(candidate);
            await _uow.SaveChangesAsync();
        }
        return decision;
    }
}
