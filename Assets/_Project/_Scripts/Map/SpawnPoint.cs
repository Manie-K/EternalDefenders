using UnityEngine;

namespace EternalDefenders
{
    public class SpawnPoint : MonoBehaviour
    {
        public void Spawn(EnemyController prefab, Transform parent)
        {
            if(prefab == null)
            {
                Debug.LogError("EnemyPrefab is null");
                return;
            }
            Instantiate(prefab, transform.position, Quaternion.identity, parent);
        }
    }
}
