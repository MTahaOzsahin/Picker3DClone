using System.Collections.Generic;
using Controllers.CollectablesController;
using UnityEngine;

namespace Helpers
{
    public class ObjectPool : SingletonMB<ObjectPool>
    {
        [Header("Collectables Prefabs"), Tooltip("Place any collectable to instantiate")]
        [SerializeField] private List<GameObject> prefabsToPooled;
        
        [HideInInspector]
        public List<GameObject> pooledGameObjectsList;
        
        [Header("How many collectables wanted to instantiate")]
        public int targetQuantityForCollectables;

        private void Start()
        {
            
            pooledGameObjectsList = new List<GameObject>();
            for (int i = 0; i < targetQuantityForCollectables; i++)
            {
                var randomIndex = Random.Range(1, prefabsToPooled.Count);
                var tempGameObject = Instantiate(prefabsToPooled[randomIndex]);
                tempGameObject.SetActive(false);
                var gameObjectType = tempGameObject.GetComponent<CollectableBase>().SelectedCollectableType;
                tempGameObject.name = gameObjectType + $"{i + 1}";
                pooledGameObjectsList.Add(tempGameObject);
            }
        }
        
        public GameObject GetPooledObject(GameObject parentGameObject)
        {
            for(int i = 0; i < targetQuantityForCollectables; i++)
            {
                if(!pooledGameObjectsList[i].activeInHierarchy)
                {
                    pooledGameObjectsList[i].transform.SetParent(parentGameObject.transform);
                    return pooledGameObjectsList[i];
                }
            }
            return null;
        }

        public void ReturnPooledObjectToPool()
        {
            foreach (var pooledGameObject in pooledGameObjectsList)
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
            if (pooledGameObjectsList.Count == 0) return;
            foreach (var pooledGameObject in pooledGameObjectsList)
            {
                Destroy(pooledGameObject);
            }
            pooledGameObjectsList.Clear();
        }

        
    }
}
