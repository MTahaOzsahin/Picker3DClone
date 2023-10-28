using System;
using System.Collections.Generic;
using Controllers.CollectablesController;
using Managers;
using UnityEngine;

namespace Controllers.PlayerController
{
    public class PlayerCollectorController : MonoBehaviour
    {
        private List<GameObject> collectedGameObjects;

        private void Awake()
        {
            collectedGameObjects = new List<GameObject>();
        }
        
        private void OnEnable()
        {
            GameManager.Instance.OnCheckPoint1 += OnCheckPoint;
            GameManager.Instance.OnCheckPoint2 += OnCheckPoint;
            GameManager.Instance.OnCheckPoint3 += OnCheckPoint;
        }

        private void OnDisable()
        {
            if (GameManager.Instance == null) return;
            GameManager.Instance.OnCheckPoint1 -= OnCheckPoint;
            GameManager.Instance.OnCheckPoint2 -= OnCheckPoint;
            GameManager.Instance.OnCheckPoint3 -= OnCheckPoint;
        }

        private void OnCheckPoint()
        {
            foreach (var collectedGameObject in collectedGameObjects)
            {
                collectedGameObject.GetComponent<CollectableBase>().PushCollectablesToTarget();
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Collectables"))
            {
                if (!collectedGameObjects.Contains(other.gameObject))
                {
                    collectedGameObjects.Add(other.gameObject);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Collectables"))
            {
                if (collectedGameObjects.Contains(other.gameObject))
                {
                    collectedGameObjects.Remove(other.gameObject);
                }
            }
        }
    }
}
