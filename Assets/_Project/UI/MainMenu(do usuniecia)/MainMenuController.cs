using UnityEngine;
using UnityEngine.UIElements;

namespace EternalDefenders
{
    public class MainMenuController : MonoBehaviour
    {
        public VisualElement ui;

        public Button playButton;
        public Button optionsButton;
        public Button quitButton;

        private void Awake()
        {
            ui = GetComponent<UIDocument>().rootVisualElement;
        }

        private void OnEnable()
        {
            playButton=ui.Q<Button>("PlayButton");
            playButton.clicked += OnPlayButtonClicked;

            optionsButton = ui.Q<Button>("OptionsButton");
            optionsButton.clicked += OnOptionsButtonClicked;

            quitButton = ui.Q<Button>("QuitButton");
            quitButton.clicked += OnQuitButtonClicked;
        }

        private void OnQuitButtonClicked()
        {
            Application.Quit();

        }
        private void OnOptionsButtonClicked()
        {
            Debug.Log("Options!");
        }

        private void OnPlayButtonClicked()
        {
            gameObject.SetActive(false);
        }
    }

}
