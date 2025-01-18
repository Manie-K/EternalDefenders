using System.Collections.Generic;
using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "EffectSO", menuName = "EternalDefenders/Effect")]
    public class EffectSO : ScriptableObject
    {
        public List<Modifier> modifiers;
        public ParticleSystem particleSystem;
        // sound effects, visual etc.
    }
}