using UnityEngine;
using System.Collections;

namespace EternalDefenders
{
    public class GunController : MonoBehaviour
    {
        [SerializeField] float reloadTime;
        [SerializeField] Bullet bulletPrefab;
        [SerializeField] Transform spawnPoint;
        [SerializeField] string enemyTag;
        [SerializeField] float maxFirePower;
        [SerializeField] float firePowerSpeed;
        [SerializeField] float rotateSpeed;
        [SerializeField] float minRotation;
        [SerializeField] float maxRotation;

        private Bullet _currentBullet;
        private string _enemyTag;
        private bool _isReloading;
        private PlayerController _playerController;
        private float _firePower;
        private bool _fire;
        private bool _isFighting = false;

        readonly int _fireSniperRifleHash = Animator.StringToHash("Fire SniperRifle");
        readonly int _aimingSniperRifleHash = Animator.StringToHash("Aiming SniperRifle");

        void Awake()
        {
            _isFighting = false;
            Reload();
        }

        void Start()
        {
            _playerController = GetComponentInParent<PlayerController>();
            _playerController.OnPlayerFight += ChangeFightState;
        }

        void Update()
        {
            if (_currentBullet == null)
            {
                Reload();
            }

            PlayerInput();
        }

        void PlayerInput()
        {
            if (Input.GetMouseButton(0) && _isFighting)
            {
                StartCoroutine(WaitForFightAndFire(0f));
            }
            else if (Input.GetMouseButton(0) && !_isFighting)
            {
                _playerController.ChangeDirection();
                _playerController.ChangeAnimation(_aimingSniperRifleHash, 0.03f);
                StartCoroutine(WaitForFightAndFire(0.3f));
            }

        }

        private IEnumerator WaitForFightAndFire(float waitingTime)
        {
            yield return new WaitForSeconds(waitingTime);

            _isFighting = true;

            _firePower = maxFirePower;
            Fire(_firePower);
            _firePower = 0;
        }

        private void ChangeFightState(bool isFighting)
        {
            _isFighting = isFighting;
        }

        public void Reload()
        {
            if (_isReloading || _currentBullet != null) return;
            _isReloading = true;
            StartCoroutine(ReloadAfterTime());
        }

        IEnumerator ReloadAfterTime()
        {
            yield return new WaitForSeconds(reloadTime);
            _currentBullet = Instantiate(bulletPrefab, spawnPoint);
            _currentBullet.SetEnemyTag(_enemyTag);
            _currentBullet.transform.localPosition = Vector3.zero;
            _isReloading = false;
        }

        public void Fire(float firePower)
        {
            if (_isReloading || _currentBullet == null) return;

            _playerController.ChangeDirection();
            
            var force = spawnPoint.TransformDirection(Vector3.forward * firePower);
            _playerController.ChangeAnimation(_fireSniperRifleHash, 0.03f, 0, true);

            _currentBullet.Fly(force);
            _currentBullet = null;
            
            Reload();
        }

        bool IsReady()
        {
            return (!_isReloading && _currentBullet != null);
        }
    }
}
