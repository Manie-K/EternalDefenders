using UnityEngine;
using System.Collections;

namespace EternalDefenders
{
    public class GunController : MonoBehaviour
    {
        [SerializeField] int reloadTime;
        [SerializeField] Bullet bulletPrefab;
        [SerializeField] Transform spawnPoint;
        [SerializeField] string enemyTag;
        [SerializeField] float maxFirePower;
        [SerializeField] float firePowerSpeed;
        [SerializeField] float rotateSpeed;
        [SerializeField] float minRotation;
        [SerializeField] float maxRotation;

        private Bullet _currentBullet;
        private bool _isReloading;
        private PlayerController _playerController;
        private float _firePower;
        private bool _fire;
        private Stats _playerStats;

        void Awake()
        {
            Reload();
        }

        void Start()
        {
            _playerStats = PlayerController.Instance.Stats;
            reloadTime = _playerStats.GetStat(StatType.Cooldown);

            _playerController = GetComponentInParent<PlayerController>();
            _playerController.OnDeath += OnPlayerDeath_Delegate;
            _playerController.OnRespawn += OnPlayerRespawn_Delegate;
            _playerController.OnFight += OnPlayerFight_Delegate;
        }

        void Update()
        {
            if (_currentBullet == null)
            {
                Reload();
            }
        }

        private IEnumerator WaitAndFire(float waitingTime)
        {
            yield return new WaitForSeconds(waitingTime);

            _firePower = maxFirePower;
            Fire(_firePower);
            _firePower = 0;
        }

        private void OnPlayerDeath_Delegate()
        {
            _playerController.ChangeAnimation(_playerController._deathRifleHash);
        }

        private void OnPlayerRespawn_Delegate()
        {
        }

        private void OnPlayerFight_Delegate()
        {
            if (_playerController.CanFight && !_isReloading && _currentBullet != null)
            {
                if (_playerController.CurrentState == PlayerState.ReadyToFight)
                {
                    _playerController.CurrentState = PlayerState.Fight;
                    _playerController.CanMove = false;
                    _playerController.ChangeDirection360();
                    StartCoroutine(WaitAndFire(0f));
                }
                else if (_playerController.CurrentState != PlayerState.ReadyToFight)
                {
                    _playerController.CurrentState = PlayerState.Fight;
                    _playerController.CanMove = false;
                    _playerController.ChangeDirection360();
                    _playerController.ChangeAnimation(_playerController._aimingSniperRifleHash, 0.03f);
                    StartCoroutine(WaitAndFire(0.3f));
                }
            }
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
            _currentBullet.SetEnemyTag(enemyTag);
            _currentBullet.transform.localPosition = Vector3.zero;
            _isReloading = false;
        }

        public void Fire(float firePower)
        {
            if (_isReloading || _currentBullet == null || _playerController.CanFight == false)
            {
                _playerController.CanMove = true;
                _playerController.CurrentState = PlayerState.ReadyToFight;
                return;
            }

            _playerController.ChangeAnimation(_playerController._fireSniperRifleHash, 0.03f);

            var force = spawnPoint.TransformDirection(Vector3.forward * firePower);

            _currentBullet.Fly(force);
            _currentBullet = null;
            
            Reload();
            _playerController.CanMove = true;
            _playerController.CurrentState = PlayerState.ReadyToFight;
        }

        bool IsReady()
        {
            return (!_isReloading && _currentBullet != null);
        }
    }
}
