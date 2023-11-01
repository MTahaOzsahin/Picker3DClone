using System.Collections.Generic;
using System.Linq;
using Controllers.CollectablesController;
using Controllers.PlatformControllers;
using Controllers.PlayerController;
using DG.Tweening;
using Managers;
using UnityEngine;

namespace Helpers
{
    public class ObjectPool : SingletonMB<ObjectPool>
    {
        [Header("Collectables Prefabs"), Tooltip("Place any collectable to instantiate")]
        [SerializeField] private List<GameObject> collectablePrefabsToPooled;
        
        [Header("How many collectables wanted to be pooled. Will be for each")]
        public int targetQuantityForCollectables;
        
        [Header("Platform Normal Prefabs")]
        [SerializeField] private GameObject platformNormalPrefabToPooled;
        
        [Header("Platform CheckPoint Prefabs")]
        [SerializeField] private GameObject platformCheckPointPrefabToPooled;
        
        [Header("How many platform wanted to be pooled"),Tooltip("Will instantiate foreach as target quantity")]
        public int targetQuantityForPlatforms;
        
        [Header("PLayer Prefabs")]
        [SerializeField] private GameObject playerPrefabToPooled;
        
        [Header("How many player wanted to be pooled")]
        public int targetQuantityForPlayer;
        
        [HideInInspector] public List<GameObject> pooledCollectablesList;
        [HideInInspector] public List<GameObject> pooledNormalPlatformsList;
        [HideInInspector] public List<GameObject> pooledCheckPointPlatformsList;
        [HideInInspector] public List<GameObject> pooledPlayerList;


        private void OnEnable()
        {
            GameManager.Instance.OnInitializingPool += InitPools;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnInitializingPool -= InitPools;
        }

        private void Awake()
        {
            pooledCollectablesList = new List<GameObject>();
            pooledNormalPlatformsList = new List<GameObject>();
            pooledCheckPointPlatformsList = new List<GameObject>();
            pooledPlayerList = new List<GameObject>();
        }


        private void InitPools()
        {
            if (pooledCollectablesList.Count == 0)
            {
                for (int i = 0; i < targetQuantityForCollectables; i++)
                {
                    foreach (var t in collectablePrefabsToPooled)
                    {
                        var tempCollectableGameObject = Instantiate(t,transform);
                        tempCollectableGameObject.SetActive(false);
                        var collectableGameObjectType = tempCollectableGameObject.GetComponent<CollectableBase>().selectedCollectableType;
                        tempCollectableGameObject.name = collectableGameObjectType + $"{i + 1}";
                        pooledCollectablesList.Add(tempCollectableGameObject);
                    }
                }
            }

            if (pooledNormalPlatformsList.Count == 0)
            {
                for (int i = 0; i < targetQuantityForPlatforms; i++)
                {
                    var tempPlatformNormalGameObject = Instantiate(platformNormalPrefabToPooled, transform);
                    tempPlatformNormalGameObject.SetActive(false);
                    var platformNormalGameObjectType = tempPlatformNormalGameObject.GetComponent<PlatformBase>().selectedPlatformType;
                    tempPlatformNormalGameObject.name = platformNormalGameObjectType + $"{i + 1}";
                    pooledNormalPlatformsList.Add(tempPlatformNormalGameObject);
                
                    var tempPlatformCheckPointGameObject = Instantiate(platformCheckPointPrefabToPooled, transform);
                    tempPlatformCheckPointGameObject.SetActive(false);
                    var platformCheckPointGameObjectType = tempPlatformCheckPointGameObject.GetComponent<PlatformBase>().selectedPlatformType;
                    tempPlatformCheckPointGameObject.name = platformCheckPointGameObjectType + $"{i + 1}";
                    pooledCheckPointPlatformsList.Add(tempPlatformCheckPointGameObject);
                }
            }

            if (pooledPlayerList.Count == 0)
            {
                for (int i = 0; i < targetQuantityForPlayer; i++)
                {
                    var tempPlayerGameObject = Instantiate(playerPrefabToPooled, transform);
                    tempPlayerGameObject.SetActive(false);
                    pooledPlayerList.Add(tempPlayerGameObject);
                }
            }
            
            GameManager.Instance.SelectedGameStates = GameStates.OnInitializingLevel;
        }
        
        public GameObject GetPooledCollectableGameObject(CollectableType selectedCollectableType,Transform parentGameObject)
        {
            if (pooledCollectablesList.Count == 0) return null;
            var pooledCollectableWantedTypeList = pooledCollectablesList.Where(x =>
                x.GetComponent<CollectableBase>().selectedCollectableType == selectedCollectableType).ToList();
            if (pooledCollectableWantedTypeList.Count == 0) return null;
            var collectableGameObject = pooledCollectableWantedTypeList.First();
            if(!collectableGameObject.activeInHierarchy)
            {
                collectableGameObject.transform.SetParent(parentGameObject);
                collectableGameObject.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                pooledCollectablesList.Remove(collectableGameObject);
                return collectableGameObject;
            }
            return null;
        }
        
