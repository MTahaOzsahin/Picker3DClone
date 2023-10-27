using UnityEngine;

namespace Controllers.PlayerController
{
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Player Movement Speed")]
        [SerializeField] private float playerZMovementSpeed;
        
        
        [SerializeField] private bool isPlayerInputSubmit;

        private Camera mainCamera;

        private Vector3 previousInputPosition;

        private void Awake()
        {
            mainCamera = Camera.main;
        }


        private void FixedUpdate()
        {
            PlayerZAxisMovement();
        }
        
        private void OnMouseDown()
        {
            isPlayerInputSubmit = true;
            previousInputPosition = Input.mousePosition;
        }

        private void OnMouseDrag()
        {
            PlayerXAxisMovement();
        }

        private void PlayerZAxisMovement()
        {
            if (!isPlayerInputSubmit) return;
            transform.Translate(0f, 0, Time.deltaTime * playerZMovementSpeed);
        }
        
        private void PlayerXAxisMovement()
        {
            var inputPosition = Input.mousePosition;
            var transformPosition = transform.position;
            if (Mathf.Abs(inputPosition.x - previousInputPosition.x) > 1f)
            {
                var distanceToScreen = mainCamera.WorldToScreenPoint(transformPosition).z;
                var inputPositionInWorldScreenPoint = mainCamera.ScreenToWorldPoint(new Vector3(inputPosition.x, 0f, distanceToScreen));
                var previousInputPositionWorldScreenPoint = mainCamera.ScreenToWorldPoint(new Vector3(previousInputPosition.x, 0f, distanceToScreen));
                var differenceAlongXAxis = inputPositionInWorldScreenPoint.x - previousInputPositionWorldScreenPoint.x;
                
                var targetPosition = new Vector3(transform.position.x + differenceAlongXAxis,transformPosition.y,transformPosition.z);
                transform.position = targetPosition;
            }
            previousInputPosition = Input.mousePosition;
        }
    }
}
