using System;
using MG_Utilities;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using HudElements;
using System.Collections.Generic;
using static UnityEditor.Experimental.GraphView.GraphView;

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
        public event Action OnPlayerDeath;
        public event Action OnPlayerRespawn;
        public PlayerState currentState;

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
        private bool _canFight = true;

        protected override void Awake()
        {
            base.Awake();

            _controller = GetComponent<CharacterController>();
            _playerTransform = transform.GetChild(0).GetComponent<Transform>();
            _animator = transform.GetChild(0).GetComponent<Animator>();

            currentState = PlayerState.Idle;
            _currentAnimationHash = _idleRifleHash;

            //TODO: Don't use playerStats directly, use Stats instead @FranciszekGwarek
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
            //Debug.Log(currentState);
            Stats.UpdateStatsModifiers(Time.deltaTime);

            if (currentState != PlayerState.Dead)
            {
                //check if player is alive
                if (Stats.GetStat(StatType.Health) <= 0)
                { 
                    _canFight = false;
                    currentState = PlayerState.Dead;
                    OnPlayerDeath?.Invoke();
                }
                else
                {

                    PlayerInput();
                }

            }
            else
            {
                _canFight = false;
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

        void OnPlayerDeathDelegate()
        {
            //death
            currentState = PlayerState.Dead;
            ChangeAnimation(_deathRifleHash);

            //respawn
            StartCoroutine(RespawnPlayerAfterDelay(respawnTime));
        }

        IEnumerator RespawnPlayerAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            _controller.enabled = false;
            transform.position = spawnPointTransform.position;
            _controller.enabled = true;

            Stats = new Stats(playerStats.GetStats());
            ChangeAnimation(_idleRifleHash);
            currentState = PlayerState.Idle;

            OnPlayerRespawn?.Invoke();
        }

        void PlayerInput()
        {
            if ((Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
                 && currentState != PlayerState.Damage && currentState != PlayerState.Fight)
            {
                //_canFight = false;

                if (currentState == PlayerState.Jump)
                {
                    MovePlayer(currentState);
                }
                else if (Input.GetKey(KeyCode.LeftShift))
                {
                    currentState = PlayerState.Run;
                    ChangeAnimation(_runRifleHash);
                    MovePlayer(currentState);
                }
                else
                {
                    currentState = PlayerState.Walk;
                    ChangeAnimation(_walkRifleHash);
                    MovePlayer(currentState);
                }

            }
            else if (currentState == PlayerState.ReadyToFight)
            {
                _canFight = true;
                ChangeDirection360();
            }
            else if (currentState != PlayerState.Fight && currentState != PlayerState.Damage
                    && currentState != PlayerState.Jump)
            {
                _canFight = true;
                ChangeAnimation(_idleRifleHash);
            }   

            if (_isGrounded && Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(Jump());
            }

        }

        IEnumerator Jump()
        {
            _canFight = false;
            currentState = PlayerState.Jump;
            _previousAnimationHash = _currentAnimationHash;
            ChangeAnimation(_jumpRifleHash, 0.01f);
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);

            yield return new WaitForSeconds(0.05f);

            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            float time = stateInfo.length;
            Debug.Log(time);

            yield return new WaitForSeconds(time);
            ChangeAnimation(_previousAnimationHash, 0.0001f);

            currentState = PlayerState.Idle;
            _canFight = true;
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

            //parameters change depends on player state
            int speed;
            if (state == PlayerState.Run) speed = playerStats.speed * 2;
            else speed = playerStats.speed;

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
            currentState = PlayerState.Damage;
            System.Random random = new System.Random();
            int number = random.Next(_damageHashes.Length);
            Console.WriteLine(number);
            ChangeAnimation(_damageHashes[number]);
            
            yield return new WaitForSeconds(1f);
            currentState = PlayerState.Idle;
        }

        public PlayerState GetState()
        {
            return currentState;
        }

        public bool CheckIfCanFight()
        {
            return _canFight;
        }
    }
}
