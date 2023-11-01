using System.Collections;
using Managers;
using UnityEngine;

namespace Controllers.PlayerController
{
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Player Movement Speed"),Range(0.1f,1f),Tooltip("Smaller values are slower.")]
        [SerializeField] private float playerZMovementSpeed;

        [Header("Player X Axis Lerp value"),Range(0.1f,1f),Tooltip("Smaller values are slower.")]
        [SerializeField] private float lerpValue;
        
        private bool isMouseDragging;
        private bool isGameStarted;
        private bool isMovementAllowed;
        private Camera mainCamera;
        private Rigidbody mRigidbody;

        private void Awake()
        {
            mainCamera = Camera.main;
            mRigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            mRigidbody.isKinematic = false;
        }

        private void OnEnable()
        {
            GameManager.Instance.OnStarted += OnGameStart;
            GameManager.Instance.OnWaitingInput += OnWaitingInput;
            GameManager.Instance.OnCheckPoint1 += OnNoneMovementState;
            GameManager.Instance.OnCheckPoint1Success += OnMovementState;
            GameManager.Instance.OnCheckPoint2 += OnNoneMovementState;
            GameManager.Instance.OnCheckPoint2Success += OnMovementState;
            GameManager.Instance.OnCheckPoint3 += OnNoneMovementState;
            GameManager.Instance.OnEnding += OnNoneMovementState;
        }

        private void OnDisable()
        {
            if (GameManager.Instance == null) return;
            GameManager.Instance.OnStarted -= OnGameStart;
            GameManager.Instance.OnWaitingInput -= OnWaitingInput;
            GameManager.Instance.OnCheckPoint1 -= OnNoneMovementState;
            GameManager.Instance.OnCheckPoint1Success -= OnMovementState;
            GameManager.Instance.OnCheckPoint2 -= OnNoneMovementState;
            GameManager.Instance.OnCheckPoint2Success -= OnMovementState;
            GameManager.Instance.OnCheckPoint3 -= OnNoneMovementState;
            GameManager.Instance.OnEnding -= OnNoneMovementState;
        }

        private void FixedUpdate()
        {
            PlayerMovement();
        }

        private void OnMouseDown()
        {
            if (isGameStarted) return;
            if (GameManager.Instance.SelectedGameStates == GameStates.OnWaitingInput)
            {
                GameManager.Instance.SelectedGameStates = GameStates.OnStarted;
            }
        }

        private void OnMouseUp()
        {
            isMouseDragging = false;
        }

        private void OnMouseDrag()
        {
            isMouseDragging = true;
        }

        private void OnGameStart()
        {
            mRigidbody.isKinematic = true;
            isGameStarted = true;
            isMovementAllowed = true;
        }

        private void OnWaitingInput()
        {
            isGameStarted = false;
        }
        

        private void OnNoneMovementState()
        {
            isMovementAllowed = false;
            mRigidbody.isKinematic = false;
            mRigidbody.velocity = Vector3.zero;
        }

        private void OnMovementState()
        {
            StartCoroutine(OnMovementStateCoroutine());
        }

        private IEnumerator OnMovementStateCoroutine()
        {
            yield return new WaitForSeconds(0.5f);
            if (GameManager.Instance.SelectedGameStates == GameStates.OnWaitingInput)
            {
                GameManager.Instance.SelectedGameStates = GameStates.OnStarted;
            }
            yield return null;
        }

        private void PlayerMovement()
        {
            if (!isMovementAllowed) return;
            var inputPosition = Input.mousePosition;
            var transformPosition = transform.position;
            if (isMouseDragging)
            {
                var distanceToScreen = mainCamera.WorldToScreenPoint(transformPosition).z;
                var inputPositionInWorldScreenPoint = mainCamera.ScreenToWorldPoint(new Vector3(inputPosition.x, 0f, distanceToScreen));
                var targetXPosition = new Vector3(inputPositionInWorldScreenPoint.x, transformPosition.y, transformPosition.z);
                var smoothXTargetPosition = Vector3.Lerp(transformPosition, targetXPosition, lerpValue);
                var targetZPosition = new Vector3(transformPosition.x, transformPosition.y, transformPosition.z + playerZMovementSpeed);
                var smoothTargetZPosition = Vector3.Lerp(transformPosition, targetZPosition, lerpValue);
                var targetPosition = new Vector3(smoothXTargetPosition.x, transformPosition.y, smoothTargetZPosition.z);
                mRigidbody.MovePosition(targetPosition);
            }
            else if (isGameStarted)
            {
                var targetZPosition = new Vector3(transformPosition.x, transformPosition.y, transformPosition.z + playerZMovementSpeed);
                var smoothTargetZPosition = Vector3.Lerp(transformPosition, targetZPosition, lerpValue);
                var targetPosition = new Vector3(transformPosition.x, transformPosition.y, smoothTargetZPosition.z);
                mRigidbody.MovePosition(targetPosition);
            }
        }
    }
}
