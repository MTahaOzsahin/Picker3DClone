using UnityEngine;

namespace Helpers
{
    public class PlayerPrefsEditor : MonoBehaviour
    {
        [Header("Player Prefs")] 
        [SerializeField] private int currentLevel;

        public void GetPlayerPrefs()
        {
            currentLevel = PlayerPrefs.GetInt("currentLevel");
        }

        public void SetPlayerPrefs()
        {
            PlayerPrefs.SetInt("currentLevel",currentLevel);
        }
    }
}
