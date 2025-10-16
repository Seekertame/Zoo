using Zoo.Domain.Entities.Animals;

namespace Zoo.Application.Interfaces;
public enum AdmissionDecision { Accept, Reject }
public interface IVeterinaryClinic
{
    AdmissionDecision Inspect(Animal candidate);
}