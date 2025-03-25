using System;
using System.Collections.Generic;
using MG_Utilities;
using UnityEngine;
using UnityEngine.UIElements;
using static EternalDefenders.TowerBundle;

namespace EternalDefenders
{
    public class Tower_Building_Panel_Controller : Singleton<Tower_Building_Panel_Controller>
    {
        private UIDocument _doc;

        private Button[] _towerBuyButtons;
        private Label[,] _towerPriceLabels;


        private Button[] _ItemsBuyButtons;
        private Label[,] _ItemsNamesLabels;
        private Label[,] _ItemsPriceLabels;
        private Label[,] _ItemsDescriptionsLabels;
        private VisualElement[,] _ItemsSprites;

        private Button ItemsPanel_Next;
        private Button ItemsPanel_Back;
        private Label ItemsPanel_PageNumber;

        private int AcctualPage;
        private int SizeOfItems;
        private ItemDatabase _ItemsDatabase;

        //main
        private Button Towers_Button;
        private Button Items_Button;

        [SerializeField]
        private VisualTreeAsset _TowersContener_Template;
        private VisualElement _TowersContener;

        [SerializeField]
        private VisualTreeAsset _ItemsContener_Template;
        private VisualElement _ItemsContener;

        [SerializeField] private List<TowerBundle> towerBundles;
        public event Action<TowerBundle> OnBuildingSelected;




        //testowanie kupowania itemow
        [SerializeField]
        private ResourceSO res_wood;
        [SerializeField]
        private ResourceSO res_stone;
        [SerializeField]
        private Sprite icon;

        private VisualElement[] _ItemsRows;


        void Start()
        {
            _doc = GetComponent<UIDocument>();

            Towers_Button = _doc.rootVisualElement.Q<Button>("TowersContener_Panel");
            Towers_Button.clicked += OnTowersButtonClicked;
            Items_Button = _doc.rootVisualElement.Q<Button>("ItemsContener_Panel");
            Items_Button.clicked += OnItemsButtonClicked;

            _TowersContener = _TowersContener_Template.CloneTree();

            _ItemsContener = _ItemsContener_Template.CloneTree();

            InitializeItemsPanel();

            AcctualPage = 0;
            _ItemsDatabase = ItemManager.Instance.ItemDictionary;
            SizeOfItems = _ItemsDatabase.Items.Count;

            _towerBuyButtons = new Button[]
            {
                _TowersContener.Q<Button>("Fire_Tower_Button"),
                _TowersContener.Q<Button>("Electric_Tower_Button"),
                _TowersContener.Q<Button>("Ice_Tower_Button"),
                _TowersContener.Q<Button>("WoodMill_Button"),
                _TowersContener.Q<Button>("StoneMill_Button")
            };

            _towerPriceLabels = new Label[,]
            {
                {
                     _TowersContener.Q<Label>("Fire_Wood_Price"),
                     _TowersContener.Q<Label>("Fire_Stone_Price")
                },
                {
                     _TowersContener.Q<Label>("Electric_Wood_Price"),
                     _TowersContener.Q<Label>("Electric_Stone_Price")
                },
                {
                     _TowersContener.Q<Label>("Ice_Wood_Price"),
                     _TowersContener.Q<Label>("Ice_Stone_Price")
                },
                {
                     _TowersContener.Q<Label>("WoodMill_Wood_Price"),
                     _TowersContener.Q<Label>("WoodMill_Stone_Price")
                },
                {
                    _TowersContener.Q<Label>("StoneMill_Wood_Price"),
                    _TowersContener.Q<Label>("StoneMill_Stone_Price")
                }
            };

            for (int i = 0; i < _towerBuyButtons.Length; i++)
            {
                int index = i;
                _towerBuyButtons[i].clicked += () => TryBuyBuilding(towerBundles[index],
                    towerBundles[index].cost[0].amount, towerBundles[index].cost[1].amount);
            }

            SetTowerPrices();

            _doc.rootVisualElement.style.display = DisplayStyle.None;

            BuildingManager.Instance.OnBuildingModeEnter += OnBuildingModeEnter_Delegate;
            BuildingManager.Instance.OnBuildingModeExit += OnBuildingModeExit_Delegate;

            OnTowersButtonClicked();

            InvokeRepeating(nameof(UpdatePriceColors), 0f, 0.1f);
        }

