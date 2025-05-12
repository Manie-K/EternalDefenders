using System;
using UnityEngine;

namespace EternalDefenders
{
    public enum SoundType
    {
        ATTACK,
        JUMP,
        LAND,
        FOOTSTEP,
        DIE
    }

    [Serializable]
    public struct SoundList
    {
        [HideInInspector] public string name;
        [SerializeField] private AudioClip[] sounds;

        public AudioClip[] Sounds { get { return sounds; } }
    }
}
