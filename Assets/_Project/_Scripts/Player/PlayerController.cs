using System;
using MG_Utilities;
using System.Collections;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Serialization;

namespace EternalDefenders
{
    public class PlayerController : Singleton<PlayerController>, IEnemyTarget
    {
        [SerializeField] Transform cameraTransform;
        [SerializeField] float speed = 6f;
        [SerializeField] float turnSmoothTime = 0.01f;
        
        public Stats Stats { get; } //sorki ale interface musi byc zaimplementowany
        public event Action OnPlayerDeath;
        
        CharacterController _controller;
        Transform _playerTransform;
        Animator _animator;

        float _turnSmoothVelocity;
        int _currentAnimationHash = 0;
        readonly int _idleHash = Animator.StringToHash("Idle");
        readonly int _runningHash = Animator.StringToHash("Running");

        void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _playerTransform = transform.GetChild(0).GetComponent<Transform>();
            _animator = transform.GetChild(0).GetComponent<Animator>();
        }

        void Start()
        {
            ChangeAnimation(_idleHash);
        }

        void Update()
        {
            ChangeDirection();
            MovePlayer();
            
            //if(_stats.GetStat(StatType.Health) <= 0) OnPlayerDeath?.Invoke();
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
