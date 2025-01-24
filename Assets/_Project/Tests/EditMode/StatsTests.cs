using System;
using System.Collections;
using EternalDefenders;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor.Graphs;

public class StatsTests
{
    [Test]
    public void StatInitTest()
    {
        int value = 23;
        Stats.Stat stat = new(value);
        Assert.AreEqual(value, stat.BaseValue);
        Assert.AreEqual(value, stat.CurrentValue);
    }

    [Test]
    public void StatsAllInitTest()
    {
        int value = 100;
        Dictionary<StatType, Stats.Stat> initStats = new() { };
        foreach (StatType statType in Enum.GetValues(typeof(StatType)))
        {
            initStats.Add(statType, new Stats.Stat(value));
        }

        Stats stats = new(initStats);

        foreach (StatType statType in Enum.GetValues(typeof(StatType)))
        {
            Assert.AreEqual(value, stats.GetStat(statType));
        }
    }

    [Test]
    public void StatsSingleInitTest()
    {
        int value = 100;
        Dictionary<StatType, Stats.Stat> initStats = new() { };
        StatType firstStatType = (StatType)Enum.GetValues(typeof(StatType)).GetValue(0);
        initStats.Add(firstStatType, new Stats.Stat(value));

        Stats stats = new(initStats);

        Assert.AreEqual(value, stats.GetStat(firstStatType));
        foreach (StatType statType in Enum.GetValues(typeof(StatType)))
        {
            if (statType != firstStatType)
            {
                Assert.AreEqual(0, stats.GetStat(statType));
            }
        }
    }

    [Test]
    public void HasStatTest()
    {
        int value = 100;
        Dictionary<StatType, Stats.Stat> initStats = new() { };
        StatType firstStatType = (StatType)Enum.GetValues(typeof(StatType)).GetValue(0);
        initStats.Add(firstStatType, new Stats.Stat(value));

        Stats stats = new(initStats);

        Assert.AreEqual(true, stats.HasStat(firstStatType));
        foreach (StatType statType in Enum.GetValues(typeof(StatType)))
        {
            if (statType != firstStatType)
            {
                Assert.AreEqual(false, stats.HasStat(statType));
            }
        }
    }

    [Test]
    public void ChangeStatTest()
    {
        int value = 100;
        int add = 14;
        int remove = -27;
        Dictionary<StatType, Stats.Stat> initStats = new() { };
        StatType firstStatType = (StatType)Enum.GetValues(typeof(StatType)).GetValue(0);
        initStats.Add(firstStatType, new Stats.Stat(value));

        Stats stats = new(initStats);

        stats.ChangeStat(firstStatType, add);
        Assert.AreEqual(value + add, stats.GetStat(firstStatType));
        stats.ChangeStat(firstStatType, remove);
        Assert.AreEqual(value + add + remove, stats.GetStat(firstStatType));
    }

    [Test]
    public void SetStatTest()
    {
        int value = 100;
        int newValue = 115;
        Dictionary<StatType, Stats.Stat> initStats = new() { };
        StatType firstStatType = (StatType)Enum.GetValues(typeof(StatType)).GetValue(0);
        initStats.Add(firstStatType, new Stats.Stat(value));

        Stats stats = new(initStats);

        stats.SetStat(firstStatType, newValue);
        Assert.AreEqual(newValue, stats.GetStat(firstStatType));
    }
}
