using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    [DefaultExecutionOrder(-9)]
    public class LevelManager : SingletonMB<LevelManager>
    {
        public List<GameObject> playerCollectedGameObjects;

        public int targetLevel;

        [Tooltip("Which area player currently on.")]
        public int playerCurrentPositionInLevel;

        public int checkPoint1Target;
        public int checkPoint2Target;
        public int checkPoint3Target;

        public GameObject clonePLayerGameObject;

        private void Awake()
        {
            DOTween.SetTweensCapacity(5000,50);
            playerCollectedGameObjects = new List<GameObject>();
        }

        private void OnEnable()
        {
            GameManager.Instance.OnWaitingInput += ClearCollectedGameObjects;
            GameManager.Instance.OnLevelAdjusting += ControlCollectedGameObjects;
            GameManager.Instance.OnCheckPoint1 += OnCheckPoint1;
            GameManager.Instance.OnCheckPoint2 += OnCheckPoint2;
            GameManager.Instance.OnCheckPoint3 += OnCheckPoint3;
            GameManager.Instance.OnCheckPoint3Success += LoadNextLevel;
        }

        private void OnDisable()
        {
            if (GameManager.Instance == null) return;
            GameManager.Instance.OnWaitingInput -= ClearCollectedGameObjects;
            GameManager.Instance.OnLevelAdjusting -= ControlCollectedGameObjects;
            GameManager.Instance.OnCheckPoint1 -= OnCheckPoint1;
            GameManager.Instance.OnCheckPoint2 -= OnCheckPoint2;
            GameManager.Instance.OnCheckPoint3 -= OnCheckPoint3;
            GameManager.Instance.OnCheckPoint3Success -= LoadNextLevel;
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
                Debug.LogError("Fail");
                StartCoroutine(TempCoroutine());
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
                Debug.LogError("Fail");
                StartCoroutine(TempCoroutine());
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
                // GameManager.Instance.SelectedGameStates = GameStates.OnLevelAdjusting;
                playerCurrentPositionInLevel = 3;
            }
            else
            {
                Debug.LogError("Fail");
                StartCoroutine(TempCoroutine());
            }
        }

        private void LoadNextLevel()
        {
            targetLevel++;
            GameManager.Instance.SelectedGameStates = GameStates.OnInitializingLevel;
        }
        
        private IEnumerator TempCoroutine()
        {
            yield return new WaitForSeconds(5f);
            var activeScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(activeScene);
        }
    }
}
