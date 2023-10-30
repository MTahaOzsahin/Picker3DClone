using Helpers;
using UnityEngine;

namespace Controllers.DeadZone
{
    public class DeadZoneController : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            // if (other.gameObject.CompareTag("Collectables"))
            // {
            //     ObjectPool.Instance.ReturnAnyPooledGameObjectToPool(other.gameObject);
            // }
        }
    }
}
