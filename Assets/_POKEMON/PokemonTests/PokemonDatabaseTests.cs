using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PokemonDatabaseTests
{
    [Test]
    [TestCase("Grass", 97)]
    [TestCase("Fire", 64)]
    [TestCase("Water", 131)]
    public void GetRightCountForTypeCount(string type, int expected)
    {
        // Arrange
        var db = new GameObject("test").AddComponent<DataReader>().Init();

        // Act
        var result = db.PokemonByType(type);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.AreEqual(expected, result.Count());

        // (Clean)
        GameObject.DestroyImmediate(db.gameObject);
    }

    [Test]
    [TestCase(25, 320)]
    [TestCase(129, 200)]
    [TestCase(493, 720)]
    public void GetPokemonPower(int id, int expected)
    {
        // Arrange
        var db = new GameObject("test").AddComponent<DataReader>().Init();

        // Act
        var result = db.GetPokemonById(id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.AreEqual(expected, result.statbase.BasePower);

        // (Clean)
        GameObject.DestroyImmediate(db.gameObject);
    }

    [Test]
    public void GetTop10PokemonIdByPower()
    {
        // Arrange
        var db = new GameObject("test").AddComponent<DataReader>().Init();

        // Act
        var result = db.GetTopPokemonByBasePower(10);
        List<int> expected = new() { 493, 150, 249, 250, 384, 483, 484, 487, 643, 644, };

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.AreEqual(result.Count, 10);
        Assert.That(result, Is.EquivalentTo(expected));

        // (Clean)
        GameObject.DestroyImmediate(db.gameObject);
    }

    [Test]
    public void GetDown10PokemonIdByPower()
    {
        // Arrange
        var db = new GameObject("test").AddComponent<DataReader>().Init();

        // Act
        var result = db.GetDownPokemonByBasePower(10);
        List<int> expected = new() { 746, 191, 298, 401, 10, 13, 265, 280, 129, 349, };

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.AreEqual(result.Count, 10);
        Assert.That(result, Is.EquivalentTo(expected));

        // (Clean)
        GameObject.DestroyImmediate(db.gameObject);
    }

    [Test]
    public void GetTop5PokemonIdByPowerWithPower()
    {
        // Arrange
        var db = new GameObject("test").AddComponent<DataReader>().Init();

        // Act
        var result = db.DecoratePokemonIdWithPower(db.GetTopPokemonByBasePower(5)).ToList();
        List<(int,int)> expected = new() 
        { 
            (493, 720),
            (150, 680),
            (249, 680),
            (250, 680),
            (384, 680),
        };

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.AreEqual(result.Count(), 5);
        Assert.That(result, Is.EquivalentTo(expected));

        // (Clean)
        GameObject.DestroyImmediate(db.gameObject);
    }

}
