using System;
using System.Collections;
using System.Collections.Generic;
using EternalDefenders;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class StatsModifiersTests
{
    [Test]
    public void StatInstantModifierFlatApplyTest()
    {
        int value = 23;
        int modifierValue = 10;
        Stats.Stat stat = new(value);

        InstantModifier modifier = ScriptableObject.CreateInstance<InstantModifier>();
        modifier.value = modifierValue;
        modifier.modifierType = ModifierType.Flat;
        modifier.statType = StatType.Health;

        stat.ApplyModifier(modifier);

        Assert.AreEqual(value + modifierValue, stat.CurrentValue);
    }

    [Test]
    public void StatInstantModifierPercentageApplyTest()
    {
        int value = 100;
        int modifierValue = 10;
        Stats.Stat stat = new(value);

        InstantModifier modifier = ScriptableObject.CreateInstance<InstantModifier>();
        modifier.value = modifierValue;
        modifier.modifierType = ModifierType.PercentAdd;
        modifier.statType = StatType.Health;

        stat.ApplyModifier(modifier);

        Assert.AreEqual(Mathf.RoundToInt(value * (modifierValue * 0.01f + 1)), stat.CurrentValue);
    }

    [Test]
    public void StatInstantModifierPercentageAndFlatApplyTest()
    {
        int value = 100;
        int flatModifierValue = 200;
        int percModifierValue = 10;
        Stats.Stat stat = new(value);

        InstantModifier modifierPerc = ScriptableObject.CreateInstance<InstantModifier>();
        modifierPerc.value = percModifierValue;
        modifierPerc.modifierType = ModifierType.PercentAdd;
        modifierPerc.statType = StatType.Health;
        stat.ApplyModifier(modifierPerc);

        InstantModifier modifierFlat = ScriptableObject.CreateInstance<InstantModifier>();
        modifierFlat.value = flatModifierValue;
        modifierFlat.modifierType = ModifierType.Flat;
        modifierFlat.statType = StatType.Health;
        stat.ApplyModifier(modifierFlat);

        Assert.AreEqual(Mathf.RoundToInt((value + flatModifierValue) * (percModifierValue * 0.01f + 1)), stat.CurrentValue);
    }

    [Test]
    public void StatInstantMultipleModifiersApplyTest()
    {
        int value = 100;
        int flatModifierValue = 200;
        int flatModifierValue2 = 15;
        int percModifierValue = 10;
        int percModifierValue2 = 25;
        Stats.Stat stat = new(value);

        InstantModifier modifierPerc = ScriptableObject.CreateInstance<InstantModifier>();
        modifierPerc.value = percModifierValue;
        modifierPerc.modifierType = ModifierType.PercentAdd;
        modifierPerc.statType = StatType.Health;
        stat.ApplyModifier(modifierPerc);

        InstantModifier modifierPerc2 = ScriptableObject.CreateInstance<InstantModifier>();
        modifierPerc2.value = percModifierValue2;
        modifierPerc2.modifierType = ModifierType.PercentAdd;
        modifierPerc2.statType = StatType.Health;
        stat.ApplyModifier(modifierPerc2);

        InstantModifier modifierFlat = ScriptableObject.CreateInstance<InstantModifier>();
        modifierFlat.value = flatModifierValue;
        modifierFlat.modifierType = ModifierType.Flat;
        modifierFlat.statType = StatType.Health;
        stat.ApplyModifier(modifierFlat);

        InstantModifier modifierFlat2 = ScriptableObject.CreateInstance<InstantModifier>();
        modifierFlat2.value = flatModifierValue2;
        modifierFlat2.modifierType = ModifierType.Flat;
        modifierFlat2.statType = StatType.Health;
        stat.ApplyModifier(modifierFlat2);

        Assert.AreEqual(Mathf.RoundToInt((value + flatModifierValue + flatModifierValue2) * 
            ((percModifierValue + percModifierValue2) * 0.01f + 1)), stat.CurrentValue);
    }

    [Test]
    public void StatsInstantMultipleModifiersApplyTest()
    {
        int value = 100;
        int flatModifierValue = 200;
        int flatModifierValue2 = 15;
        int percModifierValue = 10;
        int percModifierValue2 = 25;
        Dictionary<StatType, Stats.Stat> initStats = new() { };
        StatType firstStatType = (StatType)Enum.GetValues(typeof(StatType)).GetValue(0);

        initStats.Add(firstStatType, new Stats.Stat(value));
        Stats stats = new(initStats);

        InstantModifier modifierPerc = ScriptableObject.CreateInstance<InstantModifier>();
        modifierPerc.value = percModifierValue;
        modifierPerc.modifierType = ModifierType.PercentAdd;
        modifierPerc.statType = firstStatType;
        stats.ApplyModifier(modifierPerc);

        InstantModifier modifierPerc2 = ScriptableObject.CreateInstance<InstantModifier>();
        modifierPerc2.value = percModifierValue2;
        modifierPerc2.modifierType = ModifierType.PercentAdd;
        modifierPerc2.statType = firstStatType;
        stats.ApplyModifier(modifierPerc2);

        InstantModifier modifierFlat = ScriptableObject.CreateInstance<InstantModifier>();
        modifierFlat.value = flatModifierValue;
        modifierFlat.modifierType = ModifierType.Flat;
        modifierFlat.statType = firstStatType;
        stats.ApplyModifier(modifierFlat);

        InstantModifier modifierFlat2 = ScriptableObject.CreateInstance<InstantModifier>();
        modifierFlat2.value = flatModifierValue2;
        modifierFlat2.modifierType = ModifierType.Flat;
        modifierFlat2.statType = firstStatType;
        stats.ApplyModifier(modifierFlat2);

        Assert.AreEqual(Mathf.RoundToInt((value + flatModifierValue + flatModifierValue2) *
            ((percModifierValue + percModifierValue2) * 0.01f + 1)), stats.GetStat(firstStatType));
    }

    [Test]
    public void StatsInstantModifiersApplyMultipleStatTest()
    {
        int value = 100;
        int flatModifierValue = 200;
        int percModifierValue = 10;
        Dictionary<StatType, Stats.Stat> initStats = new() { };

        initStats.Add(StatType.Health, new Stats.Stat(value));
        initStats.Add(StatType.Damage, new Stats.Stat(value));
        Stats stats = new(initStats);

        InstantModifier modifierPerc = ScriptableObject.CreateInstance<InstantModifier>();
        modifierPerc.value = percModifierValue;
        modifierPerc.modifierType = ModifierType.PercentAdd;
        modifierPerc.statType = StatType.Health;
        stats.ApplyModifier(modifierPerc);

        InstantModifier modifierFlat = ScriptableObject.CreateInstance<InstantModifier>();
        modifierFlat.value = flatModifierValue;
        modifierFlat.modifierType = ModifierType.Flat;
        modifierFlat.statType = StatType.Damage;
        stats.ApplyModifier(modifierFlat);

        Assert.AreEqual(Mathf.RoundToInt(value * (percModifierValue * 0.01f + 1)), stats.GetStat(StatType.Health));
        Assert.AreEqual(value + flatModifierValue, stats.GetStat(StatType.Damage));
    }

    [UnityTest]
    public IEnumerator StatsInstantModifierLimitedTimeNoPersistTest()
    {
        int value = 100;
        int flatModifierValue = 200;
        float limitedTimeSec = 3;
        Dictionary<StatType, Stats.Stat> initStats = new() { };

        initStats.Add(StatType.Health, new Stats.Stat(value));
        Stats stats = new(initStats);

        InstantModifier modifier = ScriptableObject.CreateInstance<InstantModifier>();
        modifier.value = flatModifierValue;
        modifier.limitedDurationTime = limitedTimeSec;
        modifier.modifierType = ModifierType.Flat;
        modifier.statType = StatType.Health;
        stats.ApplyModifier(modifier);

        float startTime = Time.time;
        while (Time.time - startTime <= limitedTimeSec + 1)
        {
            stats.UpdateStatsModifiers(Time.deltaTime);
            if (Time.time - startTime < limitedTimeSec - 1f)
            {
                Assert.AreEqual(value + flatModifierValue, stats.GetStat(StatType.Health));
            }
            yield return null;
        }

        Assert.AreEqual(value, stats.GetStat(StatType.Health));
        yield return null;
    }

    [UnityTest]
    public IEnumerator StatsInstantModifierLimitedTimePersistTest()
    {
        int value = 100;
        int flatModifierValue = 200;
        float limitedTimeSec = 3;
        Dictionary<StatType, Stats.Stat> initStats = new() { };

        initStats.Add(StatType.Health, new Stats.Stat(value));
        Stats stats = new(initStats);

        InstantModifier modifier = ScriptableObject.CreateInstance<InstantModifier>();
        modifier.value = flatModifierValue;
        modifier.limitedDurationTime = limitedTimeSec;
        modifier.persistAfterFinish = true;
        modifier.modifierType = ModifierType.Flat;
        modifier.statType = StatType.Health;
        stats.ApplyModifier(modifier);

        float startTime = Time.time;
        while (Time.time - startTime <= limitedTimeSec + 1)
        {
            stats.UpdateStatsModifiers(Time.deltaTime);
            if (Time.time - startTime < limitedTimeSec - 1f)
            {
                Assert.AreEqual(value + flatModifierValue, stats.GetStat(StatType.Health));
            }
            yield return null;
        }

        Assert.AreEqual(value + flatModifierValue, stats.GetStat(StatType.Health));
        yield return null;
    }
}
