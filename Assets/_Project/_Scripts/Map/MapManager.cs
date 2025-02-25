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
        
        Coroutine _enemySpawnCoroutine;
        SpawnManager _spawnManager;

        void Start()
        {
            _spawnManager = GetComponent<SpawnManager>();
            if (spawningEnabled)
                _enemySpawnCoroutine = StartCoroutine(SpawnEnemies());
        }

        IEnumerator SpawnEnemies()
        {
            yield return new WaitForSeconds(5);
            while (true)
            {
                yield return StartCoroutine(_spawnManager.SpawnWave());
                yield return new WaitForSeconds(5);
                GameManager.Instance.WavePower++;
            }
        }
    }
}