        public GameObject GetPooledPlatformNormalGameObject(Transform parentGameObject)
        {
            if (pooledNormalPlatformsList.Count == 0) return null;
            var wantedNormalPlatformGameObject = pooledNormalPlatformsList.Where(x =>
                x.GetComponent<PlatformBase>().selectedPlatformType == PlatformType.Normal && !x.activeInHierarchy).ToList();
            if (wantedNormalPlatformGameObject.Count == 0) return null;
            var normalPlatform = wantedNormalPlatformGameObject.First();
            if (!normalPlatform.activeInHierarchy)
            {
                normalPlatform.transform.SetParent(parentGameObject);
                pooledNormalPlatformsList.Remove(wantedNormalPlatformGameObject.First());
                return normalPlatform;
            }

            return null;
        }
        
        public GameObject GetPooledPlatformCheckPointGameObject(Transform parentGameObject)
        {
            if (pooledCheckPointPlatformsList.Count == 0) return null;
            var wantedCheckPointPlatformGameObject = pooledCheckPointPlatformsList.Where(x =>
                x.GetComponent<PlatformBase>().selectedPlatformType == PlatformType.CheckPoint && !x.activeInHierarchy).ToList();
            if (wantedCheckPointPlatformGameObject.Count == 0) return null;
            var checkPointPlatform = wantedCheckPointPlatformGameObject.First();
            if (!checkPointPlatform.activeInHierarchy)
            {
                checkPointPlatform.transform.SetParent(parentGameObject);
                pooledCheckPointPlatformsList.Remove(wantedCheckPointPlatformGameObject.First());
                return checkPointPlatform;
            }

            return null;
        }

        public GameObject GetPooledPlayerGameObject(Transform parentGameObject)
        {
            if (pooledPlayerList.Count == 0) return null;
            var playerGameObject = pooledPlayerList.First();
            if (!playerGameObject.activeInHierarchy)
            {
                playerGameObject.transform.SetParent(parentGameObject);
                pooledPlayerList.Remove(playerGameObject);
                return playerGameObject;
            }

            return null;
        }

        public void ReturnAnyPooledGameObjectToPool(GameObject pooledGameObject)
        {
            var originScaleValue = pooledGameObject.transform.localScale;
            var scaleTween = pooledGameObject.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f).SetEase(Ease.Linear);
            var allColliders = pooledGameObject.GetComponents<BoxCollider>();
            foreach (var boxCollider in allColliders)
            {
                boxCollider.enabled = false;
            }
            pooledGameObject.transform.SetParent(transform);
            scaleTween.OnComplete(() =>
            {
                pooledGameObject.SetActive(false);
                if (pooledGameObject.GetComponent<CollectableBase>() != null)
                {
                    if (pooledCollectablesList.Contains(pooledGameObject)) return;
                    pooledCollectablesList.Add(pooledGameObject);
                }
                else if (pooledGameObject.GetComponent<PlatformNormal>() != null)
                {
                    if (pooledNormalPlatformsList.Contains(pooledGameObject)) return;
                    pooledNormalPlatformsList.Add(pooledGameObject);
                }
                else if (pooledGameObject.GetComponent<PlatformCheckPoint>() != null)
                {
                    if (pooledCheckPointPlatformsList.Contains(pooledGameObject)) return;
                    pooledCheckPointPlatformsList.Add(pooledGameObject);
                }
                else if (pooledGameObject.GetComponent<PlayerMovementController>() != null)
                {
                    if (pooledPlayerList.Contains(pooledGameObject)) return;
                    pooledPlayerList.Add(pooledGameObject);
                }
                pooledGameObject.transform.localScale = originScaleValue;
                foreach (var boxCollider in allColliders)
                {
                    boxCollider.enabled = true;
                }
                scaleTween.Kill();
            });
        }

        public void DestroyRemainPooledObjects()
        {
            if (pooledCollectablesList.Count == 0) return;
            foreach (var pooledGameObject in pooledCollectablesList)
            {
                Destroy(pooledGameObject);
            }
            pooledCollectablesList.Clear();
            
            if (pooledNormalPlatformsList.Count == 0) return;
            foreach (var pooledGameObject in pooledNormalPlatformsList)
            {
                Destroy(pooledGameObject);
            }
            pooledNormalPlatformsList.Clear();
            
            if (pooledCheckPointPlatformsList.Count == 0) return;
            foreach (var pooledGameObject in pooledCheckPointPlatformsList)
            {
                Destroy(pooledGameObject);
            }
            pooledCheckPointPlatformsList.Clear();
            
            if (pooledPlayerList.Count == 0) return;
            foreach (var pooledGameObject in pooledPlayerList)
            {
                Destroy(pooledGameObject);
            }
            pooledPlayerList.Clear();
        }

        
    }
}
