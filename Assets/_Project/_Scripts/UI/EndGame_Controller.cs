using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace EternalDefenders
{
    public class EndGame_Controller : MonoBehaviour
    {
        private UIDocument _doc;

        private Label _playerDeathLabel;
        private Label _TowerDestroyedLabel;
        private Label _EnemiesKilledLabel;

        private Button _playAgainButton;
        private Button _ExitButton;

        private bool isAnimationDone;

        void Start()
        {
            _doc = GetComponent<UIDocument>();
            _doc.rootVisualElement.style.display = DisplayStyle.None;

            _playerDeathLabel = _doc.rootVisualElement.Q<Label>("Death_Player_Number");
            _TowerDestroyedLabel = _doc.rootVisualElement.Q<Label>("Destroy_Tower_Number");
            _EnemiesKilledLabel = _doc.rootVisualElement.Q<Label>("Enemies_Killed_Number");

            _playAgainButton = _doc.rootVisualElement.Q<Button>("PlayAgain");
            _playAgainButton.clicked += PlayAgainButtonOnClicked;

            _ExitButton = _doc.rootVisualElement.Q<Button>("Exit");
            _ExitButton.clicked += ExitButtonOnClicked;

            isAnimationDone = false;

            if (MainBaseController.Instance != null)
            {
                MainBaseController.Instance.OnMainBaseDestroyed += GameOver;
            }
        }

        void GameOver()
        {
            if (isAnimationDone) return; 

            isAnimationDone = true; 
            _doc.rootVisualElement.style.display = DisplayStyle.Flex;
            DisplayStatistics();
        }

        private void DisplayStatistics()
        {
            if (GameStatisticsManager.Instance == null) return;

            int playerDeaths = GameStatisticsManager.Instance.PlayerDeaths;
            int towersDestroyed = GameStatisticsManager.Instance.TowersDestroyed;
            int enemiesKilled = GameStatisticsManager.Instance.EnemiesKilled;

            StartCoroutine(AnimateValue(_playerDeathLabel, playerDeaths, 3f));
            StartCoroutine(AnimateValue(_TowerDestroyedLabel, towersDestroyed, 3f));
            StartCoroutine(AnimateValue(_EnemiesKilledLabel, enemiesKilled, 3f));
        }

        private IEnumerator AnimateValue(Label label, int targetValue, float duration)
        {
            float elapsedTime = 0f;
            int startValue = 0;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                int currentValue = Mathf.RoundToInt(Mathf.Lerp(startValue, targetValue, elapsedTime / duration));
                label.text = "   " + currentValue.ToString();
                yield return null;
            }

        }

        private void PlayAgainButtonOnClicked()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void ExitButtonOnClicked()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
