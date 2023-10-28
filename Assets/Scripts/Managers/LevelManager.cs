using System;
using System.Collections.Generic;
using Helpers;
using UnityEngine;

namespace Managers
{
    public class LevelManager : SingletonMB<LevelManager>
    {
        public List<GameObject> playerCollectedGameObjects;

        private void Awake()
        {
            playerCollectedGameObjects = new List<GameObject>();
        }

        private void OnEnable()
        {
            GameManager.Instance.OnCheckPoint1 += OnCheckPoint1;
            GameManager.Instance.OnCheckPoint2 += OnCheckPoint2;
            GameManager.Instance.OnCheckPoint3 += OnCheckPoint3;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnCheckPoint1 -= OnCheckPoint1;
            GameManager.Instance.OnCheckPoint2 -= OnCheckPoint2;
            GameManager.Instance.OnCheckPoint3 -= OnCheckPoint3;
        }

        private void OnCheckPoint1()
        {
            
        }
        
        private void OnCheckPoint2()
        {
            
        }
        
        private void OnCheckPoint3()
        {
            
        }
    }
}
