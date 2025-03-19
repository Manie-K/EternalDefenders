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
        private int _rows = 5;
        private int _columns = 2;

        private Dictionary<(int, int), (string name, string description)> _items = new Dictionary<(int, int), (string, string)>
        {
            { (0, 0), ("Miecz Ognia", "Zadaje 50 obra¿eñ i podpala wroga.") },
            { (0, 1), ("Tarcza Obroñcy", "Zwiêksza obronê o 30 punktów.") },
            { (1, 0), ("Eliksir ¯ycia", "Przywraca 100 punktów zdrowia.") },
            { (1, 1), ("Runa Mocy", "Zwiêksza si³ê ataku o 20%.") },
            { (2, 0), ("Zatruty Sztylet", "Zadaje 25 obra¿eñ i zatruwa cel.") },
            { (2, 1), ("£uk Cienia", "Zwiêksza celnoœæ i obra¿enia dystansowe.") },
            { (3, 0), ("Amulet Œwiat³a", "Zwiêksza odpornoœæ na magiê.") },
            { (3, 1), ("Zbroja Smoka", "Zmniejsza obra¿enia o 40%.") },
            { (4, 0), ("Buty Szybkoœci", "Zwiêkszaj¹ prêdkoœæ poruszania.") },
            { (4, 1), ("Pierœcieñ Many", "Regeneruje manê o 10% szybciej.") }
        };

        void Start()
        {
            _doc = GetComponent<UIDocument>();
            _isEnabled = false;

            _doc.rootVisualElement.style.display = DisplayStyle.None;

            VisualElement itemsPanel = _doc.rootVisualElement.Q<VisualElement>("ItemsPanel");
            VisualElement itemsInfo = itemsPanel.Q<VisualElement>("ItemsInfo");
            _itemNameLabel = itemsInfo.Q<Label>("ItemName");
            _itemInfoLabel = itemsInfo.Q<Label>("ItemInformations");

            _itemNameLabel.text = "";
            _itemInfoLabel.text = "";

        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                _isEnabled = !_isEnabled;
                _doc.rootVisualElement.style.display = _isEnabled ? DisplayStyle.Flex : DisplayStyle.None;
            }

            if (_isEnabled)
            {
                CheckMouseOverItems();
            }
        }

        private void CheckMouseOverItems()
        {
            Vector2 mousePosition = Input.mousePosition;
            mousePosition.y = Screen.height - mousePosition.y; 

            bool itemFound = false;

            for (int row = 0; row < _rows; row++)
            {
                for (int col = 0; col < _columns; col++)
                {
                    VisualElement itemElement = GetItemElement(row, col);
                    if (itemElement != null && IsMouseOverElement(itemElement, mousePosition))
                    {
                        if (_items.TryGetValue((row, col), out var itemData))
                        {
                            ShowItemInfo(itemData.name, itemData.description);
                        }
                        itemFound = true;
                        break; 
                    }
                }

                if (itemFound) break;
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