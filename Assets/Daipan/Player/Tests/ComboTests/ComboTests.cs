#nullable enable
using System.Collections.Generic;
using Daipan.Player.LevelDesign.Interfaces;
using Daipan.Player.LevelDesign.Scripts;
using Daipan.Player.Scripts;
using Daipan.Stream.Scripts;
using NUnit.Framework;


public sealed class ComboTests
{
    [Test]
    public void IncreaseComboTest()
    {
        var comboCounter = new ComboCounter();
        comboCounter.IncreaseCombo();
        Assert.AreEqual(1, comboCounter.ComboCount);
        comboCounter.IncreaseCombo();
        Assert.AreEqual(2, comboCounter.ComboCount);
    }

    [Test]
    public void ResetComboTest()
    {
        var comboCounter = new ComboCounter();
        comboCounter.IncreaseCombo();
        comboCounter.ResetCombo();
        Assert.AreEqual(0, comboCounter.ComboCount);

        comboCounter.IncreaseCombo();
        comboCounter.IncreaseCombo();
        comboCounter.ResetCombo();
        Assert.AreEqual(0, comboCounter.ComboCount);
    }

    [Test]
    public void MultiplyComboTest()
    {
        // Arrange
        var comboCounter = new ComboCounter();
        var comboMultiplier = new ComboMultiplier();

        // Act 1
        for (var i = 0; i < 10; i++) comboCounter.IncreaseCombo();
        // Assert 1
        Assert.AreEqual(10, comboCounter.ComboCount);
        Assert.AreEqual(1.10, comboMultiplier.CalculateComboMultiplier(comboCounter.ComboCount));

        // Act 2
        for (var i = 0; i < 10; i++) comboCounter.IncreaseCombo();
        // Assert 2
        Assert.AreEqual(20, comboCounter.ComboCount);
        Assert.AreEqual(1.20, comboMultiplier.CalculateComboMultiplier(comboCounter.ComboCount));

        // Act 3
        comboCounter.ResetCombo();
        for (var i = 0; i < 10; i++) comboCounter.IncreaseCombo();
        // Assert 3
        Assert.AreEqual(10, comboCounter.ComboCount);
        Assert.AreEqual(1.10, comboMultiplier.CalculateComboMultiplier(comboCounter.ComboCount));
    }

    [Test]
    public void ComboMultiplierWithIncreaseViewersTest()
    {
        // Arrange
        var comboCounter = new ComboCounter();
        var comboMultiplier = new ComboMultiplier();
        var viewerNumber = new ViewerNumber();
        
        // Act 1
        viewerNumber.IncreaseViewer(100);
        // Assert 1
        Assert.AreEqual(100, viewerNumber.Number);
        
        // Act 2
        for (var i = 0; i < 10; i++) comboCounter.IncreaseCombo();
        viewerNumber.IncreaseViewer(100);
        // Assert 2
        Assert.AreEqual(210, viewerNumber.Number);
    }
    
    [Test]
    public void ComboMultiplierWithDecreaseViewersTest()
    {
        // Arrange
        var comboCounter = new ComboCounter();
        var comboMultiplier = new ComboMultiplier();
        var viewerNumber = new ViewerNumber();
        
        // Act 1
        viewerNumber.IncreaseViewer(150);
        viewerNumber.DecreaseViewer(50);
        // Assert 1
        Assert.AreEqual(100, viewerNumber.Number);
        
        // Act 2
        for (var i = 0; i < 10; i++) comboCounter.IncreaseCombo();
        viewerNumber.DecreaseViewer(50);
        // Assert 2
        Assert.AreEqual(150, viewerNumber.Number);
    }

}