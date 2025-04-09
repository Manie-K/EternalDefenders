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
        private bool _canPlayerFight = true;
        private Stats _playerStats;

        void Awake()
        {
            _canPlayerFight = true;
            Reload();
        }

        void Start()
        {
            _playerStats = PlayerController.Instance.Stats;
            reloadTime = _playerStats.GetStat(StatType.Cooldown);

            _playerController = GetComponentInParent<PlayerController>();
            _playerController.OnDeath += OnPlayerDeath_Delagate;
            _playerController.OnRespawn += OnPlayerRespawn_Delagate;
            _playerController.OnFight += OnPlayerFight_Delagate;
        }

        void Update()
        {
            _canPlayerFight = _playerController.CheckIfCanFight();

            if (_currentBullet == null)
            {
                Reload();
            }
        }

        private IEnumerator WaitForFightAndFire(float waitingTime)
        {
            yield return new WaitForSeconds(waitingTime);

            _firePower = maxFirePower;
            Fire(_firePower);
            _firePower = 0;
        }

        private void OnPlayerDeath_Delagate()
        {
            _canPlayerFight = false;
            _playerController.ChangeAnimation(_playerController._deathRifleHash);
        }

        private void OnPlayerRespawn_Delagate()
        {
            _canPlayerFight = true;
        }

        private void OnPlayerFight_Delagate()
        {
            if (_canPlayerFight && !_isReloading && _currentBullet != null)
            {
                if (_playerController.currentState == PlayerState.ReadyToFight)
                {
                    _playerController.currentState = PlayerState.Fight;
                    _playerController.CanMove = false;
                    _playerController.ChangeDirection360();
                    StartCoroutine(WaitForFightAndFire(0f));
                }
                else if (_playerController.currentState != PlayerState.ReadyToFight)
                {
                    _playerController.currentState = PlayerState.Fight;
                    _playerController.CanMove = false;
                    _playerController.ChangeDirection360();
                    _playerController.ChangeAnimation(_playerController._aimingSniperRifleHash, 0.03f);
                    StartCoroutine(WaitForFightAndFire(0.3f));
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
            if (_isReloading || _currentBullet == null || _canPlayerFight == false)
            {
                _playerController.CanMove = true;
                _playerController.currentState = PlayerState.ReadyToFight;
                return;
            }

            _playerController.ChangeAnimation(_playerController._fireSniperRifleHash, 0.03f);

            var force = spawnPoint.TransformDirection(Vector3.forward * firePower);

            _currentBullet.Fly(force);
            _currentBullet = null;
            
            Reload();
            _playerController.CanMove = true;
            _playerController.currentState = PlayerState.ReadyToFight;
        }

        bool IsReady()
        {
            return (!_isReloading && _currentBullet != null);
        }
    }
}
