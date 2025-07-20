using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "Cascade", menuName = "EternalDefenders/ItemSystem/Items/Cascade")]
    public class Cascade : Item
    {

        [System.Serializable]
        public class StatBoost
        {
            public StatType statType;
            public int value;
        }

        [SerializeField] List<StatBoost> _boosts;

        public override void Collect()
        {
            if (DuplicateCount == 0)
            {
                DuplicateCount++;
                ItemManager.Instance.OnItemPickUp += ApplyRandomStat;
            }

        }

        public override void Remove()
        {
            if (DuplicateCount == 1)
            {
                DuplicateCount--;
                ItemManager.Instance.OnItemPickUp -= ApplyRandomStat;
            }

        }

        private void ApplyRandomStat(Item item)
        {
            Stats playerStats = PlayerController.Instance.Stats;
            StatBoost randomBoost = _boosts.ElementAt(UnityEngine.Random.Range(0, _boosts.Count));

            InstantModifier modifier = ScriptableObject.CreateInstance<InstantModifier>();
            modifier.statType =randomBoost.statType;
            modifier.modifierType = ModifierType.Flat;
            modifier.value = randomBoost.value;
            modifier.persistAfterFinish = true;
            modifier.limitedDurationTime = 0.01f;

            playerStats.ApplyModifier(modifier);
        }
    }
}
