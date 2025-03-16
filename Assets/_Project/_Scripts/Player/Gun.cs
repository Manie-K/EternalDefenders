using UnityEngine;
using System.Collections;

namespace EternalDefenders
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] float reloadTime;
        [SerializeField] Bullet bulletPrefab;
        [SerializeField] Transform spawnPoint;

        private Bullet _currentBullet;
        private string _enemyTag;
        private bool _isReloading;

        void Update()
        {
            if (_currentBullet == null)
            {
                Reload();
            }
        }

        public void SetEnemyTag(string enemyTag)
        {
            _enemyTag = enemyTag;
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
            var force = spawnPoint.TransformDirection(Vector3.forward * firePower);
            _currentBullet.Fly(force);
            _currentBullet = null;
            Reload();
            //Debug.Log("Fire");
        }

        bool IsReady()
        {
            return (!_isReloading && _currentBullet != null);
        }
    }
}
