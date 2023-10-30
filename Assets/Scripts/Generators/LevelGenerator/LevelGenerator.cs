using System;
using System.Collections.Generic;
using Cinemachine;
using Controllers.CollectablesController;
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
        private List<Vector3> collectablesRedPositions;
        private List<Vector3> collectablesGreenPositions;
        private List<Vector3> collectablesBluePositions;
        
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
            GetLevelData();
            TakePositionsFromMap();
            GeneratePlatform();
            GenerateCollectable();
            GeneratePlayer();
            GameManager.Instance.SelectedGameStates = GameStates.OnWaitingInput;
        }
        
        private void GetLevelData()
        {
            currentLevelData = ResourcesManager.Instance.LoadLevel(LevelManager.Instance.targetLevel);
            var levelData = currentLevelData;
            var levelManagerInstance = LevelManager.Instance;
            levelManagerInstance.checkPoint1Target = levelData.checkPoint1Target;
            levelManagerInstance.checkPoint2Target = levelData.checkPoint2Target;
            levelManagerInstance.checkPoint3Target = levelData.checkPoint3Target;
            levelManagerInstance.playerCurrentPositionInLevel = 1;
        }
        
        private void TakePositionsFromMap()
        {
            collectablesRedPositions = new List<Vector3>();
            collectablesGreenPositions = new List<Vector3>();
            collectablesBluePositions = new List<Vector3>();
            var levelMapPlatform = currentLevelData.collectablesCoordinateTexture;
            for (int x = 0; x < levelMapPlatform.width; x++)
            {
                for (int y = 0; y < levelMapPlatform.height; y++)
                {
                    var pixelColor = levelMapPlatform.GetPixel(x, y);
                    var adjustedPosition = new Vector3(x - 4, 0.75f, y - 25);
                    if (pixelColor == Color.red)
                    {
                        collectablesRedPositions.Add(adjustedPosition);
                    }
                    else if ( pixelColor == Color.green)
                    {
                        collectablesGreenPositions.Add(adjustedPosition);
                    }
                    else if ( pixelColor == Color.blue)
                    {
                        collectablesBluePositions.Add(adjustedPosition);
                    }
                }
            }
        }

        private void GeneratePlayer()
        {
            var playerStartPosition = currentLevelData.playerStartPosition;
            var rotationToQuaternion = Quaternion.Euler(new Vector3(0f, 180f, 0f));
            var clonePlayerGameObject = Instantiate(playerPrefab, playerStartPosition, rotationToQuaternion,transform);
            cineMachineVirtualCamera.Follow = clonePlayerGameObject.transform;
            LevelManager.Instance.clonePLayerGameObject = clonePlayerGameObject;
        }

        private void GenerateCollectable()
        {
            foreach (var collectableColorPosition in collectablesRedPositions)
            {
                var collectableGameObject = ObjectPool.Instance.GetPooledCollectableGameObject(CollectableType.Sphere,transform);
                collectableGameObject.SetActive(true);
                collectableGameObject.transform.position = collectableColorPosition;
            }
            
            foreach (var collectableColorPosition in collectablesGreenPositions)
            {
                var collectableGameObject = ObjectPool.Instance.GetPooledCollectableGameObject(CollectableType.Cube,transform);
                collectableGameObject.SetActive(true);
                collectableGameObject.transform.position = collectableColorPosition;
            }
            
            foreach (var collectableColorPosition in collectablesBluePositions)
            {
                var collectableGameObject = ObjectPool.Instance.GetPooledCollectableGameObject(CollectableType.Capsule,transform);
                collectableGameObject.SetActive(true);
                collectableGameObject.transform.position = collectableColorPosition;
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
    }
}
