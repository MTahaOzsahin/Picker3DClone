using System;
using Helpers;
using UnityEngine;

namespace Managers
{
    public class SoundManager : SingletonMB<SoundManager>
    {
        private AudioSource audioSource;
        
        [Header("Win Sound")] 
        [SerializeField] private AudioClip winSound;
        [SerializeField,Range(0f,1f)] private float winSoundVolume;
        
        [Header("Lose Sound")] 
        [SerializeField] private AudioClip loseSound;
        [SerializeField,Range(0f,1f)] private float loseSoundVolume;
        
        [Header("CheckPoint Success Sound")] 
        [SerializeField] private AudioClip checkPointSuccessSound;
        [SerializeField,Range(0f,1f)] private float checkPointSuccessSoundVolume;
        
        [Header("Start Sound")] 
        [SerializeField] private AudioClip startSound;
        [SerializeField, Range(0f, 1f)] private float startSoundVolume;
        
        [Header("Collected Sound")] 
        [SerializeField] private AudioClip collectedSound;
        [SerializeField, Range(0f, 1f)] private float collectedSoundVolume;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            GameManager.Instance.OnStarted += PlayStartSound;
            GameManager.Instance.OnEnding += PlayWinLoseSound;
            GameManager.Instance.OnCheckPoint1Success += PlayCheckPointSound;
            GameManager.Instance.OnCheckPoint2Success += PlayCheckPointSound;
        }

        private void OnDisable()
        {
            if (GameManager.Instance == null) return;
            GameManager.Instance.OnStarted -= PlayStartSound;
            GameManager.Instance.OnEnding -= PlayWinLoseSound;
            GameManager.Instance.OnCheckPoint1Success -= PlayCheckPointSound;
            GameManager.Instance.OnCheckPoint2Success -= PlayCheckPointSound;
        }

        public void PlayCollectedSound()
        {
            audioSource.PlayOneShot(collectedSound,collectedSoundVolume);
        }

        private void PlayWinLoseSound()
        {
            if (LevelManager.Instance == null) return;
            switch (LevelManager.Instance.selectedLevelProgress)
            {
                case LevelProgress.OnProgress:
                    //
                    break;
                case LevelProgress.Win:
                    audioSource.PlayOneShot(winSound,winSoundVolume);
                    break;
                case LevelProgress.Lose:
                    audioSource.PlayOneShot(loseSound,loseSoundVolume);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void PlayCheckPointSound()
        {
            audioSource.PlayOneShot(checkPointSuccessSound,checkPointSuccessSoundVolume);
        }
        
        private void PlayStartSound()
        {
            if (LevelManager.Instance.PlayerCurrentPositionInLevel != 1) return;
            audioSource.PlayOneShot(startSound,startSoundVolume);
        }
    }
}
