using System;
using System.Collections.Generic;
using Controllers.PlatformControllers;
using UnityEngine;

namespace Data.LevelData
{
    [CreateAssetMenu(fileName = "LevelData",menuName = "Level Data/New Level Data",order = 1)]
    public class LevelData : ScriptableObject
    {
        public Vector3 playerStartPosition;
        public Vector3 platformStartPosition;
        public int levelIndex;
        public int checkPoint1Target;
        public int checkPoint2Target;
        public int checkPoint3Target;
        public Texture2D collectablesCoordinateTexture;
        [Tooltip("Must be selected at least 3 checkpoint platform.As well as order must be one normal and one checkpoint." +
                 "For Collectables positions please be sure to select correct Texture map.")]public List<PlatformData> platformData;
    }

    [Serializable]
    public class PlatformData
    {
        public PlatformType platformType;
    }
}
