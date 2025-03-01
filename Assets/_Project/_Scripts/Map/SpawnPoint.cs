using UnityEngine;

namespace EternalDefenders
{
    public abstract class SpawnPoint : MonoBehaviour
    {
        public virtual void Spawn(EnemyController prefab, Transform parent)
        {
            if (prefab == null)
            {
                Debug.LogError("EnemyPrefab is null");
                return;
            }
            Instantiate(prefab, transform.position, Quaternion.identity, parent);
        }
    }
}
