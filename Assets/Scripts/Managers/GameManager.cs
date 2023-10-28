using System;
using Helpers;
using UnityEngine;

namespace Managers
{
    public enum GameStates
    {
        Initializing,
        WaitingInput,
        Started,
        CheckPoint1,
        CheckPoint2,
        CheckPoint3,
        Ending
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
        
        
        public Action OnInitializing;
        public Action OnWaitingInput;
        public Action OnStarted;
        public Action OnCheckPoint1;
        public Action OnCheckPoint2;
        public Action OnCheckPoint3;
        public Action OnEnding;

        private void OnEnable()
        {
            SelectedGameStates = GameStates.Initializing;
        }

        /// <summary>
        /// Clearing all actions listeners.
        /// </summary>
        private void OnDisable()
        {
            OnInitializing = null;
            OnWaitingInput = null;
            OnStarted = null;
            OnCheckPoint1 = null;
            OnCheckPoint2 = null;
            OnCheckPoint3 = null;
            OnEnding = null;
        }

        private void ControlGameStates()
        {
            switch (SelectedGameStates)
            {
                case GameStates.Initializing:
                    OnInitializing?.Invoke();
                    break;
                case GameStates.WaitingInput:
                    OnWaitingInput?.Invoke();
                    break;
                case GameStates.Started:
                    OnStarted?.Invoke();
                    break;
                case GameStates.CheckPoint1:
                    OnCheckPoint1?.Invoke();
                    break;
                case GameStates.CheckPoint2:
                    OnCheckPoint2?.Invoke();
                    break;
                case GameStates.CheckPoint3:
                    OnCheckPoint3?.Invoke();
                    break;
                case GameStates.Ending:
                    OnEnding?.Invoke();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    
}
