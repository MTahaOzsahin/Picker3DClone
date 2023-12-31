using System;
using System.Collections;
using System.Globalization;
using DG.Tweening;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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

        [Header("Start Panel Game Objects")]
        [SerializeField] private GameObject dragToStartImage;

        [Header("Ending Panels Game Objects")]
        [SerializeField] private GameObject winCondition;
        [SerializeField] private GameObject loseCondition;

        [Header("Ending Panels Buttons")] 
        [SerializeField] private Button nextLevelButton;
        [SerializeField] private Button restartLevelButton;

        [Header("Game Panel CheckPoint Success GameObject List")] 
        [SerializeField] private GameObject checkPointSuccessAreaGameObject;

        private Tweener scaleTween;

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
            var currentLevel = LevelManager.Instance.CurrentLevel;
            currentLevelText.text = currentLevel.ToString(CultureInfo.InvariantCulture);
            nextLevelText.text = (currentLevel + 1).ToString(CultureInfo.InvariantCulture);
            levelProgressFirst.color = Color.white;
            levelProgressSecond.color = Color.white;
            levelProgressThird.color = Color.white;
            
            scaleTween = dragToStartImage.transform.DOScale(new Vector3(1.1f, 1.1f, 1f), 0.8f)
                .SetEase(Ease.Linear).SetLoops(-1,LoopType.Yoyo);
        }

        private void OnStart()
        {
            if (scaleTween.IsActive())
            {
                scaleTween.Kill();
            }
            startPanel.SetActive(false);
        }

        private void OnCheckPoint1Success()
        {
            SuccessTextHandler();
            levelProgressFirst.color = Color.green;
        }
        

        private void OnCheckPoint2Success()
        {
            SuccessTextHandler();
            levelProgressSecond.color = Color.green;
        }
        
        private void OnCheckPoint3Success()
        {
            SuccessTextHandler();
            levelProgressThird.color = Color.green;
        }
        
        private void SuccessTextHandler()
        {
            var successTextList = checkPointSuccessAreaGameObject.GetComponentsInChildren<TextMeshProUGUI>(true);
            var randomIndex = Random.Range(0, successTextList.Length);
            var successText = successTextList[randomIndex];
            var parentGameObject = successText.transform.parent;
            parentGameObject.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            parentGameObject.gameObject.SetActive(true);
            var scaleTextTween = parentGameObject.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.6f);
            scaleTextTween.OnComplete(() =>
            {
                parentGameObject.gameObject.SetActive(false);
                scaleTextTween.Kill();
            });
        }

        private void OnEnding()
        {
            StartCoroutine(EndingCoroutine());
        }

        private IEnumerator EndingCoroutine()
        {
            yield return new WaitForSeconds(0.6f);
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
            LevelManager.Instance.CurrentLevel++;
            if (LevelManager.Instance.CurrentLevel == 11)
            {
                LevelManager.Instance.CurrentLevel = 1;
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
