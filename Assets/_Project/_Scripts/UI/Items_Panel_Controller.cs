using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace EternalDefenders
{
    public class Items_Panel_Controller : MonoBehaviour
    {
        private UIDocument _doc;
        private bool _isEnabled;
        private Label _itemNameLabel;
        private Label _itemInfoLabel;

        //testowanie kupowania itemow
        private VisualElement[] _ItemsSprites;
        private List<Item> _equippedItems;

        [SerializeField]
        private Sprite icon;

        void Start()
        {
            _doc = GetComponent<UIDocument>();
            _isEnabled = false;

            _doc.rootVisualElement.style.display = DisplayStyle.None;

            VisualElement itemsPanel = _doc.rootVisualElement.Q<VisualElement>("ItemsPanel");
            VisualElement itemsInfo = itemsPanel.Q<VisualElement>("ItemsInfo");
            _itemNameLabel = itemsInfo.Q<Label>("ItemName");
            _itemInfoLabel = itemsInfo.Q<Label>("ItemInformations");

            //testowanie kupowania itemow
            _ItemsSprites = new VisualElement[10];

            for (int i = 0; i < 10; i++)
            {
                _ItemsSprites[i] = _doc.rootVisualElement.Q<VisualElement>($"Item{i + 1}_Image");
            }

            _itemNameLabel.text = "";
            _itemInfoLabel.text = "";

        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                _isEnabled = !_isEnabled;
                _doc.rootVisualElement.style.display = _isEnabled ? DisplayStyle.Flex : DisplayStyle.None;

                if(_isEnabled)
                {
                    UpdateEquippedItems();
                }
            }

            if (_isEnabled)
            {
                CheckMouseOverEquippedItems();
            }
        }

        //testowanie kupowania itemow
        private void UpdateEquippedItems()
        {
             _equippedItems = ItemManager.Instance._equippedItems;

            for (int i = 0; i < _equippedItems.Count; i++)
            {
                _ItemsSprites[i].style.backgroundImage = null;
            }

            for (int i = 0; i < _equippedItems.Count && i < _ItemsSprites.Length; i++)
            {
                 _ItemsSprites[i].style.backgroundImage = new StyleBackground(icon);           
            }
        }


        private void CheckMouseOverEquippedItems()
        {
            Vector2 mousePosition = Input.mousePosition;
            mousePosition.y = Screen.height - mousePosition.y;

            bool itemFound = false;

            for (int i = 0; i < _equippedItems.Count && i < _ItemsSprites.Length; i++)
            {
                if (_ItemsSprites[i] != null && IsMouseOverElement(_ItemsSprites[i], mousePosition))
                {
                    ShowItemInfo(_equippedItems[i].Name, _equippedItems[i].Description);
                    itemFound = true;
                    break;
                }
            }

            if (!itemFound)
            {
                ClearItemInfo();
            }
        }

        private bool IsMouseOverElement(VisualElement element, Vector2 mousePos)
        {
            Vector2 elementPos = element.worldBound.position;
            Vector2 elementSize = element.worldBound.size;

            return mousePos.x >= elementPos.x && mousePos.x <= elementPos.x + elementSize.x &&
                   mousePos.y >= elementPos.y && mousePos.y <= elementPos.y + elementSize.y;
        }

        private VisualElement GetItemElement(int row, int column)
        {
            VisualElement ItemsPanel = _doc.rootVisualElement.Q<VisualElement>("ItemsPanel");
            VisualElement Items = ItemsPanel.Q<VisualElement>("Items");
            VisualElement rowElement = Items.Q<VisualElement>($"row{row+1}");
            return rowElement?.Q<VisualElement>($"Item{column+1}");
        }

        private void ShowItemInfo(string name, string description)
        {
            _itemNameLabel.text = name;
            _itemInfoLabel.text = description;
        }

        private void ClearItemInfo()
        {
            _itemNameLabel.text = "";
            _itemInfoLabel.text = "";
        }
    }
}