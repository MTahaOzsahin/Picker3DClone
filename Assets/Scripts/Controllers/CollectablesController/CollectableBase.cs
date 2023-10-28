using System;
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

        private Rigidbody mRigidbody;

        private void Awake()
        {
            mRigidbody = GetComponent<Rigidbody>();
        }

        public void PushCollectablesToTarget()
        {
            mRigidbody.AddForce(Vector3.forward * 50f);
        }
    }
}
