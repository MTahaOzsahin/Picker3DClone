using System;
using System.Collections.Generic;
using System.Linq;
using Controllers.CollectablesController;
using Controllers.PlatformControllers;
using Controllers.PlayerController;
using Managers;
using UnityEngine;

namespace Helpers
{
    public class ObjectPool : SingletonMB<ObjectPool>
    {
        [Header("Collectables Prefabs")]
        [SerializeField] private GameObject collectableSpherePrefabToPooled;
        [SerializeField] private GameObject collectableCubePrefabToPooled;
        [SerializeField] private GameObject collectableCapsulePrefabToPooled;
        
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
        
        private Queue<GameObject> pooledCollectableSphereQueue;
        private Queue<GameObject> pooledCollectableCubeQueue;
        private Queue<GameObject> pooledCollectableCapsuleQueue;
        private Queue<GameObject> pooledNormalPlatformsQueue; 
        private Queue<GameObject> pooledCheckPointPlatformsQueue; 
        private Queue<GameObject> pooledPlayerQueue;
        

        private Vector3 collectablesOriginScale;


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
            pooledCollectableCapsuleQueue = new Queue<GameObject>();
            pooledCollectableCubeQueue = new Queue<GameObject>();
            pooledCollectableSphereQueue = new Queue<GameObject>();
            pooledNormalPlatformsQueue = new Queue<GameObject>();
            pooledCheckPointPlatformsQueue = new Queue<GameObject>();
            pooledPlayerQueue = new Queue<GameObject>();
        }


        private void InitPools()
        {
            if (pooledCollectableSphereQueue.Count == 0)
            {
                for (int i = 0; i < targetQuantityForCollectables; i++)
                {
                    var tempCollectableGameObject = Instantiate(collectableSpherePrefabToPooled,transform);
                    tempCollectableGameObject.SetActive(false);
                    var collectableGameObjectType = tempCollectableGameObject.GetComponent<CollectableBase>().selectedCollectableType;
                    tempCollectableGameObject.name = collectableGameObjectType + $"{i + 1}";
                    pooledCollectableSphereQueue.Enqueue(tempCollectableGameObject);
                    collectablesOriginScale = tempCollectableGameObject.transform.localScale;
                }
            }
            
            if (pooledCollectableCubeQueue.Count == 0)
            {
                for (int i = 0; i < targetQuantityForCollectables; i++)
                {
                    var tempCollectableGameObject = Instantiate(collectableCubePrefabToPooled,transform);
                    tempCollectableGameObject.SetActive(false);
                    var collectableGameObjectType = tempCollectableGameObject.GetComponent<CollectableBase>().selectedCollectableType;
                    tempCollectableGameObject.name = collectableGameObjectType + $"{i + 1}";
                    pooledCollectableCubeQueue.Enqueue(tempCollectableGameObject);
                    collectablesOriginScale = tempCollectableGameObject.transform.localScale;
                }
            }
            
            if (pooledCollectableCapsuleQueue.Count == 0)
            {
                for (int i = 0; i < targetQuantityForCollectables; i++)
                {
                    var tempCollectableGameObject = Instantiate(collectableCapsulePrefabToPooled,transform);
                    tempCollectableGameObject.SetActive(false);
                    var collectableGameObjectType = tempCollectableGameObject.GetComponent<CollectableBase>().selectedCollectableType;
                    tempCollectableGameObject.name = collectableGameObjectType + $"{i + 1}";
                    pooledCollectableCapsuleQueue.Enqueue(tempCollectableGameObject);
                    collectablesOriginScale = tempCollectableGameObject.transform.localScale;
                }
            }

            if (pooledNormalPlatformsQueue.Count == 0)
            {
                for (int i = 0; i < targetQuantityForPlatforms; i++)
                {
                    var tempPlatformNormalGameObject = Instantiate(platformNormalPrefabToPooled, transform);
                    tempPlatformNormalGameObject.SetActive(false);
                    var platformNormalGameObjectType = tempPlatformNormalGameObject.GetComponent<PlatformBase>().selectedPlatformType;
                    tempPlatformNormalGameObject.name = platformNormalGameObjectType + $"{i + 1}";
                    pooledNormalPlatformsQueue.Enqueue(tempPlatformNormalGameObject);
                
                    var tempPlatformCheckPointGameObject = Instantiate(platformCheckPointPrefabToPooled, transform);
                    tempPlatformCheckPointGameObject.SetActive(false);
                    var platformCheckPointGameObjectType = tempPlatformCheckPointGameObject.GetComponent<PlatformBase>().selectedPlatformType;
                    tempPlatformCheckPointGameObject.name = platformCheckPointGameObjectType + $"{i + 1}";
                    pooledCheckPointPlatformsQueue.Enqueue(tempPlatformCheckPointGameObject);
                }
            }

            if (pooledPlayerQueue.Count == 0)
            {
                for (int i = 0; i < targetQuantityForPlayer; i++)
                {
                    var tempPlayerGameObject = Instantiate(playerPrefabToPooled, transform);
                    tempPlayerGameObject.SetActive(false);
                    pooledPlayerQueue.Enqueue(tempPlayerGameObject);
                }
            }
            
            GameManager.Instance.SelectedGameStates = GameStates.OnInitializingLevel;
        }
        
