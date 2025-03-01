using System.Collections.Generic;
using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "Effect", menuName = "EternalDefenders/Effect")]
    public class Effect : ScriptableObject
    {
        public List<Modifier> modifiers;
        public ParticleSystem particleSystem;
        // sound effects, visual etc.
    }
}