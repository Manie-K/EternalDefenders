using EternalDefenders;
using HudElements;
using Mono.Cecil;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;

namespace EternalDefenders
{
    public class HudManager : MonoBehaviour
    {

        [SerializeField] private float waveTimeRemaining;

        public VisualTreeAsset counterUXML;

        private VisualElement woodCounterContainer;
        private VisualElement rockCounterContainer;
        private VisualElement aliveMobsCounterContainer;
        private VisualElement deadMobsCounterContainer;
        private VisualElement waveTimerContainer;

        private Label counterWoodLabel;
        private Label counterStoneLabel;
        private Label counterDeadMobsLabel;
        private Label counterAliveMobsLabel;
        private Label counterWaveLabel;

        public Sprite woodIcon;
        public Sprite stoneIcon;
        public Sprite deathIcon;
        public Sprite MobIcon;

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
            aliveMobsCounterContainer = hudRootElement.Q<VisualElement>("AliveMobs");
            deadMobsCounterContainer = hudRootElement.Q<VisualElement>("DeadMobs");
            waveTimerContainer = hudRootElement.Q<VisualElement>("WaveTimer");

            PlayerResourceInventory.Instance.OnInventoryChanged += UpdateCounterLabels;
            GameStatisticsManager.Instance.OnEnemyDead += UpdateDeadMobs;

            if (woodCounterContainer != null && rockCounterContainer != null && waveTimerContainer != null 
                && aliveMobsCounterContainer!=null && deadMobsCounterContainer!=null)
            {
                LoadCounterUI(woodCounterContainer);
                LoadCounterUI(rockCounterContainer);
                LoadCounterUI(aliveMobsCounterContainer);
                LoadCounterUI(deadMobsCounterContainer);
                LoadCounterUI(waveTimerContainer);
            }

            counterWoodLabel = woodCounterContainer.Q<Label>("counter");
            counterStoneLabel = rockCounterContainer.Q<Label>("counter");
            counterAliveMobsLabel = aliveMobsCounterContainer.Q<Label>("counter");
            counterDeadMobsLabel = deadMobsCounterContainer.Q<Label>("counter");
            counterWaveLabel = waveTimerContainer.Q<Label>("counter");

            counterWaveLabel.text = "0";
            counterWaveLabel.style.fontSize = new StyleLength(Length.Percent(55));
            counterAliveMobsLabel.text = "0";
            counterDeadMobsLabel.text = "0";

            AddImageToContainer(woodCounterContainer, woodIcon, 0);
            AddImageToContainer(rockCounterContainer, stoneIcon, 0);
            AddImageToContainer(aliveMobsCounterContainer, MobIcon, 1);
            AddImageToContainer(deadMobsCounterContainer, deathIcon, 1);

            InvokeRepeating(nameof(UpdateHealthBars), 0f, 0.1f);
            InvokeRepeating(nameof(UpdateAliveMobs), 0f, 0.1f);
        }

        private void Update()
        {
            UpdateWaveTimer();
        }

        private void LoadCounterUI(VisualElement container)
        {
            var counterRoot = counterUXML.CloneTree();
            counterRoot.style.flexGrow = 1;
            counterRoot.style.width = Length.Percent(100);
            counterRoot.style.height = Length.Percent(100);
            container.Add(counterRoot);
        }

        private void AddImageToContainer(VisualElement container, Sprite sprite, int type)
        {

            VisualElement imageContainer = container.Q<VisualElement>("Image");

            Image image = new Image();
            image.sprite = sprite;

            if (type == 0)
            {
                image.style.width = Length.Percent(150);
                image.style.height = Length.Percent(150);
            }
            else
            {
                image.style.width = Length.Percent(80);
                image.style.height = Length.Percent(80);
            }

            imageContainer.Clear();
            imageContainer.Add(image);
        }

        private void UpdateAliveMobs()
        {
            counterAliveMobsLabel.text = $"{SpawnManager.Instance.GetEnemiersParent().childCount}";
        }

        private void UpdateDeadMobs()
        {
            counterDeadMobsLabel.text = $"{GameStatisticsManager.Instance.EnemiesKilled}";
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

        private void UpdateWaveTimer()
        {

            int minutes = Mathf.FloorToInt(waveTimeRemaining / 60f);
            int seconds = Mathf.FloorToInt(waveTimeRemaining % 60f);

            counterWaveLabel.text = $"{minutes:00}:{seconds:00}";

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