using UnityEngine;
using System.Collections;

namespace EternalDefendersPrototype
{
    public class Gun : MonoBehaviour
    {
        [SerializeField]
        private float reloadTime;

        [SerializeField]
        private Bullet bulletPrefab;

        [SerializeField]
        private Transform spawnPoint;

        private Bullet currentBullet;

        private string enemyTag;

        private bool isReloading;

        public void SetEnemyTag(string enemyTag)
        {
            this.enemyTag = enemyTag;
        }

        public void Reload()
        {
            if (isReloading || currentBullet != null) return;
            isReloading = true;
            StartCoroutine(ReloadAfterTime());
        }

        private IEnumerator ReloadAfterTime()
        {
            yield return new WaitForSeconds(reloadTime);
            currentBullet = Instantiate(bulletPrefab, spawnPoint);
            currentBullet.transform.localPosition = Vector3.zero;
            currentBullet.SetEnemyTag(enemyTag);
            isReloading = false;
        }

        public void Fire(float firePower)
        {
            if (isReloading || currentBullet == null) return;
            var force = spawnPoint.TransformDirection(Vector3.forward * firePower);
            currentBullet.Fly(force);
            currentBullet = null;
            Reload();
        }

        public bool IsReady()
        {
            return (!isReloading && currentBullet != null);
        }
    }
}
