using UnityEngine;
using System.Collections;

namespace EternalDefendersPrototype
{
    public class Crossbow : MonoBehaviour
    {
        [SerializeField]
        private float reloadTime;

        [SerializeField]
        private Arrow arrowPrefab;

        [SerializeField]
        private Transform spawnPoint;

        private Arrow currentArrow;

        private string enemyTag;

        private bool isReloading;

        public void SetEnemyTag(string enemyTag)
        {
            this.enemyTag = enemyTag;
        }

        public void Reload()
        {
            if (isReloading || currentArrow != null) return;
            isReloading = true;
            StartCoroutine(ReloadAfterTime());
        }

        private IEnumerator ReloadAfterTime()
        {
            yield return new WaitForSeconds(reloadTime);
            currentArrow = Instantiate(arrowPrefab, spawnPoint);
            currentArrow.transform.localPosition = Vector3.zero;
            currentArrow.SetEnemyTag(enemyTag);
            isReloading = false;
        }

        public void Fire(float firePower)
        {
            if (isReloading || currentArrow == null) return;
            var force = spawnPoint.TransformDirection(Vector3.forward * firePower);
            currentArrow.Fly(force);
            currentArrow = null;
            Reload();
        }

        public bool IsReady()
        {
            return (!isReloading && currentArrow != null);
        }
    }
}
