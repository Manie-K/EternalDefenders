using UnityEngine;

namespace EternalDefenders
{
    public class SpawnPoint : MonoBehaviour
    {
        public void Spawn(GameObject prefab)
        {
            Instantiate(prefab, gameObject.transform.position, Quaternion.identity);
        }
    }
}
