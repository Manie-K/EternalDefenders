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

        private Button _FireTowerBuy;
        private Label _FireWoodPrice;
        private Label _FireStonePrice;

        private Button _ElectricTowerBuy;
        private Label _ElectricWoodPrice;
        private Label _ElectricStonePrice;

        private Button _IceTowerBuy;
        private Label _IceWoodPrice;
        private Label _IceStonePrice;

        private Button _WoodMillBuy;
        private Label _WoodMillWoodPrice;
        private Label _WoodMillStonePrice;

        private Button _StoneMillBuy;
        private Label _StoneMillWoodPrice;
        private Label _StoneMillStonePrice;

        [SerializeField] private List<TowerBundle> towerBundles;
        public event Action<TowerBundle> OnBuildingSelected;

        void Start()
        {
            _doc = GetComponent<UIDocument>();

            _FireTowerBuy = _doc.rootVisualElement.Q<Button>("Fire_Tower_Button");
            _FireWoodPrice = _doc.rootVisualElement.Q<Label>("Fire_Wood_Price");
            _FireStonePrice = _doc.rootVisualElement.Q<Label>("Fire_Stone_Price");
            _FireTowerBuy.clicked += () => TryBuyBuilding(towerBundles[0], towerBundles[0].cost[0].amount, towerBundles[0].cost[1].amount);

            _ElectricTowerBuy = _doc.rootVisualElement.Q<Button>("Electric_Tower_Button");
            _ElectricWoodPrice = _doc.rootVisualElement.Q<Label>("Electric_Wood_Price");
            _ElectricStonePrice = _doc.rootVisualElement.Q<Label>("Electric_Stone_Price");
            _ElectricTowerBuy.clicked += () => TryBuyBuilding(towerBundles[1], towerBundles[1].cost[0].amount, towerBundles[1].cost[1].amount);

            _IceTowerBuy = _doc.rootVisualElement.Q<Button>("Ice_Tower_Button");
            _IceWoodPrice = _doc.rootVisualElement.Q<Label>("Ice_Wood_Price");
            _IceStonePrice = _doc.rootVisualElement.Q<Label>("Ice_Stone_Price");
            _IceTowerBuy.clicked += () => TryBuyBuilding(towerBundles[2], towerBundles[2].cost[0].amount, towerBundles[2].cost[1].amount);

            _WoodMillBuy = _doc.rootVisualElement.Q<Button>("WoodMill_Button");
            _WoodMillWoodPrice = _doc.rootVisualElement.Q<Label>("WoodMill_Wood_Price");
            _WoodMillStonePrice = _doc.rootVisualElement.Q<Label>("WoodMill_Stone_Price");
            _WoodMillBuy.clicked += () => TryBuyBuilding(towerBundles[3], towerBundles[3].cost[0].amount, towerBundles[3].cost[1].amount);

            _StoneMillBuy = _doc.rootVisualElement.Q<Button>("StoneMill_Button");
            _StoneMillWoodPrice = _doc.rootVisualElement.Q<Label>("StoneMill_Wood_Price");
            _StoneMillStonePrice = _doc.rootVisualElement.Q<Label>("StoneMill_Stone_Price");
            _StoneMillBuy.clicked += () => TryBuyBuilding(towerBundles[4], towerBundles[4].cost[0].amount, towerBundles[4].cost[1].amount);

            _doc.rootVisualElement.style.display = DisplayStyle.None;

            BuildingManager.Instance.OnBuildingModeEnter += OnBuildingModeEnter_Delegate;
            BuildingManager.Instance.OnBuildingModeExit += OnBuildingModeExit_Delegate;

            SetTowerPrices();

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
        void SetTowerPrices()
        {
            if (towerBundles.Count > 0)
            {
                var fireTower = towerBundles[0];
                _FireWoodPrice.text = fireTower.cost[0].amount.ToString();
                _FireStonePrice.text = fireTower.cost[1].amount.ToString();
            }

            if (towerBundles.Count > 1)
            {
                var electricTower = towerBundles[1];
                _ElectricWoodPrice.text = electricTower.cost[0].amount.ToString();
                _ElectricStonePrice.text = electricTower.cost[1].amount.ToString();
            }

            if (towerBundles.Count > 2)
            {
                var iceTower = towerBundles[2];
                _IceWoodPrice.text = iceTower.cost[0].amount.ToString();
                _IceStonePrice.text = iceTower.cost[1].amount.ToString();
            }

            if (towerBundles.Count > 3)
            {
                var woodMill = towerBundles[3];
                _WoodMillWoodPrice.text = woodMill.cost[0].amount.ToString();
                _WoodMillStonePrice.text = woodMill.cost[1].amount.ToString();
            }

            if (towerBundles.Count > 4)
            {
                var stoneMill = towerBundles[4];
                _StoneMillWoodPrice.text = stoneMill.cost[0].amount.ToString();
                _StoneMillStonePrice.text = stoneMill.cost[1].amount.ToString();
            }
        }

        void UpdatePriceColors()
        {
            PlayerResourceInventory inventory = PlayerResourceInventory.Instance;

            UpdateLabelColor(towerBundles[0].cost[0], _FireWoodPrice, inventory, towerBundles[0].cost[0].resource);
            UpdateLabelColor(towerBundles[0].cost[1], _FireStonePrice, inventory, towerBundles[0].cost[1].resource);

            UpdateLabelColor(towerBundles[1].cost[0], _ElectricWoodPrice, inventory, towerBundles[1].cost[0].resource);
            UpdateLabelColor(towerBundles[1].cost[1], _ElectricStonePrice, inventory, towerBundles[1].cost[1].resource);

            UpdateLabelColor(towerBundles[2].cost[0], _IceWoodPrice, inventory, towerBundles[2].cost[0].resource);
            UpdateLabelColor(towerBundles[2].cost[1], _IceStonePrice, inventory, towerBundles[2].cost[1].resource);
            /*
            UpdateLabelColor(towerBundles[3].cost[0], _StoneMillWoodPrice, inventory, towerBundles[3].cost[0].resource);
            UpdateLabelColor(towerBundles[3].cost[1],_StoneMillStonePrice, inventory, towerBundles[3].cost[1].resource);

            UpdateLabelColor(towerBundles[4].cost[0], _WoodMillWoodPrice, inventory, towerBundles[4].cost[0].resource);
            UpdateLabelColor(towerBundles[4].cost[1], _WoodMillStonePrice, inventory, towerBundles[4].cost[1].resource);
            */
        }

        void UpdateLabelColor(ResourceCost cost, Label label, PlayerResourceInventory inventory, ResourceSO resource)
        {
            label.style.color = inventory.HasEnoughOfResource(resource, cost.amount) ? Color.white : Color.red;
        }
    }
}