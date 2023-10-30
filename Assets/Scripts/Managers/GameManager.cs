using System;
using Helpers;
using UnityEngine;

namespace Managers
{
    public enum GameStates
    {
        OnInitializingPool,
        OnInitializingLevel,
        OnWaitingInput,
        OnStarted,
        OnCheckPoint1,
        OnCheckPoint1Success,
        OnCheckPoint2,
        OnCheckPoint2Success,
        OnCheckPoint3,
        OnCheckPoint3Success,
        OnLevelAdjusting,
        OnEnding
    }
    [DefaultExecutionOrder(-10)]
    public class GameManager : SingletonMB<GameManager>
    {
        [SerializeField] private GameStates selectedGameStates;
        public GameStates SelectedGameStates
        {
            get => selectedGameStates;
            set
            {
                selectedGameStates = value;
                ControlGameStates();
            }
        }
        
        
        public Action OnInitializingPool;
        public Action OnInitializingLevel;
        public Action OnWaitingInput;
        public Action OnStarted;
        public Action OnCheckPoint1;
        public Action OnCheckPoint1Success;
        public Action OnCheckPoint2;
        public Action OnCheckPoint2Success;
        public Action OnCheckPoint3;
        public Action OnCheckPoint3Success;
        public Action OnLevelAdjusting;
        public Action OnEnding;

        private void Start()
        {
            SelectedGameStates = GameStates.OnInitializingPool;
        }

        /// <summary>
        /// Clearing all actions listeners.
        /// </summary>
        private void OnDisable()
        {
            OnInitializingPool = null;
            OnInitializingLevel = null;
            OnWaitingInput = null;
            OnStarted = null;
            OnCheckPoint1 = null;
            OnCheckPoint1Success = null;
            OnCheckPoint2 = null;
            OnCheckPoint2Success = null;
            OnCheckPoint3 = null;
            OnCheckPoint3Success = null;
            OnEnding = null;
        }

        private void ControlGameStates()
        {
            switch (SelectedGameStates)
            {
                case GameStates.OnInitializingPool:
                    OnInitializingPool?.Invoke();
                    break;
                case GameStates.OnInitializingLevel:
                    OnInitializingLevel?.Invoke();
                    break;
                case GameStates.OnWaitingInput:
                    OnWaitingInput?.Invoke();
                    break;
                case GameStates.OnStarted:
                    OnStarted?.Invoke();
                    break;
                case GameStates.OnCheckPoint1:
                    OnCheckPoint1?.Invoke();
                    break;
                case GameStates.OnCheckPoint1Success:
                    OnCheckPoint1Success?.Invoke();
                    break;
                case GameStates.OnCheckPoint2:
                    OnCheckPoint2?.Invoke();
                    break;
                case GameStates.OnCheckPoint2Success:
                    OnCheckPoint2Success?.Invoke();
                    break;
                case GameStates.OnCheckPoint3:
                    OnCheckPoint3?.Invoke();
                    break;
                case GameStates.OnCheckPoint3Success:
                    OnCheckPoint3Success?.Invoke();
                    break;
                case GameStates.OnLevelAdjusting:
                    OnLevelAdjusting?.Invoke();
                    break;
                case GameStates.OnEnding:
                    OnEnding?.Invoke();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    
}
