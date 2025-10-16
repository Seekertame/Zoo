using Zoo.Domain.Entities.Animals;

namespace Zoo.Application.Interfaces;
public interface IAdmissionService
{
    Task<AdmissionDecision> AdmitAsync(Animal candidate);
}