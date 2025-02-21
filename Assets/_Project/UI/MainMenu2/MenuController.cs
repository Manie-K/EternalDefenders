using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace EternalDefenders
{
    public class MenuController : MonoBehaviour
    {
        private UIDocument _doc;
        private Button _playButton;
        private Button _exitButton;
        private Button _settingsButton;
        private VisualElement _buttonsWrapper;

        [SerializeField]
        private VisualTreeAsset _settingsButtonsTemplate;
        private VisualElement _settingsButtons;

        [Header("Mute Button")]
        [SerializeField]
        private Texture2D _mutedTexture;
        [SerializeField]
        private Texture2D _unmutedTexture;
        private bool _muted;

        private Button _muteButton;
        private void Awake()
        {
            _doc = GetComponent<UIDocument>();
            _playButton = _doc.rootVisualElement.Q<Button>("PlayButton");
            _playButton.clicked += PlayButtonOnClicked;
            _exitButton = _doc.rootVisualElement.Q<Button>("ExitButton");
            _exitButton.clicked += ExitButtonOnClicked;
            _settingsButton = _doc.rootVisualElement.Q<Button>("SettingsButton");
            _settingsButton.clicked += SettingsButtonOnClicked;
            _muteButton = _doc.rootVisualElement.Q<Button>("MuteButton");
            _muteButton.clicked += MuteButtonOnClicked;            

            _buttonsWrapper = _doc.rootVisualElement.Q<VisualElement>("Buttons");

            _settingsButtons = _settingsButtonsTemplate.CloneTree();
            var backButton = _settingsButtons.Q<Button>("BackButton");
            backButton.clicked += BackButtonOnclicked;
        }

        private void PlayButtonOnClicked()
        {
            SceneManager.LoadScene("SampleScene");
        }

        private void ExitButtonOnClicked()
        {
            Application.Quit();
        }

        private void SettingsButtonOnClicked()
        {
            _buttonsWrapper.Clear();
            _buttonsWrapper.Add(_settingsButtons);
        }
        private void BackButtonOnclicked()
        {
            _buttonsWrapper.Clear();
            _buttonsWrapper.Add(_playButton);
            _buttonsWrapper.Add(_settingsButton);
            _buttonsWrapper.Add(_exitButton);
        }
        private void MuteButtonOnClicked()
        {
            _muted = !_muted;

            var bg = _muteButton.style.backgroundImage;
            bg.value = Background.FromTexture2D(_muted ? _mutedTexture : _unmutedTexture);
            _muteButton.style.backgroundImage = bg;

            AudioListener.volume = _muted ? 0 : 1;
        }
    }
}
