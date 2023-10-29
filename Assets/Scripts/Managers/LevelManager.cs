using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using UnityEngine;

namespace Managers
{
    public class LevelManager : SingletonMB<LevelManager>
    {
        public List<GameObject> playerCollectedGameObjects;

        public int levelIndex;

        [Tooltip("Which area player currently on.")]
        public int playerCurrentPositionInLevel;

        public int checkPoint1Target;
        public int checkPoint2Target;
        public int checkPoint3Target;

        private void Awake()
        {
            playerCollectedGameObjects = new List<GameObject>();
        }

        private void OnEnable()
        {
            GameManager.Instance.OnWaitingInput += ClearCollectedGameObjects;
            GameManager.Instance.OnLevelAdjusting += ControlCollectedGameObjects;
            GameManager.Instance.OnCheckPoint1 += OnCheckPoint1;
            GameManager.Instance.OnCheckPoint2 += OnCheckPoint2;
            GameManager.Instance.OnCheckPoint3 += OnCheckPoint3;
        }

        private void OnDisable()
        {
            if (GameManager.Instance == null) return;
            GameManager.Instance.OnWaitingInput -= ClearCollectedGameObjects;
            GameManager.Instance.OnLevelAdjusting -= ControlCollectedGameObjects;
            GameManager.Instance.OnCheckPoint1 -= OnCheckPoint1;
            GameManager.Instance.OnCheckPoint2 -= OnCheckPoint2;
            GameManager.Instance.OnCheckPoint3 -= OnCheckPoint3;
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
                GameManager.Instance.SelectedGameStates = GameStates.OnLevelAdjusting;
                playerCurrentPositionInLevel = 2;
            }
            else
            {
                Debug.LogError("Fail");
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
                GameManager.Instance.SelectedGameStates = GameStates.OnLevelAdjusting;
            }
            else
            {
                Debug.LogError("Fail");
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
                GameManager.Instance.SelectedGameStates = GameStates.OnLevelAdjusting;
                playerCurrentPositionInLevel = 3;
            }
            else
            {
                Debug.LogError("Fail");
            }
        }
    }
}
