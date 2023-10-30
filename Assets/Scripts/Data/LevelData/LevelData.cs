using System;
using System.Collections.Generic;
using Controllers.CollectablesController;
using Controllers.PlatformControllers;
using UnityEngine;

namespace Data.LevelData
{
    [CreateAssetMenu(fileName = "LevelData",menuName = "Level Data/New Level Data",order = 1)]
    public class LevelData : ScriptableObject
    {
        public Vector3 playerStartPosition;
        public int levelIndex;
        public int checkPoint1Target;
        public int checkPoint2Target;
        public int checkPoint3Target;
        public Texture2D collectablesCoordinateTexture;
        public List<PlatformData> platformData;
        // public List<CollectableData> collectableData;
    }

    [Serializable]
    public class PlatformData
    {
        public Vector3 position;
        public PlatformType platformType;
        public int platformCheckPointIndex;
    }
    
    // [Serializable]
    // public class CollectableData
    // {
    //     public Vector3 position;
    //     public CollectableType ballPackType;
    // }
}
