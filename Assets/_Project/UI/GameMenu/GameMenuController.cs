using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace EternalDefenders
{
    public class GameMenuController : MonoBehaviour
    {
        private UIDocument _doc;
        private Button _playButton;
        private Button _quitButton;
        private Button _settingsButton;
        private VisualElement _buttonsWrapper;
        private bool _isMenuVisible = true;

        [SerializeField]
        private VisualTreeAsset _settingsButtonsTemplate;
        private VisualElement _settingsButtons;

        [SerializeField]
        private VisualTreeAsset _controllsPanelTemplate;
        private VisualElement _controllsPanel;

        private void Awake()
        {
            _doc = GetComponent<UIDocument>();
            _playButton = _doc.rootVisualElement.Q<Button>("play");
            _playButton.clicked += PlayButtonOnClicked;
            _quitButton = _doc.rootVisualElement.Q<Button>("quit");
            _quitButton.clicked += QuitButtonOnClicked;
            _settingsButton = _doc.rootVisualElement.Q<Button>("settings");
            _settingsButton.clicked += SettingsButtonOnClicked;

            _buttonsWrapper = _doc.rootVisualElement.Q<VisualElement>("Buttons");

            _settingsButtons = _settingsButtonsTemplate.CloneTree();
            var backButton = _settingsButtons.Q<Button>("BackButton");
            backButton.clicked += BackButtonOnClicked;

            var ControllsButton = _settingsButtons.Q<Button>("controlls");
            ControllsButton.clicked += ControllsButtonOnClicked;

            _controllsPanel = _controllsPanelTemplate.CloneTree();
            var backControllsButton = _controllsPanel.Q<Button>("BackControllsButton");
            backControllsButton.clicked += BackControllsButtonOnclicked;

            ToggleMenu();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleMenu();
            }
        }

        private void PlayButtonOnClicked()
        {
            ToggleMenu(); 
        }

        private void QuitButtonOnClicked()
        {
            SceneManager.LoadScene("MainMenu");
        }

        private void SettingsButtonOnClicked()
        {
            _buttonsWrapper.Clear();
            _buttonsWrapper.Add(_settingsButtons);
        }

        private void BackButtonOnClicked()
        {
            _buttonsWrapper.Clear();
            _buttonsWrapper.Add(_playButton);
            _buttonsWrapper.Add(_settingsButton);
            _buttonsWrapper.Add(_quitButton);
        }

        private void ToggleMenu()
        {
            _isMenuVisible = !_isMenuVisible;
            _doc.rootVisualElement.style.display = _isMenuVisible ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void BackControllsButtonOnclicked()
        {
            _buttonsWrapper.Clear();
            _buttonsWrapper.Add(_settingsButtons);
        }

        private void ControllsButtonOnClicked()
        {
            _buttonsWrapper.Clear();
            _buttonsWrapper.Add(_controllsPanel);
        }
    }
}
