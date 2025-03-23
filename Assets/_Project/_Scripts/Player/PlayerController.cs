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
        [SerializeField] PlayerStats playerStats;
        [SerializeField] Transform cameraTransform;
        [SerializeField] Transform spawnPointTransform;
        [SerializeField] float turnSmoothTime = 0.01f;

        public Stats Stats { get; private set; }
        public event Action OnPlayerDeath;
        public event Action OnPlayerRespawn;
        public event Action<bool> OnPlayerFight;

        CharacterController _controller;
        Transform _playerTransform;
        Animator _animator;

        public readonly int _idleHash = Animator.StringToHash("Idle");
        public readonly int _runningHash = Animator.StringToHash("Run");
        public readonly int _runningRifleHash = Animator.StringToHash("Run Rifle");
        public readonly int _idleRifleHash = Animator.StringToHash("Idle Rifle");
        public readonly int _aimingSniperRifleHash = Animator.StringToHash("Aiming SniperRifle");
        public readonly int _fireSniperRifleHash = Animator.StringToHash("Fire SniperRifle");
        public readonly int _deathRifleHash = Animator.StringToHash("Death Rifle");
        public readonly int[] _damageHashes = new int[] { Animator.StringToHash("Damage1"), 
            Animator.StringToHash("Damage2"), Animator.StringToHash("Damage3") };

        private float _turnSmoothVelocity;
        private int _currentAnimationHash;
        private PlayerState _currentState;

        private Vector3 _velocity;
        private bool _isGrounded;
        private readonly float _gravity = -9.81f;

        protected override void Awake()
        {
            base.Awake();

            _controller = GetComponent<CharacterController>();
            _playerTransform = transform.GetChild(0).GetComponent<Transform>();
            _animator = transform.GetChild(0).GetComponent<Animator>();

            _currentState = PlayerState.Idle;
            _currentAnimationHash = _idleRifleHash;

            Stats = new Stats(playerStats.GetStats());

            OnPlayerDeath += OnPlayerDeathDelegate;
        }

        void Start()
        {
            _controller.enabled = false;
            transform.position = spawnPointTransform.position;
            _controller.enabled = true;

            ChangeAnimation(_idleRifleHash);
        }

        void Update()
        {
            Stats.UpdateStatsModifiers(Time.deltaTime);

            if (_currentState != PlayerState.Dead)
            {
                //check if player is alive
                if (Stats.GetStat(StatType.Health) <= 0)
                {
                    _currentState = PlayerState.Dead;
                    OnPlayerDeath?.Invoke();
                }
                else
                {

                    PlayerInput();

                    //gravity
                    _isGrounded = _controller.isGrounded;

                    if (_isGrounded && _velocity.y < 0)
                    {
                        _velocity.y = -2f;
                    }

                    _velocity.y += _gravity * Time.deltaTime;
                    _controller.Move(_velocity * Time.deltaTime);
                }

            }
        }

        void OnPlayerDeathDelegate()
        {
            //death
            _currentState = PlayerState.Dead;
            ChangeAnimation(_deathRifleHash);

            //respawn
            StartCoroutine(RespawnPlayerAfterDelay(6f));
        }

        IEnumerator RespawnPlayerAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            _controller.enabled = false;
            transform.position = spawnPointTransform.position;
            _controller.enabled = true;

            Stats = new Stats(playerStats.GetStats());
            ChangeAnimation(_idleRifleHash);
            _currentState = PlayerState.Idle;

            OnPlayerRespawn?.Invoke();
        }

        void PlayerInput()
        {
            if (Input.GetMouseButton(0) && _currentState != PlayerState.Fight)
            {
                ChangeDirection360();
                _currentState = PlayerState.Fight;
                //OnPlayerFight?.Invoke(true);
            }
            else if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                _currentState = PlayerState.Run;
                MovePlayer();
                OnPlayerFight?.Invoke(false);
            }
            else if (_currentState != PlayerState.Fight)
            {
                ChangeAnimation(_idleRifleHash);
                OnPlayerFight?.Invoke(false);
            }
            else if (_currentState == PlayerState.Fight)
            {
                ChangeDirection360();
            }

        }

        void ChangeDirection(Vector3 movementDirection)
        {         
            float targetAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(_playerTransform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, turnSmoothTime);
            
            _playerTransform.rotation = Quaternion.Euler(0f, angle, 0f);
            cameraTransform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        public void ChangeDirection360()
        {
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
            Vector3 movementDirection = new Vector3(horizontal, 0f, vertical).normalized;

            if (movementDirection.magnitude >= 0.1f)
            {
                ChangeDirection(movementDirection);
                ChangeAnimation(_runningRifleHash);
                _controller.Move(movementDirection * (playerStats.speed * Time.deltaTime));
            }
        }

        public void ChangeAnimation(object animation, float crossFadeDuration = 0.05f, float time = 0, bool canLoop = false) 
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
                int animationHash;
                if (animation is string) animationHash = Animator.StringToHash((string)animation);
                else animationHash = (int)animation;

                if (_currentAnimationHash != animationHash && !canLoop)
                {
                    _currentAnimationHash = animationHash;
                    _animator.CrossFade(animationHash, crossFadeDuration);
                }
                else if (_currentAnimationHash != animationHash && canLoop)
                {
                    _currentAnimationHash = animationHash;
                    _animator.CrossFade(animationHash, crossFadeDuration);
                }
            }
        }

        public void GetDamage()
        {
            System.Random random = new System.Random();
            int number = random.Next(_damageHashes.Length);
            Console.WriteLine(number);
            ChangeAnimation(_damageHashes[number]);
        }

        public PlayerState GetState()
        {
            return _currentState;
        }
    }
}
