using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "Gamba", menuName = "EternalDefenders/ItemSystem/Items/Gamba")]
    public class Gamba : Item
    {
        [SerializeField] private int _maxResourceAmount;
        [SerializeField] private int _minResourceAmount;
        
        public override void Collect()
        {
            Gamble();
        }
        public override void Remove()
        {
            return;
        }
        public void Gamble()
        {
            var inventory = PlayerResourceInventory.Instance;
            int amount = Random.Range(_minResourceAmount, _maxResourceAmount);

            int randomResource = Random.Range(0, Cost.Count);

            if (amount > 0)
            {
                inventory.AddResource(Cost[randomResource].resource, amount);
            }
            else
            {
                inventory.RemoveResourceNetagive(Cost[randomResource].resource, -amount);
            }
        }
    }
}
