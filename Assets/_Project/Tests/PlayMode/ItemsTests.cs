using EternalDefenders;
using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        // Guardian Angel id: 0
        ItemManager.Instance.AddItemByID(0);
        TowerController towerPrefab = Object.FindAnyObjectByType<TowerController>();
        Assert.IsTrue(towerPrefab != null);
        GuardianAngel angel = ItemManager.Instance.EquippedItems.OfType<GuardianAngel>().FirstOrDefault();
        Assert.IsTrue(angel != null);

        towerPrefab.Stats.ChangeStat(StatType.Health, -towerPrefab.Stats.GetStat(StatType.MaxHealth));
        yield return new WaitForSeconds(angel.ProtectionCooldown / 2.0f);
        Assert.IsTrue(towerPrefab.Stats.GetStat(StatType.Health) > 0);
        yield return null;
    }

    [UnityTest]
    public IEnumerator GuardianAngelTowerDestroyDuringCooldownTest()
    {
        // Guardian Angel id: 0
        ItemManager.Instance.AddItemByID(0);
        TowerController towerPrefab = Object.FindAnyObjectByType<TowerController>();
        Assert.IsTrue(towerPrefab != null);
        GuardianAngel angel = ItemManager.Instance.EquippedItems.OfType<GuardianAngel>().FirstOrDefault();
        Assert.IsTrue(angel != null);

        towerPrefab.Stats.ChangeStat(StatType.Health, -towerPrefab.Stats.GetStat(StatType.MaxHealth));
        yield return new WaitForSeconds(angel.ProtectionCooldown / 2.0f);
        towerPrefab.Stats.ChangeStat(StatType.Health, -towerPrefab.Stats.GetStat(StatType.MaxHealth));
        yield return new WaitForSeconds(angel.ProtectionCooldown / 2.0f);
        Assert.IsTrue(towerPrefab == null);
        yield return null;
    }

    [UnityTest]
    public IEnumerator GuardianAngelTowerDestroyAfterCooldownTest()
    {
        // Guardian Angel id: 0
        ItemManager.Instance.AddItemByID(0);
        TowerController towerPrefab = Object.FindAnyObjectByType<TowerController>();
        Assert.IsTrue(towerPrefab != null);
        GuardianAngel angel = ItemManager.Instance.EquippedItems.OfType<GuardianAngel>().FirstOrDefault();
        Assert.IsTrue(angel != null);

        towerPrefab.Stats.ChangeStat(StatType.Health, -towerPrefab.Stats.GetStat(StatType.MaxHealth));
        yield return new WaitForSeconds(angel.ProtectionCooldown + 1.0f);
        towerPrefab.Stats.ChangeStat(StatType.Health, -towerPrefab.Stats.GetStat(StatType.MaxHealth));
        yield return new WaitForSeconds(angel.ProtectionCooldown / 2.0f);
        Assert.IsTrue(towerPrefab.Stats.GetStat(StatType.Health) > 0);
        yield return null;
    }

    [UnityTest]
    public IEnumerator GuardianAngelDuplicationTest()
    {
        // Guardian Angel id: 0
        ItemManager.Instance.AddItemByID(0);
        TowerController towerPrefab = Object.FindAnyObjectByType<TowerController>();
        Assert.IsTrue(towerPrefab != null);
        GuardianAngel angel = ItemManager.Instance.EquippedItems.OfType<GuardianAngel>().FirstOrDefault();
        Assert.IsTrue(angel != null);

        towerPrefab.Stats.ChangeStat(StatType.Health, -towerPrefab.Stats.GetStat(StatType.MaxHealth));
        yield return new WaitForSeconds(angel.ProtectionCooldown / 2.0f);
        Assert.IsTrue(towerPrefab.Stats.GetStat(StatType.Health) > 0);
        ItemManager.Instance.AddItemByID(0);
        towerPrefab.Stats.ChangeStat(StatType.Health, -towerPrefab.Stats.GetStat(StatType.MaxHealth));
        yield return new WaitForSeconds(angel.ProtectionCooldown / 2.0f);
        Assert.IsTrue(towerPrefab.Stats.GetStat(StatType.Health) > 0);
        ItemManager.Instance.AddItemByID(0);
        towerPrefab.Stats.ChangeStat(StatType.Health, -towerPrefab.Stats.GetStat(StatType.MaxHealth));
        yield return new WaitForSeconds(angel.ProtectionCooldown / 2.0f);
        Assert.IsTrue(towerPrefab.Stats.GetStat(StatType.Health) > 0);
        yield return null;
    }

    [UnityTest]
    public IEnumerator HealthShotFullUseTest()
    {
        // Health Shot id: 1
        ItemManager.Instance.AddItemByID(1);
        PlayerController player = PlayerController.Instance;
        Assert.IsTrue(player != null);
        HealthShot healthShot = ItemManager.Instance.EquippedItems.OfType<HealthShot>().FirstOrDefault();
        Assert.IsTrue(healthShot != null);

        player.Stats.SetStat(StatType.Health, 1);
        int regenValue = Mathf.RoundToInt(0.015f * player.Stats.GetStat(StatType.MaxHealth));
        healthShot.Use();
        yield return new WaitForSeconds(10.5f);
        Assert.AreEqual(1 + regenValue * 10, player.Stats.GetStat(StatType.Health));
        yield return new WaitForSeconds(10);
        Assert.AreEqual(1 + regenValue * 20, player.Stats.GetStat(StatType.Health));
        yield return new WaitForSeconds(10);
        Assert.AreEqual(1 + regenValue * 20, player.Stats.GetStat(StatType.Health));
        yield return null;
    }

    [UnityTest]
    public IEnumerator HealthShotMaxHealthLimitTest()
    {
        // Health Shot id: 1
        ItemManager.Instance.AddItemByID(1);
        PlayerController player = PlayerController.Instance;
        Assert.IsTrue(player != null);
        HealthShot healthShot = ItemManager.Instance.EquippedItems.OfType<HealthShot>().FirstOrDefault();
        Assert.IsTrue(healthShot != null);

        player.Stats.SetStat(StatType.Health, (int)(player.Stats.GetStat(StatType.MaxHealth) * 0.9f));
        healthShot.Use();
        yield return new WaitForSeconds(21);
        Assert.AreEqual(player.Stats.GetStat(StatType.MaxHealth), player.Stats.GetStat(StatType.Health));
        yield return null;
    }

    // I don't know if we allow or not double use, for now I assume that we don't
    [UnityTest]
    public IEnumerator HealthShotDoubleUseTest()
    {
        // Health Shot id: 1
        ItemManager.Instance.AddItemByID(1);
        PlayerController player = PlayerController.Instance;
        Assert.IsTrue(player != null);
        HealthShot healthShot = ItemManager.Instance.EquippedItems.OfType<HealthShot>().FirstOrDefault();
        Assert.IsTrue(healthShot != null);

        player.Stats.SetStat(StatType.Health, 1);
        int regenValue = Mathf.RoundToInt(0.015f * player.Stats.GetStat(StatType.MaxHealth));
        healthShot.Use();
        yield return new WaitForSeconds(10);
        healthShot.Use();
        yield return new WaitForSeconds(11);
        Assert.AreEqual(1 + regenValue * 20, player.Stats.GetStat(StatType.Health));
        yield return null;
    }

    [UnityTest]
    public IEnumerator HealthShotUseAfterCooldownTest()
    {
        // Health Shot id: 1
        ItemManager.Instance.AddItemByID(1);
        PlayerController player = PlayerController.Instance;
        Assert.IsTrue(player != null);
        HealthShot healthShot = ItemManager.Instance.EquippedItems.OfType<HealthShot>().FirstOrDefault();
        Assert.IsTrue(healthShot != null);

        player.Stats.SetStat(StatType.Health, 1);
        int regenValue = Mathf.RoundToInt(0.015f * player.Stats.GetStat(StatType.MaxHealth));
        healthShot.Use();
        yield return new WaitForSeconds(21 + healthShot.CooldownDuration);
        healthShot.Use();
        yield return new WaitForSeconds(21);
        Assert.AreEqual(1 + regenValue * 40, player.Stats.GetStat(StatType.Health));
        yield return null;
    }

    [UnityTest]
    public IEnumerator UnfathomMaliceTest()
    {
        // Unfathom Malice id: 2
        PlayerController player = PlayerController.Instance;
        Assert.IsTrue(player != null);
        int playerBasicDmg = player.Stats.GetStat(StatType.Damage);
        ItemManager.Instance.AddItemByID(2);
        UnfathomMalice malice = ItemManager.Instance.EquippedItems.OfType<UnfathomMalice>().FirstOrDefault();
        Assert.IsTrue(malice != null);

        Assert.AreEqual(playerBasicDmg + 5, player.Stats.GetStat(StatType.Damage));
        yield return new WaitForSeconds(11);
        Assert.AreEqual(playerBasicDmg + 5 + 10, player.Stats.GetStat(StatType.Damage));
        yield return new WaitForSeconds(5);
        Assert.AreEqual(playerBasicDmg + 5, player.Stats.GetStat(StatType.Damage));
        yield return new WaitForSeconds(5);
        Assert.AreEqual(playerBasicDmg + 5 + 10, player.Stats.GetStat(StatType.Damage));
        yield return new WaitForSeconds(5);
        Assert.AreEqual(playerBasicDmg + 5, player.Stats.GetStat(StatType.Damage));
        yield return null;
    }

    [UnityTest]
    public IEnumerator UnfathomMaliceDuplicationTest()
    {
        // Unfathom Malice id: 2
        PlayerController player = PlayerController.Instance;
        Assert.IsTrue(player != null);
        int playerBasicDmg = player.Stats.GetStat(StatType.Damage);
        int duplicates = 3;
        for (int i = 0; i < duplicates; i++)
        {
            ItemManager.Instance.AddItemByID(2);
        }
        UnfathomMalice malice = ItemManager.Instance.EquippedItems.OfType<UnfathomMalice>().FirstOrDefault();
        Assert.IsTrue(malice != null);

        Assert.AreEqual(playerBasicDmg + 5 * duplicates, player.Stats.GetStat(StatType.Damage));
        yield return new WaitForSeconds(11);
        Assert.AreEqual(playerBasicDmg + 5 * duplicates + 10, player.Stats.GetStat(StatType.Damage));
        yield return new WaitForSeconds(5);
        Assert.AreEqual(playerBasicDmg + 5 * duplicates, player.Stats.GetStat(StatType.Damage));
        yield return new WaitForSeconds(5);
        Assert.AreEqual(playerBasicDmg + 5 * duplicates + 10, player.Stats.GetStat(StatType.Damage));
        yield return new WaitForSeconds(5);
        Assert.AreEqual(playerBasicDmg + 5 * duplicates, player.Stats.GetStat(StatType.Damage));
        yield return null;
    }

    [UnityTest]
    public IEnumerator EnergyCoreTest()
    {
        // Energy Core id: 3
        PlayerController player = PlayerController.Instance;
        Assert.IsTrue(player != null);
        int playerBasicSpeed = player.Stats.GetStat(StatType.Speed);
        ItemManager.Instance.AddItemByID(3);
        EnergyCore energy = ItemManager.Instance.EquippedItems.OfType<EnergyCore>().FirstOrDefault();
        Assert.IsTrue(energy != null);

        Assert.AreEqual(playerBasicSpeed + 5, player.Stats.GetStat(StatType.Speed));
        yield return new WaitForSeconds(10);
        Assert.AreEqual(playerBasicSpeed + 5, player.Stats.GetStat(StatType.Speed));
        yield return null;
    }

    [UnityTest]
    public IEnumerator EnergyCoreDuplicationTest()
    {
        // Energy Core id: 3
        PlayerController player = PlayerController.Instance;
        Assert.IsTrue(player != null);
        int playerBasicSpeed = player.Stats.GetStat(StatType.Speed);
        int duplicates = 3;
        for (int i = 0; i < duplicates; i++)
        {
            ItemManager.Instance.AddItemByID(3);
        }
        EnergyCore energy = ItemManager.Instance.EquippedItems.OfType<EnergyCore>().FirstOrDefault();
        Assert.IsTrue(energy != null);

        Assert.AreEqual(playerBasicSpeed + 5 * duplicates, player.Stats.GetStat(StatType.Speed));
        yield return new WaitForSeconds(10);
        Assert.AreEqual(playerBasicSpeed + 5 * duplicates, player.Stats.GetStat(StatType.Speed));
        yield return null;
    }

    [UnityTest]
    public IEnumerator NanoSpikeGauntletsTest()
    {
        // Nano-Spike Gauntlets id: 4
        PlayerController player = PlayerController.Instance;
        Assert.IsTrue(player != null);
        int playerBasicDmg = player.Stats.GetStat(StatType.Damage);
        ItemManager.Instance.AddItemByID(4);
        NanoSpikeGauntlets gauntlets = ItemManager.Instance.EquippedItems.OfType<NanoSpikeGauntlets>().FirstOrDefault();
        Assert.IsTrue(gauntlets != null);

        Assert.AreEqual(playerBasicDmg + 20, player.Stats.GetStat(StatType.Damage));
        yield return new WaitForSeconds(10);
        Assert.AreEqual(playerBasicDmg + 20, player.Stats.GetStat(StatType.Damage));
        yield return null;
    }

    [UnityTest]
    public IEnumerator NanoSpikeGauntletsDuplicationTest()
    {
        // Nano-Spike Gauntlets id: 4
        PlayerController player = PlayerController.Instance;
        Assert.IsTrue(player != null);
        int playerBasicDmg = player.Stats.GetStat(StatType.Damage);
        int duplicates = 3;
        for (int i = 0; i < duplicates; i++)
        {
            ItemManager.Instance.AddItemByID(4);
        }
        NanoSpikeGauntlets gauntlets = ItemManager.Instance.EquippedItems.OfType<NanoSpikeGauntlets>().FirstOrDefault();
        Assert.IsTrue(gauntlets != null);

        Assert.AreEqual(playerBasicDmg + 20 * duplicates, player.Stats.GetStat(StatType.Damage));
        yield return new WaitForSeconds(10);
        Assert.AreEqual(playerBasicDmg + 20 * duplicates, player.Stats.GetStat(StatType.Damage));
        yield return null;
    }
}