        void OnDisable()
        {
            BuildingManager buildingManager = BuildingManager.Instance;
            if (buildingManager == null) return;
            buildingManager.OnBuildingModeEnter -= OnBuildingModeEnter_Delegate;
            buildingManager.OnBuildingModeExit -= OnBuildingModeExit_Delegate;
        }

        void OnBuildingModeExit_Delegate() => _doc.rootVisualElement.style.display = DisplayStyle.None;

        void OnBuildingModeEnter_Delegate() => _doc.rootVisualElement.style.display = DisplayStyle.Flex;


        private void InitializeItemsPanel()
        {

            _ItemsBuyButtons = new Button[4];
            _ItemsNamesLabels = new Label[4, 1];
            _ItemsPriceLabels = new Label[4, 2];
            _ItemsDescriptionsLabels = new Label[4, 1];
            _ItemsSprites = new VisualElement[4, 1];
            _ItemsRows = new VisualElement[4];

            for (int i = 0; i < 4; i++)
            {
                _ItemsBuyButtons[i] = _ItemsContener.Q<Button>($"Item{i + 1}_Button");

                _ItemsNamesLabels[i, 0] = _ItemsContener.Q<Label>($"Item{i + 1}_Name");

                _ItemsPriceLabels[i, 0] = _ItemsContener.Q<Label>($"Item{i + 1}_Wood_Price");
                _ItemsPriceLabels[i, 1] = _ItemsContener.Q<Label>($"Item{i + 1}_Stone_Price");

                _ItemsDescriptionsLabels[i, 0] = _ItemsContener.Q<Label>($"Item{i + 1}_Description");

                _ItemsSprites[i, 0] = _ItemsContener.Q<VisualElement>($"Item{i + 1}_Icon");

                _ItemsRows[i] = _ItemsContener.Q<VisualElement>($"Item{i + 1}");

                int index = i; 
                _ItemsBuyButtons[i].clicked += () => TryBuyItem(index);
            }

            ItemsPanel_Next = _ItemsContener.Q<Button>("NextButton");
            ItemsPanel_Next.clicked += OnNextButtonClicked;

            ItemsPanel_Back = _ItemsContener.Q<Button>("Back_Button");
            ItemsPanel_Back.clicked += OnBackButtonClicked;

            ItemsPanel_PageNumber = _ItemsContener.Q<Label>("Page_Number");
        }


        void OnTowersButtonClicked()
        {
            VisualElement towerBuildingPanel = _doc.rootVisualElement.Q<VisualElement>("Contener");

            towerBuildingPanel.Clear();

            _TowersContener.style.flexGrow = 1;
            towerBuildingPanel.Add(_TowersContener);
        }

        void OnItemsButtonClicked()
        {
            VisualElement towerBuildingPanel = _doc.rootVisualElement.Q<VisualElement>("Contener");

            towerBuildingPanel.Clear();

            _ItemsContener.style.flexGrow = 1;
            towerBuildingPanel.Add(_ItemsContener);

            AcctualPage = 0;
            ItemsPanel_PageNumber.text = (AcctualPage + 1).ToString();

            DisplayItemsOnPage();
        }

        void OnNextButtonClicked()
        {
            int maxPage = Mathf.CeilToInt((float)SizeOfItems / 4) - 1;

            if (AcctualPage < maxPage)
            {
                AcctualPage++;
                ItemsPanel_PageNumber.text = (AcctualPage + 1).ToString();
                DisplayItemsOnPage();
            }
        }

        void OnBackButtonClicked()
        {
            if (AcctualPage > 0)
            {
                AcctualPage--;
                ItemsPanel_PageNumber.text = (AcctualPage + 1).ToString();
                DisplayItemsOnPage();
            }
        }

