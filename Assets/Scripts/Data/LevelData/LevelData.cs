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
        public List<PlatformData> platformData;
        public List<CollectableData> collectableData;
    }

    [Serializable]
    public class PlatformData
    {
        public Vector3 position;
        public PlatformType platformType;
        public int checkPointCount;
    }
    
    [Serializable]
    public class CollectableData
    {
        public Vector3 position;
        public CollectableType ballPackType;
    }
}