        public GameObject GetPooledCollectableGameObject(CollectableType selectedCollectableType,Transform parentGameObject)
        {
            switch (selectedCollectableType)
            {
                case CollectableType.Sphere:
                    if (pooledCollectableSphereQueue.Count == 0) return null;
                    var collectableSphereGameObject = pooledCollectableSphereQueue.First();
                    if(!collectableSphereGameObject.activeInHierarchy)
                    {
                        collectableSphereGameObject.transform.SetParent(parentGameObject);
                        collectableSphereGameObject.transform.localScale = collectablesOriginScale;
                        pooledCollectableSphereQueue.Dequeue();
                        return collectableSphereGameObject;
                    }
                    break;
                case CollectableType.Cube:
                    if (pooledCollectableCubeQueue.Count == 0) return null;
                    var collectableCubeGameObject = pooledCollectableCubeQueue.First();
                    if(!collectableCubeGameObject.activeInHierarchy)
                    {
                        collectableCubeGameObject.transform.SetParent(parentGameObject);
                        collectableCubeGameObject.transform.localScale = collectablesOriginScale;
                        pooledCollectableCubeQueue.Dequeue();
                        return collectableCubeGameObject;
                    }
                    break;
                case CollectableType.Capsule:
                    if (pooledCollectableCapsuleQueue.Count == 0) return null;
                    var collectableCapsuleGameObject = pooledCollectableCapsuleQueue.First();
                    if(!collectableCapsuleGameObject.activeInHierarchy)
                    {
                        collectableCapsuleGameObject.transform.SetParent(parentGameObject);
                        collectableCapsuleGameObject.transform.localScale = collectablesOriginScale;
                        pooledCollectableCapsuleQueue.Dequeue();
                        return collectableCapsuleGameObject;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(selectedCollectableType), selectedCollectableType, null);
            }
           
            return null;
        }
        

        public GameObject GetPooledPlatformGameObject(PlatformType selectedPlatformType, Transform parentGameObject)
        {
            switch (selectedPlatformType)
            {
                case PlatformType.Normal:
                    if (pooledNormalPlatformsQueue.Count == 0) return null;
                    var normalPlatform = pooledNormalPlatformsQueue.First();
                    if (!normalPlatform.activeInHierarchy)
                    {
                        normalPlatform.transform.SetParent(parentGameObject);
                        pooledNormalPlatformsQueue.Dequeue();
                        return normalPlatform;
                    }
                    break;
                case PlatformType.CheckPoint:
                    if (pooledCheckPointPlatformsQueue.Count == 0) return null;
                    var checkPointPlatform = pooledCheckPointPlatformsQueue.First();
                    if (!checkPointPlatform.activeInHierarchy)
                    {
                        checkPointPlatform.transform.SetParent(parentGameObject);
                        pooledCheckPointPlatformsQueue.Dequeue();
                        return checkPointPlatform;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(selectedPlatformType), selectedPlatformType, null);
            }
            return null;
        }

