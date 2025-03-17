using UnityEngine;
using UnityEngine.UIElements;

namespace EternalDefenders
{
    public class Items_Panel_Controller : MonoBehaviour
    {
        private UIDocument _doc;

        private bool _isEnabled;

        void Start()
        {
            _doc = GetComponent<UIDocument>();
            _isEnabled = false;

            _doc.rootVisualElement.style.display = DisplayStyle.None;
        }


        void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (!_isEnabled) _doc.rootVisualElement.style.display = DisplayStyle.Flex;
                else _doc.rootVisualElement.style.display = DisplayStyle.None;
                _isEnabled = !_isEnabled;
            }
        }
    }
}
