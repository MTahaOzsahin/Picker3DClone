using Helpers;
using Managers;
using UnityEngine;

namespace Controllers.CollectablesController
{
    public enum CollectableType
    {
        Sphere,
        Cube,
        Capsule
    }
    public class CollectableBase : MonoBehaviour
    {
        public CollectableType selectedCollectableType;

        [Header("Push strength"),Range(1f,500f)] 
        [SerializeField] private float pushStrength;
        
        private Rigidbody mRigidbody;
        
        private void Awake()
        {
            mRigidbody = GetComponent<Rigidbody>();
        }

        public void PushCollectablesToTarget()
        {
            mRigidbody.AddForce(Vector3.forward * pushStrength);
        }

        private void Update()
        {
            var playerCloneGameObject = LevelManager.Instance.clonePLayerGameObject;
            if (playerCloneGameObject == null) return;
            var playerCloneGameObjectPosition = playerCloneGameObject.transform.position;
            var thisTransformPosition = transform.position;
            var zDistanceFromPlayer =
                playerCloneGameObjectPosition.z - thisTransformPosition.z;
            var yDistanceFromPlayer =
                playerCloneGameObjectPosition.y - thisTransformPosition.y;
            if (zDistanceFromPlayer > 10f)
            {
                ObjectPool.Instance.ReturnAnyPooledGameObjectToPool(gameObject);
            }

            if (yDistanceFromPlayer > 30f)
            {
                ObjectPool.Instance.ReturnAnyPooledGameObjectToPool(gameObject);
            }
        }
    }
}
