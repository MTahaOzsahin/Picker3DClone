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
            var zDistanceFromPlayer =
                LevelManager.Instance.clonePLayerGameObject.transform.position.z - transform.position.z;
            if (zDistanceFromPlayer > 30f)
            {
                ObjectPool.Instance.ReturnAnyPooledGameObjectToPool(gameObject);
            }
        }
    }
}
