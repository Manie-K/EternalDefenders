using UnityEngine;

namespace EternalDefenders
{
    public class SpawnPoint : MonoBehaviour
    {
        public void Spawn(EnemyController prefab)
        {
            if(prefab == null)
            {
                Debug.LogError("EnemyPrefab is null");
                return;
            }
            Instantiate(prefab, gameObject.transform.position, Quaternion.identity);
        }
    }
}
