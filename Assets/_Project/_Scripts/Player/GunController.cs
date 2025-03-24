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
        private string _enemyTag;
        private bool _isReloading;
        private PlayerController _playerController;
        private float _firePower;
        private bool _fire;
        private bool _isFighting = false;
        private bool _canPlayerFight = true;
        private Stats _playerStats;

        void Awake()
        {
            _isFighting = false;
            _canPlayerFight = true;
            Reload();
        }

        void Start()
        {
            _playerStats = PlayerController.Instance.Stats;
            reloadTime = _playerStats.GetStat(StatType.Cooldown);

            _playerController = GetComponentInParent<PlayerController>();
            _playerController.OnPlayerFight += ChangeFightState;
            _playerController.OnPlayerDeath += OnPlayerDeath;
            _playerController.OnPlayerRespawn += OnPlayerRespawn;
        }

        void Update()
        {
            _canPlayerFight = _playerController.CheckIfCanFight();

            if (_currentBullet == null)
            {
                Reload();
            }

            PlayerInput();
        }

        void PlayerInput()
        {
            if (_canPlayerFight)
            {
                if (Input.GetMouseButton(0) && _isFighting)
                {
                    StartCoroutine(WaitForFightAndFire(0f));
                }
                else if (Input.GetMouseButton(0) && !_isFighting)
                {
                    _playerController.ChangeDirection360();
                    _playerController.ChangeAnimation(_playerController._aimingSniperRifleHash, 0.03f);
                    StartCoroutine(WaitForFightAndFire(0.3f));
                }
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

        private void OnPlayerDeath()
        {
            _canPlayerFight = false;
            _playerController.ChangeAnimation(_playerController._deathRifleHash);
        }

        private void OnPlayerRespawn()
        {
            _canPlayerFight = true;
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
            if (_isReloading || _currentBullet == null || _canPlayerFight == false) return;

            _playerController.ChangeAnimation(_playerController._fireSniperRifleHash, 0.03f, 0, true);

            var force = spawnPoint.TransformDirection(Vector3.forward * firePower);

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
