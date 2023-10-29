using System;
using Controllers.PlatformControllers;
using Data.LevelData;
using Helpers;
using Managers;
using UnityEngine;

namespace Generators.LevelGenerator
{
    public class LevelGenerator : MonoBehaviour
    {
        private int levelIndex;
        private Vector3 playerStartPosition;

        public LevelData exampleLevelData;

        private void OnEnable()
        {
            GameManager.Instance.OnInitializingLevel += GenerateLevel;
        }

        private void OnDisable()
        {
            if (GameManager.Instance == null) return;
            GameManager.Instance.OnInitializingLevel -= GenerateLevel;
        }

        private void GenerateLevel()
        {
            GeneratePlatform();
            GenerateCollectable();
        }

        private void GenerateCollectable()
        {
            var collectablesData = exampleLevelData.collectableData;
            foreach (var collectable in collectablesData)
            {
                var collectableGameObject = ObjectPool.Instance.GetPooledCollectableGameObject(transform);
                collectableGameObject.SetActive(true);
                collectableGameObject.transform.position = collectable.position;
            }
        }

        private void GeneratePlatform()
        {
            var platformData = exampleLevelData.platformData;
            foreach (var platform in platformData)
            {
                switch (platform.platformType)
                {
                    case PlatformType.Normal:
                        var platformNormalGameObject = ObjectPool.Instance.GetPooledPlatformNormalGameObject(transform);
                        platformNormalGameObject.SetActive(true);
                        platformNormalGameObject.transform.position = platform.position;
                        break;
                    case PlatformType.CheckPoint:
                        var platformCheckPointGameObject = ObjectPool.Instance.GetPooledPlatformCheckPointGameObject(transform);
                        platformCheckPointGameObject.SetActive(true);
                        platformCheckPointGameObject.transform.position = platform.position;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
