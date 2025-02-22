using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EternalDefenders
{
    public class MapManager : MonoBehaviour
    {
        //TODO make good data for containing spawning data
        //also should be singleton?
        [SerializeField] bool spawningEnabled = true;
        [SerializeField] List<EnemyController> enemyPrefabs;
        
        SpawnPoint[] _enemySpawnPoints;
        Coroutine _enemySpawnCoroutine;

        void Start()
        {
            _enemySpawnPoints = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.InstanceID);

            if (spawningEnabled)
                _enemySpawnCoroutine = StartCoroutine(SpawnEnemies());
        }

        IEnumerator SpawnEnemies()
        {
            yield return new WaitForSeconds(10);
            while (true)
            {
                int enemyIndex = Random.Range(0, enemyPrefabs.Count);
                int spawnerIndex = Random.Range(0, _enemySpawnPoints.Length);
                _enemySpawnPoints[spawnerIndex].Spawn(enemyPrefabs[enemyIndex]);
                yield return new WaitForSeconds(3);
            }
        }
    }
}
