using System;
using MG_Utilities;
using UnityEditor;
using UnityEngine;

namespace EternalDefenders
{
    public class GameManager : Singleton<GameManager>
    {
        //TODO everything 
        #region Fields
        
        /// <summary>
        /// Time in minutes
        /// </summary>
        [SerializeField] private float _gameLength;
        [SerializeField] private int _wavePower;

        private float _endGameTimeRemaining;
        public event Action OnGameOver;

        #endregion

        #region Properties

        public float EndGameTimeRemaining
        {
            get { return _endGameTimeRemaining; }
        }
        public float GameLength
        {
            get { return _gameLength; }
        }
        public int WavePower 
        { 
            get { return _wavePower; }
            set { _wavePower = value; }
        }

        #endregion

        void Start()
        {
            Time.timeScale = 1f;
            _endGameTimeRemaining = GameLength * 60;

            MainBaseController.Instance.OnMainBaseDestroyed += GameOver;
        }

        private void Update()
        {
            _endGameTimeRemaining -= Time.deltaTime;
            _endGameTimeRemaining = Math.Max(EndGameTimeRemaining, 0f);

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
