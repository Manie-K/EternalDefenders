using MG_Utilities;
using System;
using UnityEngine;

namespace EternalDefenders
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] AudioClip music;
        [SerializeField, Range(0.0f, 1.0f)] float musicVolume = 0.1f;
        [SerializeField, Range(0.0f, 1.0f)] float sfxVolume = 0.1f;

        AudioSource _audioSource;

        public static event Action<float> OnSfxVolumeChanged;

        public float MusicVolume
        { 
            get { return musicVolume; }
            set 
            {
                musicVolume = value;
                _audioSource.volume = value;
            }
        }

        public float SFXVolume
        {
            get { return sfxVolume; }
            set 
            {
                sfxVolume = value;
                OnSfxVolumeChanged?.Invoke(value);
            }
        }

        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.loop = true;
            _audioSource.volume = MusicVolume;
            _audioSource.clip = music;
            _audioSource.Play();
        }
    }
}
