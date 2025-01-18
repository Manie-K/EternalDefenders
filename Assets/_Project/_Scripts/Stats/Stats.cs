using System.Collections.Generic;
using PlasticGui.WorkspaceWindow.PendingChanges;

namespace EternalDefenders
{
    public class Stats
    {
        readonly Dictionary<StatType, int> _stats;
        readonly List<Modifier> _modifiers;
        public Stats(Dictionary<StatType, int> stats)
        {
            _stats = new Dictionary<StatType, int>(stats); //shallow copy
        }
        
        public int GetStat(StatType stat) => _stats.GetValueOrDefault(stat, 0);
        public void SetStat(StatType stat, int value) => _stats[stat] = value;
        public void ChangeStat(StatType stat, int value)
        {
            int currentValue = _stats.GetValueOrDefault(stat, 0);
            _stats[stat] = currentValue + value;
        }

        public void AddModifier(Modifier modifier) => _modifiers.Add(modifier);
    }
}