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
        private Button _ElectricTowerBuy;
        private Button _IceTowerBuy;

        [SerializeField] List<TowerBundle> towerBundles;
        public event Action<TowerBundle> OnBuildingSelected;


        void Start()
        {
            _doc = GetComponent<UIDocument>();

            _FireTowerBuy = _doc.rootVisualElement.Q<Button>("Fire_Tower_Button");
            _FireTowerBuy.clicked += () => FireTowerBuyButtonOnClicked();

            _ElectricTowerBuy = _doc.rootVisualElement.Q<Button>("Electric_Tower_Button");
            _ElectricTowerBuy.clicked += () => ElectricTowerBuyButtonOnClicked();

            _IceTowerBuy = _doc.rootVisualElement.Q<Button>("Ice_Tower_Button");
            _IceTowerBuy.clicked += () => IceTowerBuyButtonOnClicked();

            _doc.rootVisualElement.style.display = DisplayStyle.None;

            BuildingManager.Instance.OnBuildingModeEnter += OnBuildingModeEnter_Delegate;
            BuildingManager.Instance.OnBuildingModeExit += OnBuildingModeExit_Delegate;

        }
        void OnDisable()
        {
            //TODO: Clean up this mess
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
            OnBuildingModeExit_Delegate();
            OnBuildingSelected?.Invoke(towerBundles[0]);
        }

        void ElectricTowerBuyButtonOnClicked()
        {
            OnBuildingModeExit_Delegate();
            OnBuildingSelected?.Invoke(towerBundles[1]);
        }

        void IceTowerBuyButtonOnClicked()
        {
            OnBuildingModeExit_Delegate();
            OnBuildingSelected?.Invoke(towerBundles[2]);
        }
    }
}