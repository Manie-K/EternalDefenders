using EternalDefenders;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class ItemsTests
{
    // Runs before each test
    [UnitySetUp]
    public IEnumerator Setup()
    {
        SceneManager.LoadScene("TestingWorld");
        // Wait for the scene to load
        yield return null;
    }

    // Runs after each test
    [UnityTearDown]
    public IEnumerator Teardown()
    {
        // Unload the testing scene after test
        SceneManager.LoadScene("EmptyTestingWorld");
        // Wait for the scene to load
        yield return null;
    }

    [UnityTest]
    public IEnumerator GuardianAngelProtectionTest()
    {
        TowerController towerPrefab = Object.FindAnyObjectByType<TowerController>();
        Assert.IsTrue(towerPrefab != null);
        towerPrefab.Stats.ChangeStat(StatType.Health, -towerPrefab.Stats.GetStat(StatType.MaxHealth));
        yield return new WaitForSeconds(2);
        Assert.IsTrue(towerPrefab.Stats.GetStat(StatType.Health) > 0);
        yield return null;
    }

    [UnityTest]
    public IEnumerator GuardianAngelTowerDestroyDuringCooldownTest()
    {
        TowerController towerPrefab = Object.FindAnyObjectByType<TowerController>();
        Assert.IsTrue(towerPrefab != null);
        towerPrefab.Stats.ChangeStat(StatType.Health, -towerPrefab.Stats.GetStat(StatType.MaxHealth));
        yield return new WaitForSeconds(2);
        towerPrefab.Stats.ChangeStat(StatType.Health, -towerPrefab.Stats.GetStat(StatType.MaxHealth));
        yield return new WaitForSeconds(2);
        Assert.IsTrue(towerPrefab == null);
        yield return null;
    }

    [UnityTest]
    public IEnumerator GuardianAngelTowerDestroyAfterCooldownTest()
    {
        TowerController towerPrefab = Object.FindAnyObjectByType<TowerController>();
        Assert.IsTrue(towerPrefab != null);
        towerPrefab.Stats.ChangeStat(StatType.Health, -towerPrefab.Stats.GetStat(StatType.MaxHealth));
        yield return new WaitForSeconds(31);
        towerPrefab.Stats.ChangeStat(StatType.Health, -towerPrefab.Stats.GetStat(StatType.MaxHealth));
        yield return new WaitForSeconds(2);
        Assert.IsTrue(towerPrefab.Stats.GetStat(StatType.Health) > 0);
        yield return null;
    }
}
