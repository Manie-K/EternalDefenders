using System;
using System.Collections.Generic;
using PlasticGui.WorkspaceWindow.PendingChanges;

namespace EternalDefenders
{
    public class Stats
    {
        public class Stat
        {
            public int BaseValue
            {
                get => _baseValue;
                set
                {
                    _baseValue = value;
                    _isDirty = true;
                }
            }
            public int CurrentValue
            {
                get
                {
                    if(_isDirty)
                    {
                        CalculateStat();
                    }
                    return _currentValue;
                }
                set
                {
                    _currentValue = value;
                    _isDirty = true;
                }
            }
        
            readonly List<Modifier> _modifiers;
            int _currentValue;
            int _baseValue;
            bool _isDirty;
            
            public Stat(int baseValue)
            {
                BaseValue = baseValue;
                CurrentValue = baseValue;
                _modifiers = new List<Modifier>();
            }

            public void UpdateModifiers(float dt)
            {
                _isDirty = false;
                _modifiers.ForEach(m =>
                {
                    m.UpdateModifier(dt);
                    if(m.IsDirty)
                    {
                        _isDirty = true;
                    }
                    if(m.IsFinished)
                    {
                        RemoveModifier(m);
                    }
                });
            }

            public void ApplyModifier(Modifier modifier)
            {
                modifier.InitModifer();
                _modifiers.Add(modifier);
                CalculateStat();
                modifier.SetClean();
            }
            
            void RemoveModifier(Modifier modifier)
            {
                //TODO: change value back to original ???
                _modifiers.Remove(modifier);
                _isDirty = true;
            }

            void CalculateStat()
            {
                int value = _baseValue;
                foreach(var mod in _modifiers)
                {
                    if(mod.modifierType == ModifierType.Flat)
                    {
                        value += mod.value;
                    }
                }

                int percentage = 0;
                foreach(var mod in _modifiers)
                {
                    if(mod.modifierType == ModifierType.PercentAdd)
                    {
                        percentage += mod.value;
                    }    
                }
                value *= (percentage / 100);
                
                _currentValue = value;
                _isDirty = false;
            }
        }
        
        readonly Dictionary<StatType, Stat> _stats;
        
        public Stats(Dictionary<StatType, Stat> stats)
        {
            _stats = new Dictionary<StatType, Stat>(stats); //shallow copy
        }
        
        public bool HasStat(StatType statType) => _stats.ContainsKey(statType);
        public int GetStat(StatType statType)
        {
            if(!HasStat(statType))
            {
                return 0;
            }
            EnforceStatsDependencies(statType);
            return _stats[statType].CurrentValue;
        }

        void EnforceStatsDependencies(StatType statType)
        {
            if(statType == StatType.Health)
            {
                if(HasStat(StatType.MaxHealth) && GetStat(StatType.Health) > GetStat(StatType.MaxHealth))
                {
                    SetStat(StatType.Health, GetStat(StatType.MaxHealth));
                }
            }
            else if(statType == StatType.Shield)
            {
                if(!HasStat(StatType.MaxShield)&& GetStat(StatType.Shield) > GetStat(StatType.MaxShield))
                {
                    SetStat(StatType.Shield, GetStat(StatType.MaxShield));
                }
            }
        }

        public void ChangeStat(StatType statType, int value)
        {
            if(!HasStat(statType))
            {
                return;
            }
            
            _stats[statType].CurrentValue += value;
        }
        public void SetStat(StatType statType, int value)
        {
            if (!HasStat(statType))
            {
                return;
            }
            
            _stats[statType].CurrentValue += value;
        }

        //Called every frame - need to update modifiers' timers, need to keep this method light
        public void UpdateStatsModifiers(float dt) 
        {
            foreach(var stat in _stats.Values)
            {
                stat.UpdateModifiers(dt);
            }
        }
        public void ApplyModifier(Modifier modifier)
        {
            if (!HasStat(modifier.statType))
            {
                return;
            }
            _stats[modifier.statType].ApplyModifier(modifier);
        }
    }
}