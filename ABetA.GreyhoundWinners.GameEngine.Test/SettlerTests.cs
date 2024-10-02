namespace AbetA.GreyhoundWinners.GameEngine.Test;

using ABetA.GreyhoundWinners.GameEngine;

public class Tests
{
    [Test]
    public void SettleCatchAMatchMarketWhenGivenAFullTrapsResultOnlySettlesAFullTrapsMarket()
    {
        // Arrange
        
        var settler = new Settler();
        
        // Act

        var result = settler.SettleCatchAMatchMarket([1, 1, 1, 1, 2, 2]).ToList();

        // Assert
        
        Assert.That(result.Count(), Is.EqualTo(1));
        Assert.That(result.First().Selection, Is.EqualTo("Full Traps"));
    }
}