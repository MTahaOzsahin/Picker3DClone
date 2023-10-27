using UnityEngine;

namespace Controllers.PlayerController
{
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Player Movement Speed"), SerializeField]
        private float playerMovementSpeed;

        [SerializeField] private bool isGo;


        private void FixedUpdate()
        {
            if (!isGo) return;
            transform.Translate(0,0,Time.deltaTime * playerMovementSpeed);
        }
    }
}
