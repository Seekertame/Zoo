using System;
using FluentAssertions;
using Zoo.Domain.Entities.Animals;
using Zoo.Domain.Entities.Things;
using Xunit;

namespace Zoo.Tests.Domain;

public class AnimalValidationTests
{
    [Fact]
    public void Animal_should_throw_if_number_is_non_positive()
    {
        Action act = () => new Rabbit("Bunny", number: 0, foodKgPerDay: 1, kindness: 6);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Animal_should_throw_if_food_is_negative()
    {
        Action act = () => new Tiger("Tigra", number: 1001, foodKgPerDay: -1);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(11)]
    public void Herbivore_should_throw_if_kindness_out_of_range(int invalid)
    {
        Action act = () => new Monkey("Abu", number: 1002, foodKgPerDay: 2, kindness: invalid);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(5, false)]
    [InlineData(6, true)]
    [InlineData(10, true)]
    public void IsContactZooAllowed_depends_on_kindness(int kindness, bool expected)
    {
        var r = new Rabbit("Bunny", 123, 1, kindness);
        r.IsContactZooAllowed.Should().Be(expected);
    }

    [Fact]
    public void Thing_requires_positive_inventory_number()
    {
        Action act = () => new Table("Стол", number: 0);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}
