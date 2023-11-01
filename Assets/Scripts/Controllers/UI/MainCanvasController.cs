using System;
using System.Collections;
using System.Globalization;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.UI
{
    public class MainCanvasController : MonoBehaviour
    {
        [Header("Level Progress bars")]
        [SerializeField] private Image levelProgressFirst;
        [SerializeField] private Image levelProgressSecond;
        [SerializeField] private Image levelProgressThird;

        [Header("Level Texts")] 
        [SerializeField] private TextMeshProUGUI currentLevelText;
        [SerializeField] private TextMeshProUGUI nextLevelText;

        [Header("Panels")]
        [SerializeField] private GameObject startPanel;
        [SerializeField] private GameObject endingPanel;

        [Header("Ending Panels Game Objects")]
        [SerializeField] private GameObject winCondition;
        [SerializeField] private GameObject loseCondition;

        [Header("Ending Panels Buttons")] 
        [SerializeField] private Button nextLevelButton;
        [SerializeField] private Button restartLevelButton;
        
        private void OnEnable()
        {
            nextLevelButton.onClick.AddListener(OnNextLevelButtonClick);
            restartLevelButton.onClick.AddListener(OnRestartLevelButtonClick);
            
            GameManager.Instance.OnInitializingLevel += OnInitializingLevel;
            GameManager.Instance.OnStarted += OnStart;
            GameManager.Instance.OnCheckPoint1Success += OnCheckPoint1Success;
            GameManager.Instance.OnCheckPoint2Success += OnCheckPoint2Success;
            GameManager.Instance.OnCheckPoint3Success += OnCheckPoint3Success;
            GameManager.Instance.OnEnding += OnEnding;
        }

        private void OnDisable()
        {
            nextLevelButton.onClick.RemoveAllListeners();
            restartLevelButton.onClick.RemoveAllListeners();
            
            if (GameManager.Instance == null) return;
            GameManager.Instance.OnInitializingLevel -= OnInitializingLevel;
            GameManager.Instance.OnStarted -= OnStart;
            GameManager.Instance.OnCheckPoint1Success -= OnCheckPoint1Success;
            GameManager.Instance.OnCheckPoint2Success -= OnCheckPoint2Success;
            GameManager.Instance.OnCheckPoint3Success -= OnCheckPoint3Success;
            GameManager.Instance.OnEnding -= OnEnding;
        }

        private void OnInitializingLevel()
        {
            endingPanel.SetActive(false);
            var currentLevel = LevelManager.Instance.targetLevel;
            currentLevelText.text = currentLevel.ToString(CultureInfo.InvariantCulture);
            nextLevelText.text = (currentLevel + 1).ToString(CultureInfo.InvariantCulture);
            levelProgressFirst.color = Color.white;
            levelProgressSecond.color = Color.white;
            levelProgressThird.color = Color.white;
        }

        private void OnStart()
        {
            startPanel.SetActive(false);
        }

        private void OnCheckPoint1Success()
        {
            levelProgressFirst.color = Color.green;
        }
        
        private void OnCheckPoint2Success()
        {
            levelProgressSecond.color = Color.green;
        }
        
        private void OnCheckPoint3Success()
        {
            levelProgressThird.color = Color.green;
        }

        private void OnEnding()
        {
            StartCoroutine(EndingCoroutine());
        }

        private IEnumerator EndingCoroutine()
        {
            yield return new WaitForSeconds(0.5f);
            endingPanel.SetActive(true);
            switch (LevelManager.Instance.selectedLevelProgress)
            {
                case LevelProgress.OnProgress:
                    //
                    break;
                case LevelProgress.Win:
                    winCondition.SetActive(true);
                    break;
                case LevelProgress.Lose:
                    loseCondition.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            yield return null;
        }

        private void OnNextLevelButtonClick()
        {
            LevelManager.Instance.targetLevel++;
            if (LevelManager.Instance.targetLevel == 5)
            {
                LevelManager.Instance.targetLevel = 1;
            }
            GameManager.Instance.SelectedGameStates = GameStates.OnInitializingLevel;
            winCondition.SetActive(false);
            endingPanel.SetActive(false);
            startPanel.SetActive(true);
        }

        private void OnRestartLevelButtonClick()
        {
            GameManager.Instance.SelectedGameStates = GameStates.OnInitializingLevel;
            loseCondition.SetActive(false);
            endingPanel.SetActive(false);
            startPanel.SetActive(true);
        }
    }
}
