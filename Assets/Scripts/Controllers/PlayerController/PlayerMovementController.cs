using System;
using UnityEngine;

namespace Controllers.PlayerController
{
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Player Movement Speed")]
        [SerializeField] private float playerZMovementSpeed;
        
        [SerializeField] private bool isPlayerInputSubmit;
        
        [Header("Player X Axis Lerp value"),Range(0.1f,1f)]
        [SerializeField] private float lerpValue;

        private bool isMouseDragging;
        
        private Camera mainCamera;

        private Vector3 previousInputPosition;

        private Rigidbody mRigidbody;

        private void Awake()
        {
            mainCamera = Camera.main;
            mRigidbody = GetComponent<Rigidbody>();
        }


        private void FixedUpdate()
        {
            PlayerZAxisMovement();
            PlayerXAxisMovement();
        }
        
        private void OnMouseDown()
        {
            // isPlayerInputSubmit = true;
            previousInputPosition = Input.mousePosition;
        }

        private void OnMouseUp()
        {
            isMouseDragging = false;
        }

        private void OnMouseDrag()
        {
            isMouseDragging = true;
        }

        private void PlayerZAxisMovement()
        {
            if (!isPlayerInputSubmit) return;
            mRigidbody.velocity = new Vector3(0f, 0, Time.fixedDeltaTime * playerZMovementSpeed * 10f);
        }
        
        private void PlayerXAxisMovement()
        {
            if(!isMouseDragging) return;
            var inputPosition = Input.mousePosition;
            var transformPosition = transform.position;
            if (Mathf.Abs(inputPosition.x - previousInputPosition.x) > 0.5f)
            {
                var distanceToScreen = mainCamera.WorldToScreenPoint(transformPosition).z;
                var inputPositionInWorldScreenPoint = mainCamera.ScreenToWorldPoint(new Vector3(inputPosition.x, 0f, distanceToScreen));
                var targetPosition = new Vector3(inputPositionInWorldScreenPoint.x, transformPosition.y,
                    transformPosition.z);
                var smoothTargetPosition = Vector3.Lerp(transformPosition, targetPosition, lerpValue);
                mRigidbody.MovePosition(smoothTargetPosition);
            }
            previousInputPosition = Input.mousePosition;
        }
    }
}
