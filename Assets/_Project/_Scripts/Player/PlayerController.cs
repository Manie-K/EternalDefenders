using System;
using MG_Utilities;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using HudElements;
using System.Collections.Generic;
using static UnityEditor.Experimental.GraphView.GraphView;
using Codice.CM.SEIDInfo;

namespace EternalDefenders
{
    public class PlayerController : Singleton<PlayerController>, IEnemyTarget
    {
        [SerializeField] PlayerStats playerStats;
        [SerializeField] Transform cameraTransform;
        [SerializeField] Transform spawnPointTransform;
        [SerializeField] float turnSmoothTime = 0.01f;
        [SerializeField] int respawnTime = 6;

        public Stats Stats { get; private set; }
        public event Action OnDeath;
        public event Action OnRespawn;
        public event Action OnFight;
        public PlayerState CurrentState { get; set; }

        CharacterController _controller;
        Transform _playerTransform;
        Animator _animator;

        public readonly int _runRifleHash = Animator.StringToHash("Run Rifle");
        public readonly int _idleRifleHash = Animator.StringToHash("Idle Rifle");
        public readonly int _aimingSniperRifleHash = Animator.StringToHash("Aiming SniperRifle");
        public readonly int _fireSniperRifleHash = Animator.StringToHash("Fire SniperRifle");
        public readonly int _deathRifleHash = Animator.StringToHash("Death Rifle");
        public readonly int _walkRifleHash = Animator.StringToHash("Walk Rifle");
        public readonly int _jumpRifleHash = Animator.StringToHash("Jump Rifle");
        public readonly int[] _damageHashes = new int[] { Animator.StringToHash("Damage1"), 
            Animator.StringToHash("Damage2"), Animator.StringToHash("Damage3") };

        private float _turnSmoothVelocity;
        private int _currentAnimationHash;
        private int _previousAnimationHash;

        private Vector3 _velocity;
        private bool _isGrounded;
        private readonly float _gravity = -9.81f;
        private float _jumpHeight = 2f;

        public bool CanFight { get; set; }
        public bool CanMove { get; set; }

        protected override void Awake()
        {
            base.Awake();

            _controller = GetComponent<CharacterController>();
            _playerTransform = transform.GetChild(0).GetComponent<Transform>();
            _animator = transform.GetChild(0).GetComponent<Animator>();

            CurrentState = PlayerState.Idle;
            _currentAnimationHash = _idleRifleHash;

            Stats = new Stats(playerStats.GetStats());

            OnDeath += OnDeath_Delegate;
            OnRespawn += OnRespawn_Delegate;
            CanMove = true;
            CanFight = true;
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

            if (Stats.GetStat(StatType.Health) <= 0)
            {
                CanFight = false;
                CanMove = false;
                CurrentState = PlayerState.Dead;
                OnDeath?.Invoke();
            }

            if (CurrentState != PlayerState.Dead && CurrentState != PlayerState.Damage)
            {
               PlayerActions();
            }
            else
            {
                CanFight = false;
                CanMove = false;
            }

            //gravity
            _isGrounded = _controller.isGrounded;

            if (_isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }

            _velocity.y += _gravity * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);

        }

        void OnDeath_Delegate()
        {
            //death
            CurrentState = PlayerState.Dead;
            CanMove = false;
            ChangeAnimation(_deathRifleHash);

            //respawn
            StartCoroutine(RespawnPlayerAfterDelay(respawnTime));
        }

        IEnumerator RespawnPlayerAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            OnRespawn?.Invoke();
        }

        void OnRespawn_Delegate()
        {
            _controller.enabled = false;
            transform.position = spawnPointTransform.position;
            _controller.enabled = true;

            Stats = new Stats(playerStats.GetStats());
            ChangeAnimation(_idleRifleHash);
            CurrentState = PlayerState.Idle;
            CanMove = true;
            CanFight = true;
        }

        void PlayerActions()
        {
            var input = InputManager.Instance;

            if (input.IsPlayerFighting && CanFight == true)
            {
                OnFight?.Invoke();
            }
            else if (input.IsPlayerMoving && CurrentState != PlayerState.Fight && CanMove == true)
            {
                if (CurrentState == PlayerState.Jump)
                {
                    MovePlayer(CurrentState);
                }
                else if (input.IsPlayerSprinting)
                {
                    CurrentState = PlayerState.Run;
                    ChangeAnimation(_runRifleHash);
                    MovePlayer(CurrentState);
                }
                else
                {
                    CurrentState = PlayerState.Walk;
                    ChangeAnimation(_walkRifleHash);
                    MovePlayer(CurrentState);
                }

            }
            else if (CurrentState == PlayerState.ReadyToFight)
            {
                CanFight = true;
                ChangeDirection360();
            }
            else if ((CurrentState != PlayerState.Fight && CurrentState != PlayerState.Jump) || CurrentState == PlayerState.Idle)
            {
                CanFight = true;
                CurrentState = PlayerState.Idle;
                ChangeAnimation(_idleRifleHash);
            }   

            if (_isGrounded && input.IsPlayerJumping)
            {
                StartCoroutine(Jump());
            }

        }

        IEnumerator Jump()
        {
            CanFight = false;
            CurrentState = PlayerState.Jump;
            _previousAnimationHash = _currentAnimationHash;
            ChangeAnimation(_jumpRifleHash, 0.01f);
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);

            yield return new WaitForSeconds(0.05f);

            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            float time = stateInfo.length;
            Debug.Log(time);

            yield return new WaitForSeconds(time);
            ChangeAnimation(_previousAnimationHash, 0.0001f);

            CurrentState = PlayerState.Idle;
            CanFight = true;
        }

        void ChangeDirection(Vector3 movementDirection)
        {         
            float targetAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(_playerTransform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, turnSmoothTime);
            
            _playerTransform.rotation = Quaternion.Euler(0f, angle, 0f);
            //cameraTransform.rotation = Quaternion.Euler(0f, angle, 0f);
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
                //cameraTransform.rotation = Quaternion.Euler(0f, angle, 0f);
            }
        }

        void MovePlayer(PlayerState state)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 forward = _playerTransform.forward;
            Vector3 right = _playerTransform.right;
            Vector3 movementDirection = new Vector3(horizontal, 0f, vertical).normalized;

            int speed;
            if (state == PlayerState.Run) speed = Stats.GetStat(StatType.Speed) * 2;
            else speed = Stats.GetStat(StatType.Speed);

            if (movementDirection.magnitude >= 0.1f)
            {
                ChangeDirection(movementDirection);
                _controller.Move(movementDirection * (speed * Time.deltaTime));
            }
        }

        public void ChangeAnimation(object animation, float crossFadeDuration = 0.05f, float time = 0, bool canLoop = false, int layer = 0) 
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

                if (_currentAnimationHash != animationHash || canLoop)
                {
                    _currentAnimationHash = animationHash;
                    _animator.CrossFade(animationHash, crossFadeDuration, layer);
                }
            }
        }

        public IEnumerator OnDamage()
        {
            CurrentState = PlayerState.Damage;
            CanFight = false;
            CanMove = false;

            System.Random random = new System.Random();
            int number = random.Next(_damageHashes.Length);
            ChangeAnimation(_damageHashes[number]);
            
            yield return new WaitForSeconds(1f);

            CurrentState = PlayerState.Idle;
            CanMove = true;
            CanFight = true;
        }

    }
}
