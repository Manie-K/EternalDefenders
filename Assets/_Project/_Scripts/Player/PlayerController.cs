using System;
using MG_Utilities;
using System.Collections;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using HudElements;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using Codice.Client.BaseCommands;

namespace EternalDefenders
{
    public class PlayerController : Singleton<PlayerController>, IEnemyTarget
    {
        [SerializeField] Transform cameraTransform;
        [SerializeField] float speed = 6f;
        [SerializeField] float turnSmoothTime = 0.01f;

        public UIDocument hud;
        private HealthBar _healthBar;
        private HealthBar _shieldBar;

        public Stats Stats { get; private set; } //sorki ale interface musi byc zaimplementowany 
        public event Action OnPlayerDeath;
        
        CharacterController _controller;
        Transform _playerTransform;
        Animator _animator;

        float _turnSmoothVelocity;
        int _currentAnimationHash = 0;
        readonly int _idleHash = Animator.StringToHash("Idle");
        readonly int _runningHash = Animator.StringToHash("Running");

        protected override void Awake()
        {
            base.Awake();
            _controller = GetComponent<CharacterController>();
            _playerTransform = transform.GetChild(0).GetComponent<Transform>();
            _animator = transform.GetChild(0).GetComponent<Animator>();
        }

        void Start()
        {
            ChangeAnimation(_idleHash);

            var root = hud.rootVisualElement;
            _healthBar = root.Q<HealthBar>("HealthBar");
            _shieldBar = root.Q<HealthBar>("ShieldBar");


            var initialStats = new Dictionary<StatType, Stats.Stat>
            {
                { StatType.Health, new Stats.Stat(100) }, 
                { StatType.MaxHealth, new Stats.Stat(100) }, 
                { StatType.Shield, new Stats.Stat(50) },   
                { StatType.MaxShield, new Stats.Stat(50) } 
            };

            // Tworzymy obiekt Stats na podstawie powy�szego s�ownika
            Stats = new Stats(initialStats);

            if (_healthBar != null && Stats.HasStat(StatType.Health))
            {
                int currentHealth = Stats.GetStat(StatType.Health);
                int baseHealth = Stats.GetStat(StatType.MaxHealth);
                _healthBar.value = (float) currentHealth / baseHealth;
            }
            if (_shieldBar != null && Stats.HasStat(StatType.Shield))
            {
                int currentShield = Stats.GetStat(StatType.Shield);
                int baseShield = Stats.GetStat(StatType.MaxShield);
                _shieldBar.value = (float) currentShield / baseShield;
            }
        }

        void Update()
        {
            ChangeDirection();
            MovePlayer();

            if(Stats.GetStat(StatType.Health) <= 0) OnPlayerDeath?.Invoke();
            if (_healthBar != null) _healthBar.value = (float) Stats.GetStat(StatType.Health) / Stats.GetStat(StatType.MaxHealth);           
            if (_shieldBar != null) _shieldBar.value = (float) Stats.GetStat(StatType.Shield) / Stats.GetStat(StatType.MaxShield);
        }

        void ChangeDirection()
        {
            //TODO possible 3d terrain issiues
            Vector3 mouseWorldPosition = CameraController.Instance.GetWorldMousePosition();
            Vector3 lookDirection = (mouseWorldPosition - _playerTransform.position).normalized;
            
            if (lookDirection.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(lookDirection.x, lookDirection.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(_playerTransform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, turnSmoothTime);
                _playerTransform.rotation = Quaternion.Euler(0f, angle, 0f);
                cameraTransform.rotation = Quaternion.Euler(0f, angle, 0f);
            }
        }

        void MovePlayer()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 forward = _playerTransform.forward;
            Vector3 right = _playerTransform.right;
            Vector3 movementDirection = (forward * vertical + right * horizontal).normalized;

            if (movementDirection.magnitude >= 0.1f)
            {
                ChangeAnimation(_runningHash);
                _controller.Move(movementDirection * (speed * Time.deltaTime));
            }
            else
            {
                ChangeAnimation(_idleHash);
            }
        }

        void ChangeAnimation(int animationHash, float crossFadeDuration = 0.2f, float time = 0) 
        {
            if (time > 0)
            {
                StartCoroutine(Wait());
            }
            else Validate();

            IEnumerator Wait()
            {
                yield return new WaitForSeconds(time - crossFadeDuration);
                Validate();
            }

            void Validate()
            {
                if(_currentAnimationHash != animationHash)
                {
                    //Debug.Log($"Changing animation from {_currentAnimationHash} to {animationHash}");
                    _currentAnimationHash = animationHash;
                    _animator.CrossFade(animationHash, crossFadeDuration);
                }
            }
        }

    }
}
