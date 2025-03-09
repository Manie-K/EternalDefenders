using System;
using System.Collections.Generic;
using MG_Utilities;
using UnityEngine;
using UnityEngine.UIElements;

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


        [SerializeField] private int _playerWood = 100;
        [SerializeField] private int _playerStone = 100;

        [SerializeField] private List<TowerBundle> towerBundles;
        public event Action<TowerBundle> OnBuildingSelected;

        void Start()
        {
            _doc = GetComponent<UIDocument>();

            _FireTowerBuy = _doc.rootVisualElement.Q<Button>("Fire_Tower_Button");
            _FireWoodPrice = _doc.rootVisualElement.Q<Label>("Fire_Wood_Price");
            _FireStonePrice = _doc.rootVisualElement.Q<Label>("Fire_Stone_Price");

            _FireTowerBuy.clicked += () => FireTowerBuyButtonOnClicked();

            _ElectricTowerBuy = _doc.rootVisualElement.Q<Button>("Electric_Tower_Button");
            _ElectricTowerBuy.clicked += () => ElectricTowerBuyButtonOnClicked();
            _ElectricWoodPrice = _doc.rootVisualElement.Q<Label>("Electric_Wood_Price");
            _ElectricStonePrice = _doc.rootVisualElement.Q<Label>("Electric_Stone_Price");


            _IceTowerBuy = _doc.rootVisualElement.Q<Button>("Ice_Tower_Button");
            _IceTowerBuy.clicked += () => IceTowerBuyButtonOnClicked();

            _IceWoodPrice = _doc.rootVisualElement.Q<Label>("Ice_Wood_Price");
            _IceStonePrice = _doc.rootVisualElement.Q<Label>("Ice_Stone_Price");


            _WoodMillBuy = _doc.rootVisualElement.Q<Button>("WoodMill_Button");
            _WoodMillBuy.clicked += () => WoodMillBuyButtonOnClicked();

            _WoodMillWoodPrice = _doc.rootVisualElement.Q<Label>("WoodMill_Wood_Price");
            _WoodMillStonePrice = _doc.rootVisualElement.Q<Label>("WoodMill_Stone_Price");


            _StoneMillBuy = _doc.rootVisualElement.Q<Button>("StoneMill_Button");
            _StoneMillBuy.clicked += () => StoneMillBuyButtonOnClicked();

            _StoneMillWoodPrice = _doc.rootVisualElement.Q<Label>("StoneMill_Wood_Price");
            _StoneMillStonePrice = _doc.rootVisualElement.Q<Label>("StoneMill_Stone_Price");
            _doc.rootVisualElement.style.display = DisplayStyle.None;

            BuildingManager.Instance.OnBuildingModeEnter += OnBuildingModeEnter_Delegate;
            BuildingManager.Instance.OnBuildingModeExit += OnBuildingModeExit_Delegate;

            InvokeRepeating(nameof(UpdatePriceColors), 0f, 0.1f); 
        }

        void OnDisable()
        {
            BuildingManager buildingManager = BuildingManager.Instance;
            if (buildingManager == null) return;
            BuildingManager.Instance.OnBuildingModeEnter -= OnBuildingModeEnter_Delegate;
            BuildingManager.Instance.OnBuildingModeExit -= OnBuildingModeExit_Delegate;
        }

        void OnBuildingModeExit_Delegate()
        {
            _doc.rootVisualElement.style.display = DisplayStyle.None;
        }

        void OnBuildingModeEnter_Delegate()
        {
            _doc.rootVisualElement.style.display = DisplayStyle.Flex;
        }

        void FireTowerBuyButtonOnClicked()
        {
            int woodCost = int.Parse(_FireWoodPrice.text);
            int stoneCost = int.Parse(_FireStonePrice.text);

            if (_playerWood >= woodCost && _playerStone >= stoneCost)
            {
                _playerWood -= woodCost;
                _playerStone -= stoneCost;
                OnBuildingModeExit_Delegate();
                OnBuildingSelected?.Invoke(towerBundles[0]);
            }
            else
            {
                Debug.Log("Nie masz wystarczaj¹cych zasobów!");
            }
        }

        void ElectricTowerBuyButtonOnClicked()
        {

            int woodCost = int.Parse(_ElectricWoodPrice.text);
            int stoneCost = int.Parse(_ElectricStonePrice.text);

            if (_playerWood >= woodCost && _playerStone >= stoneCost)
            {
                _playerWood -= woodCost;
                _playerStone -= stoneCost;
                OnBuildingModeExit_Delegate();
                OnBuildingSelected?.Invoke(towerBundles[1]);
            }
            else
            {
                Debug.Log("Nie masz wystarczaj¹cych zasobów!");
            }
        }

        void IceTowerBuyButtonOnClicked()
        {
            int woodCost = int.Parse(_IceWoodPrice.text);
            int stoneCost = int.Parse(_IceStonePrice.text);

            if (_playerWood >= woodCost && _playerStone >= stoneCost)
            {
                _playerWood -= woodCost;
                _playerStone -= stoneCost;
                OnBuildingModeExit_Delegate();
                OnBuildingSelected?.Invoke(towerBundles[2]);
            }
            else
            {
                Debug.Log("Nie masz wystarczaj¹cych zasobów!");
            }
        }

        void StoneMillBuyButtonOnClicked()
        {
            int woodCost = int.Parse(_StoneMillWoodPrice.text);
            int stoneCost = int.Parse(_StoneMillStonePrice.text);

            if (_playerWood >= woodCost && _playerStone >= stoneCost)
            {
                _playerWood -= woodCost;
                _playerStone -= stoneCost;
                OnBuildingModeExit_Delegate();
                Debug.Log("StoneMill Bought");
            }
            else
            {
                Debug.Log("Nie masz wystarczaj¹cych zasobów!");
            }
        }

        void WoodMillBuyButtonOnClicked()
        {
            int woodCost = int.Parse(_WoodMillWoodPrice.text);
            int stoneCost = int.Parse(_WoodMillStonePrice.text);

            if (_playerWood >= woodCost && _playerStone >= stoneCost)
            {
                _playerWood -= woodCost;
                _playerStone -= stoneCost;
                OnBuildingModeExit_Delegate();
                Debug.Log("WoodMill Bought");
            }
            else
            {
                Debug.Log("Nie masz wystarczaj¹cych zasobów!");
            }
        }

        void UpdatePriceColors()
        {
            int woodCost = int.Parse(_FireWoodPrice.text);
            int stoneCost = int.Parse(_FireStonePrice.text);

            _FireWoodPrice.style.color = _playerWood >= woodCost ? Color.white : Color.red;
            _FireStonePrice.style.color = _playerStone >= stoneCost ? Color.white : Color.red;

            woodCost = int.Parse(_ElectricWoodPrice.text);
            stoneCost = int.Parse(_ElectricStonePrice.text);

            _ElectricWoodPrice.style.color = _playerWood >= woodCost ? Color.white : Color.red;
            _ElectricStonePrice.style.color = _playerStone >= stoneCost ? Color.white : Color.red;

            woodCost = int.Parse(_IceWoodPrice.text);
            stoneCost = int.Parse(_IceStonePrice.text);

            _IceWoodPrice.style.color = _playerWood >= woodCost ? Color.white : Color.red;
            _IceStonePrice.style.color = _playerStone >= stoneCost ? Color.white : Color.red;

            woodCost = int.Parse(_StoneMillWoodPrice.text);
            stoneCost = int.Parse(_StoneMillStonePrice.text);

            _StoneMillWoodPrice.style.color = _playerWood >= woodCost ? Color.white : Color.red;
            _StoneMillStonePrice.style.color = _playerStone >= stoneCost ? Color.white : Color.red;

            woodCost = int.Parse(_WoodMillWoodPrice.text);
            stoneCost = int.Parse(_WoodMillStonePrice.text);

            _WoodMillWoodPrice.style.color = _playerWood >= woodCost ? Color.white : Color.red;
            _WoodMillStonePrice.style.color = _playerStone >= stoneCost ? Color.white : Color.red;
        }
    }
}