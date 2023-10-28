using Managers;
using UnityEngine;

namespace Controllers.PlatformControllers
{
    public class PlatformNormal : PlatformBase
    {
        [Header("The collider that positioned at beginning of the platform")]
        [SerializeField] private BoxCollider starterCollider;

        private void Awake()
        {
            starterCollider.enabled = false;
        }

        private void OnEnable()
        {
            GameManager.Instance.OnWaitingInput += DisableStarterCollider;
            GameManager.Instance.OnStarted += EnabledStarterCollider;
        }

        private void OnDisable()
        {
            if (GameManager.Instance == null) return;
            GameManager.Instance.OnWaitingInput -= DisableStarterCollider;
            GameManager.Instance.OnStarted -= EnabledStarterCollider;
        }

        private void DisableStarterCollider()
        {
            starterCollider.enabled = false;
        }

        private void EnabledStarterCollider()
        {
            starterCollider.enabled = true;
        }
    }
}
