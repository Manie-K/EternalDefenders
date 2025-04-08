using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "InstantModifier", menuName = "EternalDefenders/Modifiers/InstantModifier")]
    public class InstantModifier : Modifier
    {
        public override Modifier CreateCopy()
        {
            InstantModifier copy = CreateInstance<InstantModifier>();
            copy.InitOnCreation(statType, modifierType, persistAfterFinish, limitedDurationTime, value);
            return copy;
        }
    }
}