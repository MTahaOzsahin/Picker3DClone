using Data.LevelData;
using Helpers;
using UnityEngine;

namespace Managers
{
    public class ResourcesManager : Singleton<ResourcesManager>
    {
        private const string LevelPath = "Levels/LevelData";
        protected override void Init()
        {
            //
        }
        
        public LevelData LoadLevel(int levelIndex)
        {
            return Resources.Load<LevelData>(LevelPath + levelIndex);
        }
    }
}
