using System;
using UnityEngine;

namespace EternalDefenders
{
    [RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
    public class AudioHelper : MonoBehaviour
    {
        [SerializeField] SoundList[] soundList;

        AudioSource _audioSource;

        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            if (AudioManager.Instance != null)
            {
                SetVolume(AudioManager.Instance.SFXVolume);
            }

            AudioManager.OnSfxVolumeChanged += SetVolume;
        }

        void SetVolume(float value)
        { 
            _audioSource.volume = value;
        }

        public void PlaySound(SoundType sound, int index)
        {
            _audioSource.PlayOneShot(soundList[(int)sound].Sounds[index]);
        }

#if UNITY_EDITOR

        private void OnEnable()
        {
            string[] names = Enum.GetNames(typeof(SoundType));
            Array.Resize(ref soundList, names.Length);
            for (int i = 0; i < names.Length; i++) 
            {
                soundList[i].name = names[i];
            }
        }

#endif
    }
}