        private void DisplayItemsOnPage()
        {
            int itemsPerPage = 4;
            int startIndex = AcctualPage * itemsPerPage;

            for (int i = 0; i < itemsPerPage; i++)
            {
                int itemIndex = startIndex + i;

                if (itemIndex < _ItemsDatabase.Items.Count)
                {
                    var currentItem = _ItemsDatabase.Items[itemIndex].Item;

                    _ItemsNamesLabels[i, 0].text = currentItem.Name;
                    _ItemsPriceLabels[i, 0].text = "30";
                    _ItemsPriceLabels[i, 1].text = "30";
                    _ItemsDescriptionsLabels[i, 0].text = currentItem.Description;

                    _ItemsSprites[i, 0].style.backgroundImage = new StyleBackground(icon);


                    _ItemsRows[i].style.display = DisplayStyle.Flex;
                }
                else
                {
                    _ItemsNamesLabels[i, 0].text = "";
                    _ItemsPriceLabels[i, 0].text = "";
                    _ItemsPriceLabels[i, 1].text = "";
                    _ItemsDescriptionsLabels[i, 0].text = "";

                    _ItemsRows[i].style.display = DisplayStyle.None;
                }
            }
        }

        void TryBuyBuilding(TowerBundle tower, int woodCost, int stoneCost)
        {

            PlayerResourceInventory inventory = PlayerResourceInventory.Instance;

            if (inventory.HasEnoughOfResource(tower.cost[1].resource, stoneCost) && inventory.HasEnoughOfResource(tower.cost[0].resource, woodCost))
            {
                inventory.RemoveResource(tower.cost[1].resource, stoneCost);
                inventory.RemoveResource(tower.cost[0].resource, woodCost);

                OnBuildingModeExit_Delegate();

                OnBuildingSelected?.Invoke(tower);

            }
            else
            {
                Debug.Log("Nie masz wystarczaj¹cych zasobów!");
            }
        }

        //testowanie kupowania itemow
        void TryBuyItem(int buttonIndex)
        {
            int itemIndex = AcctualPage * 4 + buttonIndex;

            if (itemIndex >= _ItemsDatabase.Items.Count) return;

            var selectedItem = _ItemsDatabase.Items[itemIndex].Item;
            PlayerResourceInventory inventory = PlayerResourceInventory.Instance;

            if (inventory.HasEnoughOfResource(res_stone, 30) && inventory.HasEnoughOfResource(res_wood, 30))
            {
                //ten warunek mozna zmienic pozniej
                if (!ItemManager.Instance.EquippedItems.Contains(selectedItem))
                {
                    inventory.RemoveResource(res_stone, 30);
                    inventory.RemoveResource(res_wood, 30);
                    ItemManager.Instance.AddItemByID(selectedItem.Id);
                }
            }
            else
            {
                Debug.Log("Nie masz wystarczaj¹cych zasobów!");
            }
        }

        void SetTowerPrices()
        {
            for (int i = 0; i < towerBundles.Count; i++)
            {
                _towerPriceLabels[i, 0].text = towerBundles[i].cost[0].amount.ToString();
                _towerPriceLabels[i, 1].text = towerBundles[i].cost[1].amount.ToString();
            }
        }

        void UpdatePriceColors()
        {
            PlayerResourceInventory inventory = PlayerResourceInventory.Instance;

            for (int i = 0; i < towerBundles.Count; i++)
            {
                UpdateLabelColor(towerBundles[i].cost[0], _towerPriceLabels[i, 0], inventory, towerBundles[i].cost[0].resource);
                UpdateLabelColor(towerBundles[i].cost[1], _towerPriceLabels[i, 1], inventory, towerBundles[i].cost[1].resource);
            }

            //testowanie kupowania itemow
            for (int i = 0; i < 4; i++)
            {
                UpdateLabelColor2(30, _ItemsPriceLabels[i, 0], inventory, res_wood);
                UpdateLabelColor2(30, _ItemsPriceLabels[i, 1], inventory, res_stone);
            }
        }
        void UpdateLabelColor(ResourceCost cost, Label label, PlayerResourceInventory inventory, ResourceSO resource)
        {
            label.style.color = inventory.HasEnoughOfResource(resource, cost.amount) ? Color.white : Color.red;
        }

        //testowanie kupowania itemow
        void UpdateLabelColor2(int amount, Label label, PlayerResourceInventory inventory, ResourceSO resource)
        {
            label.style.color = inventory.HasEnoughOfResource(resource, amount) ? Color.white : Color.red;
        }
    }
}