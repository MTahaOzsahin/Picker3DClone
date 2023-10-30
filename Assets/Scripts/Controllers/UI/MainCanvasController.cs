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
        
        private void OnEnable()
        {
            GameManager.Instance.OnInitializingLevel += OnInitializingLevel;
            GameManager.Instance.OnStarted += OnStart;
            GameManager.Instance.OnCheckPoint1Success += OnCheckPoint1Success;
            GameManager.Instance.OnCheckPoint2Success += OnCheckPoint2Success;
            GameManager.Instance.OnCheckPoint3Success += OnCheckPoint3Success;
        }

        private void OnDisable()
        {
            if (GameManager.Instance == null) return;
            GameManager.Instance.OnInitializingLevel -= OnInitializingLevel;
            GameManager.Instance.OnStarted -= OnStart;
            GameManager.Instance.OnCheckPoint1Success -= OnCheckPoint1Success;
            GameManager.Instance.OnCheckPoint2Success -= OnCheckPoint2Success;
            GameManager.Instance.OnCheckPoint3Success -= OnCheckPoint3Success;
        }

        private void OnInitializingLevel()
        {
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
    }
}
