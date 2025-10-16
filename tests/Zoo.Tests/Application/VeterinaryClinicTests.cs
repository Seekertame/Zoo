using FluentAssertions;
using Zoo.Infrastructure.Clinics;
using Zoo.Domain.Entities.Animals;
// using Xunit;

namespace Zoo.Tests.Application;

public class VeterinaryClinicTests
{
    [Fact]
    public void Herbivore_is_accepted()
    {
        var clinic = new RandomizedVeterinaryClinic();
        clinic.Inspect(new Rabbit("Kind", 10, 1, 9)).Should().Be(Zoo.Application.Interfaces.AdmissionDecision.Accept);
    }

    [Theory]
    [InlineData(7, true)]
    [InlineData(8, false)]
    public void Predator_is_accepted_only_if_food_le_7(int food, bool accepted)
    {
        var clinic = new RandomizedVeterinaryClinic();
        var dec = clinic.Inspect(new Tiger("Sher", 99, food));
        (dec == Zoo.Application.Interfaces.AdmissionDecision.Accept).Should().Be(accepted);
    }
}
