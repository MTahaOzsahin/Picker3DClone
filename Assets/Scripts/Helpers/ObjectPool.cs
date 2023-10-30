using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Controllers.CollectablesController;
using Controllers.PlatformControllers;
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
        
        [HideInInspector] public List<GameObject> pooledCollectablesList;
        [HideInInspector] public List<GameObject> pooledNormalPlatformsList;
        [HideInInspector] public List<GameObject> pooledCheckPointPlatformsList;


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
        }


        private void InitPools()
        {
            for (int i = 0; i < targetQuantityForCollectables; i++)
            {
                for (int j = 0; j < collectablePrefabsToPooled.Count; j++)
                {
                    var tempCollectableGameObject = Instantiate(collectablePrefabsToPooled[j],transform);
                    tempCollectableGameObject.SetActive(false);
                    var collectableGameObjectType = tempCollectableGameObject.GetComponent<CollectableBase>().selectedCollectableType;
                    tempCollectableGameObject.name = collectableGameObjectType + $"{i + 1}";
                    pooledCollectablesList.Add(tempCollectableGameObject);
                }
            }
            
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

            GameManager.Instance.SelectedGameStates = GameStates.OnInitializingLevel;
        }
        
        public GameObject GetPooledCollectableGameObject(CollectableType selectedCollectableType,Transform parentGameObject)
        {
            if (pooledCollectablesList.Count == 0) return null;
            var pooledCollectableWantedTypeList = pooledCollectablesList.Where(x =>
                x.GetComponent<CollectableBase>().selectedCollectableType == selectedCollectableType).ToList();
            var collectableGameObject = pooledCollectableWantedTypeList.First();
            if(!collectableGameObject.activeInHierarchy)
            {
                collectableGameObject.transform.SetParent(parentGameObject);
                pooledCollectablesList.Remove(collectableGameObject);
                return collectableGameObject;
            }
            return null;
        }
        
        public GameObject GetPooledPlatformNormalGameObject(Transform parentGameObject)
        {
            var wantedNormalPlatformGameObject = pooledNormalPlatformsList.Where(x =>
                x.GetComponent<PlatformBase>().selectedPlatformType == PlatformType.Normal && !x.activeInHierarchy).ToList();
            if (wantedNormalPlatformGameObject.Count == 0) return null;
            var normalPlatform = wantedNormalPlatformGameObject.First();
            normalPlatform.transform.SetParent(parentGameObject);
            pooledNormalPlatformsList.Remove(wantedNormalPlatformGameObject.First());
            return normalPlatform;
        }
        
        public GameObject GetPooledPlatformCheckPointGameObject(Transform parentGameObject)
        {
            var wantedCheckPointPlatformGameObject = pooledCheckPointPlatformsList.Where(x =>
                x.GetComponent<PlatformBase>().selectedPlatformType == PlatformType.CheckPoint && !x.activeInHierarchy).ToList();
            if (wantedCheckPointPlatformGameObject.Count == 0) return null;
            var checkPointPlatform = wantedCheckPointPlatformGameObject.First();
            checkPointPlatform.transform.SetParent(parentGameObject);
            pooledCheckPointPlatformsList.Remove(wantedCheckPointPlatformGameObject.First());
            return checkPointPlatform;
        }

        public void ReturnAnyPooledGameObjectToPool(GameObject pooledGameObject)
        {
            StartCoroutine(ReturnAnyPooledGameObjectCoroutine(pooledGameObject));
        }

        private IEnumerator ReturnAnyPooledGameObjectCoroutine(GameObject pooledGameObject)
        {
            var originScaleValue = pooledGameObject.transform.localScale;
            var scaleTween = pooledGameObject.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f).SetEase(Ease.Linear);
            scaleTween.OnComplete(() =>
            {
                pooledGameObject.SetActive(false);
                pooledGameObject.transform.SetParent(transform);
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
                scaleTween.Kill();
                pooledGameObject.transform.localScale = originScaleValue;
            });
            yield return null;
        }

        public void ReturnPooledCollectableGameObjectToPool()
        {
            foreach (var pooledGameObject in pooledCollectablesList)
            {
                if (pooledGameObject.activeInHierarchy)
                {
                    pooledGameObject.SetActive(false);
                    pooledGameObject.transform.SetParent(transform);
                }
            }
        }
        
        public void ReturnPooledPlatformGameObjectToPool()
        {
            foreach (var pooledGameObject in pooledNormalPlatformsList)
            {
                if (pooledGameObject.activeInHierarchy)
                {
                    pooledGameObject.SetActive(false);
                    pooledGameObject.transform.SetParent(transform);
                }
            }
            foreach (var pooledGameObject in pooledCheckPointPlatformsList)
            {
                if (pooledGameObject.activeInHierarchy)
                {
                    pooledGameObject.SetActive(false);
                    pooledGameObject.transform.SetParent(transform);
                }
            }
        }

        public void DestroyPooledObjects()
        {
            if (pooledCollectablesList.Count == 0) return;
            foreach (var pooledGameObject in pooledCollectablesList)
            {
                Destroy(pooledGameObject);
            }
            pooledCollectablesList.Clear();
        }

        
    }
}
