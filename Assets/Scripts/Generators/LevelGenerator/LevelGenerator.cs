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
        [SerializeField] private CinemachineVirtualCamera cineMachineVirtualCamera;
        private LevelData currentLevelData;
        private List<Vector3> collectablesRedPositions;
        private List<Vector3> collectablesGreenPositions;
        private List<Vector3> collectablesBluePositions;
        private Vector3[] startPositionsArray;

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
            DestroyCurrentLevel();
            GetLevelData();
            // TakePositionsFromMap();
            TakePositionsFromMap();
            GeneratePlatform();
            GenerateCollectable();
            GeneratePlayer();
            GameManager.Instance.SelectedGameStates = GameStates.OnWaitingInput;
        }

        private void DestroyCurrentLevel()
        {
            var childGameObjectCount = transform.childCount;
            if (childGameObjectCount == 0)return;
            for (int i = 0; i < childGameObjectCount; i++)
            {
                ObjectPool.Instance.ReturnAnyPooledGameObjectToPool(transform.GetChild(0).gameObject);
            }
        }
        
        private void GetLevelData()
        {
            currentLevelData = ResourcesManager.Instance.LoadLevel(LevelManager.Instance.CurrentLevel);
            var levelData = currentLevelData;
            var levelManagerInstance = LevelManager.Instance;
            levelManagerInstance.checkPoint1Target = levelData.checkPoint1Target;
            levelManagerInstance.checkPoint2Target = levelData.checkPoint2Target;
            levelManagerInstance.checkPoint3Target = levelData.checkPoint3Target;
            levelManagerInstance.PlayerCurrentPositionInLevel = 1;
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
                    var adjustedPosition = new Vector3(x/2f - 4.5f, 0.75f, y - 25);
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
            var clonePlayerGameObject = ObjectPool.Instance.GetPooledPlayerGameObject(transform);
            clonePlayerGameObject.transform.position = playerStartPosition;
            clonePlayerGameObject.transform.rotation = rotationToQuaternion;
            clonePlayerGameObject.SetActive(true);
            clonePlayerGameObject.transform.SetAsFirstSibling();
            cineMachineVirtualCamera.Follow = clonePlayerGameObject.transform;
            LevelManager.Instance.ClonePlayerGameObject = clonePlayerGameObject;
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
            int checkPointPlatformCount = 0;
            startPositionsArray = new Vector3[platformData.Count];
            startPositionsArray[0] = currentLevelData.platformStartPosition;
            for (int i = 0; i < platformData.Count; i++)
            {
                var selectedPlatformType = platformData[i].platformType;
                var tempPlatformGameObject = ObjectPool.Instance.GetPooledPlatformGameObject(selectedPlatformType, transform);
                float baseZOffset;
                switch (selectedPlatformType)
                {
                    case PlatformType.Normal:
                        baseZOffset = 30;
                        break;
                    case PlatformType.CheckPoint:
                        checkPointPlatformCount++;
                        tempPlatformGameObject.GetComponent<PlatformCheckPoint>().InitCheckPointPlatform(checkPointPlatformCount);
                        baseZOffset = 40;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                tempPlatformGameObject.transform.position = startPositionsArray[i];
                tempPlatformGameObject.SetActive(true);
                if (i == platformData.Count - 1) break;
                var previousPositionZ = startPositionsArray[i].z;
                startPositionsArray[i + 1] = new Vector3(0f, 0f, previousPositionZ + baseZOffset);
            }
        }
    }
}
