using UnityEngine;

namespace EternalDefenders
{
    public abstract class SpawnPoint : MonoBehaviour
    {
        public virtual void Spawn(EnemyController prefab)
        {
            if (prefab == null)
            {
                Debug.LogError("EnemyPrefab is null");
                return;
            }
            Instantiate(prefab, gameObject.transform.position, Quaternion.identity);
        }
    }
}
