using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Helpers;
using UnityEngine;

namespace Managers
{
    public enum LevelProgress
    {
        OnProgress,
        Win,
        Lose
    }
    
    [DefaultExecutionOrder(-9)]
    public class LevelManager : SingletonMB<LevelManager>
    {
        public LevelProgress selectedLevelProgress;
        
        [HideInInspector]public List<GameObject> playerCollectedGameObjects;

        [SerializeField] private int currentLevel;
        public int CurrentLevel
        {
            get => currentLevel;
            set
            {
                currentLevel = value;
                SetPlayerPrefsLevel(value);
            }
        }
        
        [Tooltip("Which area player currently in.")]
        [SerializeField] private int playerCurrentPositionInLevel;
        public int PlayerCurrentPositionInLevel
        {
            get => playerCurrentPositionInLevel;
            set => playerCurrentPositionInLevel = value;
        }

        public int checkPoint1Target;
        public int checkPoint2Target;
        public int checkPoint3Target;

        [SerializeField] private GameObject clonePlayerGameObject;
        public GameObject ClonePlayerGameObject
        {
            get => clonePlayerGameObject;
            set => clonePlayerGameObject = value;
        }

        private void Awake()
        {
            GetPLayerPrefs();
            DOTween.SetTweensCapacity(5000,50);
            playerCollectedGameObjects = new List<GameObject>();
            selectedLevelProgress = LevelProgress.OnProgress;
        }

        private void OnEnable()
        {
            GameManager.Instance.OnInitializingLevel += SetPLayerCurrentPosition;
            GameManager.Instance.OnStarted += SetLevelProgress;
            GameManager.Instance.OnWaitingInput += ClearCollectedGameObjects;
            GameManager.Instance.OnLevelAdjusting += ControlCollectedGameObjects;
            GameManager.Instance.OnCheckPoint1 += OnCheckPoint1;
            GameManager.Instance.OnCheckPoint2 += OnCheckPoint2;
            GameManager.Instance.OnCheckPoint3 += OnCheckPoint3;
        }

        private void OnDisable()
        {
            if (GameManager.Instance == null) return;
            GameManager.Instance.OnInitializingLevel -= SetPLayerCurrentPosition;
            GameManager.Instance.OnStarted -= SetLevelProgress;
            GameManager.Instance.OnWaitingInput -= ClearCollectedGameObjects;
            GameManager.Instance.OnLevelAdjusting -= ControlCollectedGameObjects;
            GameManager.Instance.OnCheckPoint1 -= OnCheckPoint1;
            GameManager.Instance.OnCheckPoint2 -= OnCheckPoint2;
            GameManager.Instance.OnCheckPoint3 -= OnCheckPoint3;
        }
        
        private void GetPLayerPrefs()
        {
            currentLevel = PlayerPrefs.GetInt("currentLevel",1);
        }
        
        private void SetPlayerPrefsLevel(int targetLevel)
        {
            PlayerPrefs.SetInt("currentLevel",targetLevel);
        }

        private void SetPLayerCurrentPosition()
        {
            playerCurrentPositionInLevel = 1;
        }

        private void SetLevelProgress()
        {
            selectedLevelProgress = LevelProgress.OnProgress;
        }

        private void ControlCollectedGameObjects()
        {
            if (playerCollectedGameObjects.Count != 0)
            {
                foreach (var collectedGameObject in playerCollectedGameObjects)
                {
                    ObjectPool.Instance.ReturnAnyPooledGameObjectToPool(collectedGameObject);
                }
            }
        }

        private void ClearCollectedGameObjects()
        {
            if (playerCollectedGameObjects.Count != 0)
            {
                playerCollectedGameObjects.Clear();
            }
        }

        private void OnCheckPoint1()
        {
            StartCoroutine(OnCheckPoint1Coroutine());
        }

        private IEnumerator OnCheckPoint1Coroutine()
        {
            yield return new WaitForSeconds(3f);
            var playerCollectedGameObjectCount = playerCollectedGameObjects.Count;
            if (playerCollectedGameObjectCount >= checkPoint1Target)
            {
                GameManager.Instance.SelectedGameStates = GameStates.OnCheckPoint1Success;
                GameManager.Instance.SelectedGameStates = GameStates.OnLevelAdjusting;
                playerCurrentPositionInLevel = 2;
            }
            else
            {
                selectedLevelProgress = LevelProgress.Lose;
                GameManager.Instance.SelectedGameStates = GameStates.OnEnding;
            }
        }
        
        private void OnCheckPoint2()
        {
            StartCoroutine(OnCheckPoint2Coroutine());
        }
        
        private IEnumerator OnCheckPoint2Coroutine()
        {
            yield return new WaitForSeconds(3f);
            var playerCollectedGameObjectCount = playerCollectedGameObjects.Count;
            if (playerCollectedGameObjectCount >= checkPoint2Target)
            {
                GameManager.Instance.SelectedGameStates = GameStates.OnCheckPoint2Success;
                GameManager.Instance.SelectedGameStates = GameStates.OnLevelAdjusting;
                playerCurrentPositionInLevel = 2;
            }
            else
            {
                selectedLevelProgress = LevelProgress.Lose;
                GameManager.Instance.SelectedGameStates = GameStates.OnEnding;
            }
        }
        
        private void OnCheckPoint3()
        {
            StartCoroutine(OnCheckPoint3Coroutine());
        }
        
        private IEnumerator OnCheckPoint3Coroutine()
        {
            yield return new WaitForSeconds(3f);
            var playerCollectedGameObjectCount = playerCollectedGameObjects.Count;
            if (playerCollectedGameObjectCount >= checkPoint3Target)
            {
                GameManager.Instance.SelectedGameStates = GameStates.OnCheckPoint3Success;
                playerCurrentPositionInLevel = 3;
                selectedLevelProgress = LevelProgress.Win;
            }
            else
            {
                selectedLevelProgress = LevelProgress.Lose;
            }
            GameManager.Instance.SelectedGameStates = GameStates.OnEnding;
            ControlCollectedGameObjects();
        }
    }
}
