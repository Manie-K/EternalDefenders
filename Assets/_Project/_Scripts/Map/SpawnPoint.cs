using UnityEngine;

namespace EternalDefenders
{
    public class SpawnPoint : MonoBehaviour
    {
        public void Spawn(EnemyController prefab)
        {
            Instantiate(prefab, gameObject.transform.position, Quaternion.identity);
        }
    }
}
