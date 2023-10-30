using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Managers;
using TMPro;
using UnityEngine;

namespace Controllers.PlatformControllers
{
    public enum CheckPointIndex
    {
        First,
        Second,
        Third
    }
    public class PlatformCheckPoint : PlatformBase
    {
        public CheckPointIndex selectedCheckPointIndex;

        [Header("CheckPoint Trigger Collider")]
        [SerializeField] private BoxCollider triggerBoxCollider;
        
        [Header("Pool Trigger Collider")]
        [SerializeField] private GameObject poolGameObject;

        [Header("Target Collectable Text")] 
        [SerializeField] private TextMeshProUGUI targetCountText;

        [Header("Wall BoxCollider")] 
        [SerializeField] private BoxCollider wallBoxCollider;

        private List<BoxCollider> poolGameObjectColliders;
        private List<GameObject> collectableGameObjectInPool;
        private int targetCollectablesCount;

        private void Awake()
        {
            collectableGameObjectInPool = new List<GameObject>();
            poolGameObjectColliders = new List<BoxCollider>();
            poolGameObjectColliders = poolGameObject.GetComponents<BoxCollider>().ToList();
        }

        private void OnEnable()
        {
            GameManager.Instance.OnLevelAdjusting += OnLevelAdjustColliders;
            GameManager.Instance.OnStarted += OnStartedAdjustColliders;
            GameManager.Instance.OnCheckPoint1 += OnCheckPoint;
            GameManager.Instance.OnCheckPoint2 += OnCheckPoint;
            GameManager.Instance.OnCheckPoint3 += OnCheckPoint;
        }

        private void OnDisable()
        {
            if (GameManager.Instance == null) return;
            GameManager.Instance.OnLevelAdjusting -= OnLevelAdjustColliders;
            GameManager.Instance.OnStarted -= OnStartedAdjustColliders;
            GameManager.Instance.OnCheckPoint1 -= OnCheckPoint;
            GameManager.Instance.OnCheckPoint2 -= OnCheckPoint;
            GameManager.Instance.OnCheckPoint3 -= OnCheckPoint;
        }

        public void InitCheckPointPlatform(int checkPointIndex)
        {
            var levelManagerInstance = LevelManager.Instance;
            switch (checkPointIndex)
            {
                case 1:
                    selectedCheckPointIndex = CheckPointIndex.First;
                    if (levelManagerInstance != null)
                        targetCountText.text = "0/" + levelManagerInstance.checkPoint1Target;
                    targetCollectablesCount = levelManagerInstance.checkPoint1Target;
                    break;
                case 2:
                    selectedCheckPointIndex = CheckPointIndex.Second;
                    if (levelManagerInstance != null)
                        targetCountText.text = "0/" + levelManagerInstance.checkPoint2Target;
                    targetCollectablesCount = levelManagerInstance.checkPoint2Target;
                    break;
                case 3:
                    if (levelManagerInstance != null)
                        selectedCheckPointIndex = CheckPointIndex.Third;
                    targetCountText.text = "0/" + levelManagerInstance.checkPoint3Target;
                    targetCollectablesCount = levelManagerInstance.checkPoint3Target;
                    break;
                default:
                    selectedCheckPointIndex = CheckPointIndex.First;
                    if (levelManagerInstance != null)
                        targetCountText.text = "0/" + levelManagerInstance.checkPoint1Target;
                    targetCollectablesCount = levelManagerInstance.checkPoint1Target;
                    break;
            }

            if (selectedCheckPointIndex == CheckPointIndex.Third)
            {
                wallBoxCollider.enabled = true;
            }
        }

        /// <summary>
        /// Disable all pool colliders so collected game objects will fall, platform will rise
        /// and player will keep going flawless.
        /// </summary>
        private void OnLevelAdjustColliders()
        {
            foreach (var boxCollider in poolGameObjectColliders)
            {
                boxCollider.enabled = false;
            }
            collectableGameObjectInPool.Clear();
            MoveCheckPointPlatform();
            GameManager.Instance.SelectedGameStates = GameStates.OnWaitingInput;
        }

        /// <summary>
        /// Enable all colliders again.
        /// </summary>
        private void OnStartedAdjustColliders()
        {
            StartCoroutine(OnStartedAdjustCollidersCoroutine());
        }

        private IEnumerator OnStartedAdjustCollidersCoroutine()
        {
            if (triggerBoxCollider.enabled) yield break;
            yield return new WaitForSeconds(3f);
            foreach (var boxCollider in poolGameObjectColliders)
            {
                boxCollider.enabled = true;
            }
            yield return new WaitForSeconds(1f);
            triggerBoxCollider.enabled = true;
        }

        private void MoveCheckPointPlatform()
        {
            switch (LevelManager.Instance.playerCurrentPositionInLevel)
            {
                case 1:
                    if (selectedCheckPointIndex == CheckPointIndex.First)
                    {
                        Tween moveTween = poolGameObject.transform.DOMoveY(0.3f, 0.5f).SetEase(Ease.Linear);
                        moveTween.OnComplete((() => moveTween.Kill()));
                    }
                    break;
                case 2:
                    if (selectedCheckPointIndex == CheckPointIndex.Second)
                    {
                        Tween moveTween = poolGameObject.transform.DOMoveY(0.3f, 0.5f).SetEase(Ease.Linear);
                        moveTween.OnComplete((() => moveTween.Kill()));
                    }
                    break;
                case 3:
                    if (selectedCheckPointIndex == CheckPointIndex.Third)
                    {
                        Tween moveTween = poolGameObject.transform.DOMoveY(0.3f, 0.5f).SetEase(Ease.Linear);
                        moveTween.OnComplete((() => moveTween.Kill()));
                    }
                    break;
            }
        }

        private void OnCheckPoint()
        {
            triggerBoxCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                switch (selectedCheckPointIndex)
                {
                    case CheckPointIndex.First:
                        GameManager.Instance.SelectedGameStates = GameStates.OnCheckPoint1;
                        break;
                    case CheckPointIndex.Second:
                        GameManager.Instance.SelectedGameStates = GameStates.OnCheckPoint2;
                        break;
                    case CheckPointIndex.Third:
                        GameManager.Instance.SelectedGameStates = GameStates.OnCheckPoint3;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Collectables"))
            {
                var levelManagerInstance = LevelManager.Instance;
                if (!levelManagerInstance.playerCollectedGameObjects.Contains(other.gameObject))
                {
                    levelManagerInstance.playerCollectedGameObjects.Add(other.gameObject);
                }
                if (!collectableGameObjectInPool.Contains(other.gameObject))
                {
                    collectableGameObjectInPool.Add(other.gameObject);
                }
            }

            var collectableCountInPool = collectableGameObjectInPool.Count;
            targetCountText.text = $"{collectableCountInPool + " / " + targetCollectablesCount}";
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Collectables"))
            {
                var levelManagerInstance = LevelManager.Instance;
                if (levelManagerInstance.playerCollectedGameObjects.Contains(other.gameObject))
                {
                    levelManagerInstance.playerCollectedGameObjects.Remove(other.gameObject);
                }
                if (collectableGameObjectInPool.Contains(other.gameObject))
                {
                    collectableGameObjectInPool.Remove(other.gameObject);
                }
            }
        }
    }
}