        public GameObject GetPooledPlayerGameObject(Transform parentGameObject)
        {
            if (pooledPlayerQueue.Count == 0) return null;
            var playerGameObject = pooledPlayerQueue.First();
            if (!playerGameObject.activeInHierarchy)
            {
                playerGameObject.transform.SetParent(parentGameObject);
                pooledPlayerQueue.Dequeue();
                return playerGameObject;
            }

            return null;
        }

        public void ReturnAnyPooledGameObjectToPool(GameObject pooledGameObject)
        {
            HandleColliderRigidBody(pooledGameObject);
            pooledGameObject.SetActive(false);
            pooledGameObject.transform.SetParent(transform);
            if (pooledGameObject.GetComponent<CollectableBase>() != null)
            {
                var pooledGameObjectBase = pooledGameObject.GetComponent<CollectableBase>();
                switch (pooledGameObjectBase.selectedCollectableType)
                {
                    case CollectableType.Sphere:
                        if (pooledCollectableSphereQueue.Contains(pooledGameObject)) return;
                        pooledCollectableSphereQueue.Enqueue(pooledGameObject);
                        break;
                    case CollectableType.Cube:
                        if (pooledCollectableCubeQueue.Contains(pooledGameObject)) return;
                        pooledCollectableCubeQueue.Enqueue(pooledGameObject);
                        break;
                    case CollectableType.Capsule:
                        if (pooledCollectableCapsuleQueue.Contains(pooledGameObject)) return;
                        pooledCollectableCapsuleQueue.Enqueue(pooledGameObject);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else if (pooledGameObject.GetComponent<PlatformNormal>() != null)
            {
                if (pooledNormalPlatformsQueue.Contains(pooledGameObject)) return;
                pooledNormalPlatformsQueue.Enqueue(pooledGameObject);
            }
            else if (pooledGameObject.GetComponent<PlatformCheckPoint>() != null)
            {
                if (pooledCheckPointPlatformsQueue.Contains(pooledGameObject)) return;
                pooledCheckPointPlatformsQueue.Enqueue(pooledGameObject);
            }
            else if (pooledGameObject.GetComponent<PlayerMovementController>() != null)
            {
                if (pooledPlayerQueue.Contains(pooledGameObject)) return;
                pooledPlayerQueue.Enqueue(pooledGameObject);
            }
        }

        private void HandleColliderRigidBody(GameObject pooledGameObject)
        {
            pooledGameObject.transform.position = new Vector3(0f, -50f, 100f);
            pooledGameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
            var mRigidBody = pooledGameObject.GetComponent<Rigidbody>();
            mRigidBody.velocity = Vector3.zero;
            mRigidBody.angularVelocity = Vector3.zero;
        }

        public void DestroyRemainPooledObjects()
        {
            if (pooledCollectableSphereQueue.Count == 0) return;
            foreach (var pooledGameObject in pooledCollectableSphereQueue)
            {
                Destroy(pooledGameObject);
            }
            pooledCollectableSphereQueue.Clear();
            
            if (pooledCollectableCubeQueue.Count == 0) return;
            foreach (var pooledGameObject in pooledCollectableCubeQueue)
            {
                Destroy(pooledGameObject);
            }
            pooledCollectableCubeQueue.Clear();
            
            if (pooledCollectableCapsuleQueue.Count == 0) return;
            foreach (var pooledGameObject in pooledCollectableCapsuleQueue)
            {
                Destroy(pooledGameObject);
            }
            pooledCollectableCapsuleQueue.Clear();
            
            if (pooledNormalPlatformsQueue.Count == 0) return;
            foreach (var pooledGameObject in pooledNormalPlatformsQueue)
            {
                Destroy(pooledGameObject);
            }
            pooledNormalPlatformsQueue.Clear();
            
            if (pooledCheckPointPlatformsQueue.Count == 0) return;
            foreach (var pooledGameObject in pooledCheckPointPlatformsQueue)
            {
                Destroy(pooledGameObject);
            }
            pooledCheckPointPlatformsQueue.Clear();
            
            if (pooledPlayerQueue.Count == 0) return;
            foreach (var pooledGameObject in pooledPlayerQueue)
            {
                Destroy(pooledGameObject);
            }
            pooledPlayerQueue.Clear();
        }
    }
}
