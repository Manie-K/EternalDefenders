using System;
using MG_Utilities;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using HudElements;
using System.Collections.Generic;

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

        public Stats Stats { get; private set; }
        public event Action OnPlayerDeath;
        
        CharacterController _controller;
        Transform _playerTransform;
        Animator _animator;

        float _turnSmoothVelocity;
        int _currentAnimationHash = 0;
        readonly int _idleHash = Animator.StringToHash("Idle");
        readonly int _runningHash = Animator.StringToHash("Run");
        readonly int _runningRifleHash = Animator.StringToHash("Run Rifle");
        readonly int _idleRifleHash = Animator.StringToHash("Idle Rifle");
        readonly int _aimingSniperRifleHash = Animator.StringToHash("Aiming SniperRifle");
        readonly int _fireSniperRifleHash = Animator.StringToHash("Fire SniperRifle");

        private bool isFighting = false;
        private Vector3 velocity;
        private bool isGrounded;
        public float gravity = -9.81f;

        protected override void Awake()
        {
            base.Awake();
            _controller = GetComponent<CharacterController>();
            _playerTransform = transform.GetChild(0).GetComponent<Transform>();
            _animator = transform.GetChild(0).GetComponent<Animator>();
        }

        void Start()
        {
            ChangeAnimation(_idleRifleHash);

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
            PlayerInput();

            if(Stats.GetStat(StatType.Health) <= 0) OnPlayerDeath?.Invoke();
            if (_healthBar != null) _healthBar.value = (float) Stats.GetStat(StatType.Health) / Stats.GetStat(StatType.MaxHealth);           
            if (_shieldBar != null) _shieldBar.value = (float) Stats.GetStat(StatType.Shield) / Stats.GetStat(StatType.MaxShield);

            //Gravity
            isGrounded = _controller.isGrounded;

            if (isGrounded && velocity.y <0)
            {
                velocity.y = -2f;
            }

            velocity.y += gravity * Time.deltaTime;
            _controller.Move(velocity * Time.deltaTime);
        }

        void PlayerInput()
        {
            if (Input.GetMouseButton(1))
            {
                isFighting = true;
                ChangeAnimation(_aimingSniperRifleHash, 0.02f);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                ChangeAnimation(_fireSniperRifleHash, 0.03f);   
            }
            else if (Input.GetMouseButtonUp(0))
            {
                ChangeAnimation(_aimingSniperRifleHash, 0.5f);
            }
            else if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                isFighting = false;
                MovePlayer();
            }
            else if (!isFighting)
            {
                ChangeAnimation(_idleRifleHash);
            }

        }

        void ChangeDirection(Vector3 movementDirection)
        {         
            if (movementDirection.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg;
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
            Vector3 movementDirection = new Vector3(horizontal, 0f, vertical).normalized;

            if (movementDirection.magnitude >= 0.1f)
            {
                ChangeDirection(movementDirection);
                ChangeAnimation(_runningRifleHash);
                _controller.Move(movementDirection * (speed * Time.deltaTime));
            }
        }

        void ChangeAnimation(int animationHash, float crossFadeDuration = 0.05f, float time = 0) 
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
