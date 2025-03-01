using System;
using System.Collections.Generic;
using MG_Utilities;
using UnityEngine;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

namespace EternalDefenders
{
    public class BuildingConstructionManger : Singleton<BuildingConstructionManger>
    {
        [SerializeField] GameObject buttonPrefab;
        [SerializeField] RectTransform panel;
        [SerializeField] List<TowerBundle> towerBundles;
        
        public event Action<TowerController> OnBuildingSelected;
        
        void Start()
        {
            GenerateButtons();

            BuildingManager.Instance.OnBuildingModeEnter += OnBuildingModeEnter_Delegate;
            
            BuildingManager.Instance.OnBuildingModeExit += OnBuildingModeExit_Delegate;
        }

        void OnDisable()
        {
            BuildingManager.Instance.OnBuildingModeEnter -= OnBuildingModeEnter_Delegate;
            BuildingManager.Instance.OnBuildingModeExit -= OnBuildingModeExit_Delegate;
        }
        void OnBuildingModeEnter_Delegate()
        {
            panel.gameObject.SetActive(true);
        }
        void OnBuildingModeExit_Delegate()
        {
            panel.gameObject.SetActive(false);
        }

        void GenerateButtons()
        {
            foreach (var towerBundle in towerBundles)
            {
                GameObject button = Instantiate(buttonPrefab, panel);
                button.GetComponent<Image>().sprite = towerBundle.icon;
                button.GetComponent<Button>().onClick.AddListener(() =>
                {
                    OnBuildingSelected?.Invoke(towerBundle.towerPrefab);
                });
            }
        }
    }
}