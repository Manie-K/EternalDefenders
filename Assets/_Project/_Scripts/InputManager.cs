using MG_Utilities;
using System;
using Unity.Plastic.Newtonsoft.Json.Bson;
using UnityEngine;

namespace EternalDefenders
{
    public class InputManager : Singleton<InputManager>
    {
        public enum GameMode
        {
            Menu,
            Building,
            Store,
            Inventory,
            Playing
        }

        public event Action OnStoreModeEnter;
        public event Action OnStoreModeExit;
        
        public event Action OnBuildModeEnter;
        public event Action OnBuildModeExit;
        public event Action OnBuildingPlace;

        public event Action OnPlayModeEnter;
        public event Action OnPlayModeExit;

        public event Action OnPlayerWeaponChange;

        private GameMode _currentGameMode;

        public bool IsPlayerSprinting { get; private set; }
        public bool IsPlayerJumping { get; private set; }
        public bool IsPlayerFighting { get; private set; }
        public bool IsPlayerMoving { get; private set; }


        protected override void Awake()
        {
            base.Awake();
            _currentGameMode = GameMode.Playing;
        }

        void Start()
        {
            _currentGameMode = GameMode.Playing;
            Tower_Building_Panel_Controller.Instance.OnBuildingSelected += OnBuildingSelected_Delegate;
            BuildingManager.Instance.OnBuildFinished += OnBuildFinished_Delegate;  
        }

        void Update()
        {
            switch (_currentGameMode)
            {
                case GameMode.Building:
                    HandleBuildingModeInput();
                    break;
                case GameMode.Playing:
                    HandlePlayModeInput();
                    break;
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                if (_currentGameMode == GameMode.Store || _currentGameMode == GameMode.Building)
                {
                    _currentGameMode = GameMode.Playing;
                    OnBuildModeExit?.Invoke();
                    OnPlayModeEnter?.Invoke();
                }
                else
                {
                    _currentGameMode = GameMode.Store;
                    OnPlayModeExit?.Invoke();
                    OnStoreModeEnter?.Invoke();
                }
            }

        }

        void HandleBuildingModeInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnBuildingPlace?.Invoke();
            }
        }

        void HandlePlayModeInput()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            if (horizontal != 0 || vertical != 0)
            {
                IsPlayerMoving = true;
            }
            else
            {
                IsPlayerMoving = false;
            }

            IsPlayerSprinting = Input.GetKey(KeyCode.LeftShift);
            IsPlayerJumping = Input.GetKeyDown(KeyCode.Space);
            IsPlayerFighting = Input.GetMouseButton(0);

            if (Input.GetKeyDown(KeyCode.K))
            {
                PlayerController player = FindAnyObjectByType<PlayerController>();
                WeaponsController weaponsController = player.GetComponent<WeaponsController>();

                weaponsController.SetNextArsenal();
                OnPlayerWeaponChange?.Invoke();
            }
        }
        
        void OnBuildingSelected_Delegate(TowerBundle towerBundle)
        {
            OnStoreModeExit?.Invoke();
            OnBuildModeEnter?.Invoke();
            _currentGameMode = GameMode.Building;
        }

        void OnBuildFinished_Delegate()
        {
            _currentGameMode = GameMode.Playing;
            OnBuildModeExit?.Invoke();
        }

    }
}
