using Helpers;
using Managers;
using UnityEngine;

namespace Controllers.PlatformControllers
{
    public enum PlatformType
    {
        Normal,
        CheckPoint
    }
    public class PlatformBase : MonoBehaviour
    {
        public PlatformType selectedPlatformType;

        private void Update()
        {
            if (LevelManager.Instance.ClonePlayerGameObject == null) return;
            var zDistanceFromPlayer =
                LevelManager.Instance.ClonePlayerGameObject.transform.position.z - transform.position.z;
            if (zDistanceFromPlayer > 30f)
            {
                ObjectPool.Instance.ReturnAnyPooledGameObjectToPool(gameObject);
            }
        }
    }
}
