using FluentAssertions;
using Padel.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using Padel.Application.Mappers;
using Padel.Domain.Models;

namespace Padel.Tests.Models;

public class SeasonModelTests
{
    [Fact]
    public void CreateNewSeason_Creates()
    {
        // Arrange
        var amountOfMatches = 5;
        var startDate = DateTime.UtcNow.AddDays(1);
        var title = "Summer League";
        var dayOfWeek = DayOfWeek.Monday;

        // Act
        var season = new Season(amountOfMatches, startDate, title, dayOfWeek);

        // Assert
        season.AmountOfMatches.Should().Be(amountOfMatches);
        season.StartDate.Should().Be(startDate);
        season.Title.Should().Be(title);
        season.DayOfWeek.Should().Be(dayOfWeek);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void CreateNewSeason_EmptyOrNullTitle_Throws(string title)
    {
        // Arrange
        var amountOfMatches = 5;
        var startDate = DateTime.UtcNow.AddDays(1);
        var dayOfWeek = DayOfWeek.Monday;

        // Act & Assert
        FluentActions
            .Invoking(() => new Season(amountOfMatches, startDate, title, dayOfWeek))
            .Should()
            .Throw<ArgumentException>();
        
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CreateNewSeason_InvalidAmountOfMatches_Throws(int amountOfMatches)
    {
        // Arrange
        var startDate = DateTime.UtcNow.AddDays(1);
        var title = "Summer League";
        var dayOfWeek = DayOfWeek.Monday;

        // Act & Assert
        FluentActions
            .Invoking(() => new Season(amountOfMatches, startDate, title, dayOfWeek))
            .Should()
            .Throw<ArgumentException>();
    }


    [Fact]
    public void ToEntity_ConvertsSeasonToSeasonEntity()
    {
        // Arrange
        var amountOfMatches = 5;
        var startDate = DateTime.UtcNow.AddDays(1);
        var title = "Summer League";
        var dayOfWeek = DayOfWeek.Monday;
        var season = new Season(amountOfMatches, startDate, title, dayOfWeek) { Id = 1 };

        // Act
        var seasonEntity = season.ToEntity();

        // Assert
        seasonEntity.Id.Should().Be(season.Id);
        seasonEntity.AmountOfMatches.Should().Be(season.AmountOfMatches);
        seasonEntity.DayOfWeek.Should().Be(season.DayOfWeek);
        seasonEntity.StartDate.Should().Be(season.StartDate);
        seasonEntity.Title.Should().Be(season.Title);
    }

    [Fact]
    public void FromEntity_ConvertsSeasonEntityToSeason()
    {
        // Arrange
        var seasonEntity = new SeasonEntity
        {
            Id = 2,
            AmountOfMatches = 5,
            StartDate = DateTime.UtcNow.AddDays(1),
            Title = "Winter League",
            DayOfWeek = DayOfWeek.Wednesday
        };

        // Act
        var season = seasonEntity.FromEntity();

        // Assert
        season.Id.Should().Be(seasonEntity.Id);
        season.AmountOfMatches.Should().Be(seasonEntity.AmountOfMatches);
        season.StartDate.Should().Be(seasonEntity.StartDate);
        season.Title.Should().Be(seasonEntity.Title);
        season.DayOfWeek.Should().Be(seasonEntity.DayOfWeek);
    }

    [Fact]
    public void ToEntity_WithoutId_SetsDefaultIdInEntity()
    {
        // Arrange
        var season = new Season(5, DateTime.UtcNow.AddDays(1), "Spring League", DayOfWeek.Friday);

        // Act
        var seasonEntity = season.ToEntity();

        // Assert
        seasonEntity.Id.Should().Be(0); 
    }
}
