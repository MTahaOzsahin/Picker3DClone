using System;
using Cinemachine;
using Controllers.PlatformControllers;
using Data.LevelData;
using Helpers;
using Managers;
using UnityEngine;

namespace Generators.LevelGenerator
{
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private CinemachineVirtualCamera cineMachineVirtualCamera;
        private LevelData currentLevelData;

        private void Awake()
        {
            currentLevelData = ResourcesManager.Instance.LoadLevel(1);
        }

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
            GeneratePlayer();
            GetLevelData();
            GameManager.Instance.SelectedGameStates = GameStates.OnWaitingInput;
        }

        private void GeneratePlayer()
        {
            var playerStartPosition = currentLevelData.playerStartPosition;
            var rotationToQuaternion = Quaternion.Euler(new Vector3(0f, 180f, 0f));
            var clonePlayerGameObject = Instantiate(playerPrefab, playerStartPosition, rotationToQuaternion,transform);
            cineMachineVirtualCamera.Follow = clonePlayerGameObject.transform;
        }

        private void GenerateCollectable()
        {
            var collectablesData = currentLevelData.collectableData;
            foreach (var collectable in collectablesData)
            {
                var collectableGameObject = ObjectPool.Instance.GetPooledCollectableGameObject(transform);
                collectableGameObject.SetActive(true);
                collectableGameObject.transform.position = collectable.position;
            }
        }

        private void GeneratePlatform()
        {
            var platformData = currentLevelData.platformData;
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
                        platformCheckPointGameObject.GetComponent<PlatformCheckPoint>().InitCheckPointPlatform(platform.platformCheckPointIndex);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void GetLevelData()
        {
            var levelData = currentLevelData;
            var levelManagerInstance = LevelManager.Instance;
            levelManagerInstance.levelIndex = levelData.levelIndex;
            levelManagerInstance.checkPoint1Target = levelData.checkPoint1Target;
            levelManagerInstance.checkPoint2Target = levelData.checkPoint2Target;
            levelManagerInstance.checkPoint3Target = levelData.checkPoint3Target;
            levelManagerInstance.playerCurrentPositionInLevel = 1;
        }
    }
}
