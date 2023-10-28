using System;
using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers.PlatformControllers
{
    public enum CheckPointIndex
    {
        First,
        Second,
        Third
    }
    public class PlatformCheckPoint : PlatformBase
    {
        public CheckPointIndex selectedCheckPointIndex;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                GameManager.Instance.SelectedGameStates = GameStates.CheckPoint1;
                StartCoroutine(TempCoroutine());
            }
        }

        private IEnumerator TempCoroutine()
        {
            yield return new WaitForSeconds(5f);
            var activeScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(activeScene);
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Collectables"))
            {
                var levelManagerInstance = LevelManager.Instance;
                if (!levelManagerInstance.playerCollectedGameObjects.Contains(other.gameObject))
                {
                    levelManagerInstance.playerCollectedGameObjects.Add(other.gameObject);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Collectables"))
            {
                var levelManagerInstance = LevelManager.Instance;
                if (levelManagerInstance.playerCollectedGameObjects.Contains(other.gameObject))
                {
                    levelManagerInstance.playerCollectedGameObjects.Remove(other.gameObject);
                }
            }
        }
    }
}
