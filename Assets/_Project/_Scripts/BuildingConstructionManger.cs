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
        
        public event Action<TowerBundle> OnBuildingSelected;
        
        void Start()
        {
            GenerateButtons();

            InputManager.Instance.OnStoreModeEnter += OnStoreModeEnter_Delegate;
            InputManager.Instance.OnStoreModeExit += OnStoreModeExit_Delegate;
        }

        void OnDisable()
        {
            //TODO: Clean up this mess
            InputManager inputManager = InputManager.Instance;
            if (inputManager == null) return;
            inputManager.OnStoreModeEnter -= OnStoreModeEnter_Delegate;
            inputManager.OnStoreModeExit -= OnStoreModeExit_Delegate;
        }
        void OnStoreModeEnter_Delegate()
        {
            panel.gameObject.SetActive(true);
        }
        void OnStoreModeExit_Delegate()
        {
            panel.gameObject.SetActive(false);
        }

        void GenerateButtons()
        {
            foreach (var towerBundle in towerBundles)
            {
                GameObject button = Instantiate(buttonPrefab, panel);
                button.GetComponent<Image>().sprite = towerBundle.icon;
                
                Button btn = button.GetComponent<Button>();
                btn.onClick.AddListener(() => OnBuildingSelected?.Invoke(towerBundle));
            }
        }
    }
}