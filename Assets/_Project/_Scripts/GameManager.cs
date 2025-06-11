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
        public float EndGameTimeRemaining;

        public float GameLength = 20; // in minutes

        public event Action OnGameOver;

        void Start()
        {
            Time.timeScale = 1f;
            EndGameTimeRemaining = GameLength * 60;

            MainBaseController.Instance.OnMainBaseDestroyed += GameOver;
        }

        private void Update()
        {
            EndGameTimeRemaining -= Time.deltaTime;
            EndGameTimeRemaining = Math.Max(EndGameTimeRemaining, 0f);

            if (EndGameTimeRemaining <= 0)
            {
                GameOver();
            }
        }

        void GameOver()
        {
            OnGameOver.Invoke();
            Debug.Log("======= Game Over =======");
            PauseTime(); 
            //EditorApplication.isPaused = true;
        }
        
        public void PauseTime() => Time.timeScale = 0f;
        public void ResumeTime() => Time.timeScale = 1f;
    }
}
