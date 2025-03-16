using EternalDefenders;
using HudElements;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;

namespace EternalDefenders
{
    public class HudManager : MonoBehaviour
    {
        public VisualTreeAsset counterUXML;

        private VisualElement woodCounterContainer;
        private VisualElement rockCounterContainer;
        private VisualElement waveTimerContainer;

        private Label counterWoodLabel;
        private Label counterStoneLabel;
        private Label counterWaveLabel;

        public Sprite woodIcon;
        public Sprite stoneIcon;

        private HealthBar BaseHeartBar;
        private HealthBar _healthBar;
        private HealthBar _shieldBar;

        private void Start()
        {
            var hudUIDocument = GetComponent<UIDocument>();
            var hudRootElement = hudUIDocument.rootVisualElement;

            BaseHeartBar = hudRootElement.Q<HealthBar>("BaseHeartBar");
            _healthBar = hudRootElement.Q<HealthBar>("HealthBar");
            _shieldBar = hudRootElement.Q<HealthBar>("ShieldBar");


            woodCounterContainer = hudRootElement.Q<VisualElement>("WoodCounter");
            rockCounterContainer = hudRootElement.Q<VisualElement>("StoneCounter");
            waveTimerContainer = hudRootElement.Q<VisualElement>("WaveTimer");

            PlayerResourceInventory.Instance.OnInventoryChanged += UpdateCounterLabels;

            if (woodCounterContainer != null && rockCounterContainer != null && waveTimerContainer != null)
            {
                LoadCounterUI(woodCounterContainer);
                LoadCounterUI(rockCounterContainer);
                LoadCounterUI(waveTimerContainer);
            }

            counterWoodLabel = woodCounterContainer.Q<Label>("counter");
            counterStoneLabel = rockCounterContainer.Q<Label>("counter");
            counterWaveLabel = waveTimerContainer.Q<Label>("counter");

            counterWaveLabel.text = "0";
            AddImageToContainer(woodCounterContainer, woodIcon);
            AddImageToContainer(rockCounterContainer, stoneIcon);


            InvokeRepeating(nameof(UpdateHealthBars), 0f, 0.1f);
        }

        private void LoadCounterUI(VisualElement container)
        {
            var counterRoot = counterUXML.CloneTree();
            counterRoot.style.flexGrow = 1;
            counterRoot.style.width = Length.Percent(100);
            counterRoot.style.height = Length.Percent(100);
            container.Add(counterRoot);
        }

        private void AddImageToContainer(VisualElement container, Sprite sprite)
        {

            VisualElement imageContainer = container.Q<VisualElement>("Image");

            Image image = new Image();
            image.sprite = sprite;

            image.style.width = Length.Percent(100);
            image.style.height = Length.Percent(100);

            imageContainer.Clear();
            imageContainer.Add(image);
        }

        private void UpdateCounterLabels()
        {
            Dictionary<ResourceSO, int> resources = PlayerResourceInventory.Instance.GetAllResources();

            foreach (var resource in resources)
            {
                if (resource.Key.Name == "Wood")
                {
                    counterWoodLabel.text = $"{resource.Value}";
                }
                if (resource.Key.Name == "Stone")
                {
                    counterStoneLabel.text = $"{resource.Value}";
                }
            }
        }


        void UpdateHealthBars()
        {
            if (BaseHeartBar != null)
            {
                float currentHealth = MainBaseController.Instance.Stats.GetStat(StatType.Health);
                float maxHealth = MainBaseController.Instance.Stats.GetStat(StatType.MaxHealth);
                BaseHeartBar.value = currentHealth / maxHealth;
            }

            if (_healthBar != null)
            {
                int currentHealth = PlayerController.Instance.Stats.GetStat(StatType.Health);
                int baseHealth = PlayerController.Instance.Stats.GetStat(StatType.MaxHealth);
                _healthBar.value = (float)currentHealth / baseHealth;
            }
            if (_shieldBar != null)
            {
                int currentShield = PlayerController.Instance.Stats.GetStat(StatType.Shield);
                int baseShield = PlayerController.Instance.Stats.GetStat(StatType.MaxShield);
                _shieldBar.value = (float)currentShield / baseShield;
            }


        }

    }
}