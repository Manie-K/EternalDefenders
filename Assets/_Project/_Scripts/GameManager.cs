using System;
using MG_Utilities;
using UnityEditor;
using UnityEngine;

namespace EternalDefenders
{
    public class GameManager : Singleton<GameManager>
    {
        //TODO everything 
        public int WavePower { get; set; } = 3;

        void Start()
        {
            MainBaseController.Instance.OnMainBaseDestroyed += GameOver;
        }

        void GameOver()
        {
            Debug.Log("======= Game Over =======");
            PauseTime();
            EditorApplication.isPaused = true;
        }
        
        public void PauseTime() => Time.timeScale = 0f;
        public void ResumeTime() => Time.timeScale = 1f;
    }
}
