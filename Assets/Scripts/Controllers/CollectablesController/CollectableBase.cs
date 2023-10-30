using System;
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
            var zDistanceFromPlayer =
                LevelManager.Instance.clonePLayerGameObject.transform.position.z - transform.position.z;
            if (zDistanceFromPlayer > 10f)
            {
                ObjectPool.Instance.ReturnAnyPooledGameObjectToPool(gameObject);
            }
        }
    }
}
