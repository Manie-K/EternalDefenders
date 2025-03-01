using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EternalDefenders
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] Transform enemiesParent;
        [SerializeField] List<EnemyController> enemyTier1Prefabs;
        [SerializeField] List<EnemyController> enemyTier2Prefabs;
        [SerializeField] List<EnemyController> enemyTier3Prefabs;
        [SerializeField] List<EnemyController> enemyTier4Prefabs;

        SpawnerSpawnPoint[] _enemySpawners;
        MapSpawnPoint[] _enemySpawnPointsForLevelStart;
        List<int> _minWavePowerForTier;
        //TODO change enemy power to list for each tier or add individual power for each enemy
        int _singleEnemyWavePower = 1;
        float _timeBetweenEnemySpawns = 0.5f;

        void Start()
        {
            CalcEnemyTierWaveDifficulty();
            _enemySpawners = FindObjectsByType<SpawnerSpawnPoint>(FindObjectsSortMode.InstanceID);
            _enemySpawnPointsForLevelStart = FindObjectsByType<MapSpawnPoint>(FindObjectsSortMode.InstanceID);
            PopulateMap();
        }

        public IEnumerator SpawnWave()
        {
            List<EnemyController> enemiesToSpawn = GetEnemiesToSpawn();

            foreach (EnemyController enemy in enemiesToSpawn)
            {
                int spawnerIndex = Random.Range(0, _enemySpawners.Length);
                _enemySpawners[spawnerIndex].Spawn(enemy, enemiesParent);
                yield return new WaitForSeconds(_timeBetweenEnemySpawns);
            }
        }

        List<EnemyController> GetEnemiesToSpawn()
        { 
            List<EnemyController> enemiesToSpawn = new List<EnemyController>();
            int maxTier = GetMaxEnemyTier();
            int enemiesWavePower = 0;
            // How many times we failed to get proper enemy. This is to prevent waiting too long
            int misses = 0;

            while (enemiesWavePower < GameManager.Instance.WavePower && misses < 100)
            { 
                int tier = Random.Range(1, maxTier + 1);
                int enemyIndex = 0;

                switch (tier)
                { 
                    case 1:
                        enemyIndex = Random.Range(0, enemyTier1Prefabs.Count);
                        if (_singleEnemyWavePower + enemiesWavePower <= GameManager.Instance.WavePower)
                        {
                            enemiesToSpawn.Add(enemyTier1Prefabs[enemyIndex]);
                            enemiesWavePower += _singleEnemyWavePower;
                        }
                        else
                        { 
                            misses++;
                        }
                        break;
                    case 2:
                        enemyIndex = Random.Range(0, enemyTier2Prefabs.Count);
                        if (_singleEnemyWavePower + enemiesWavePower <= GameManager.Instance.WavePower)
                        {
                            enemiesToSpawn.Add(enemyTier2Prefabs[enemyIndex]);
                            enemiesWavePower += _singleEnemyWavePower;
                        }
                        else
                        {
                            misses++;
                        }
                        break;
                    case 3:
                        enemyIndex = Random.Range(0, enemyTier3Prefabs.Count);
                        if (_singleEnemyWavePower + enemiesWavePower <= GameManager.Instance.WavePower)
                        {
                            enemiesToSpawn.Add(enemyTier3Prefabs[enemyIndex]);
                            enemiesWavePower += _singleEnemyWavePower;
                        }
                        else
                        {
                            misses++;
                        }
                        break;
                    case 4:
                        enemyIndex = Random.Range(0, enemyTier4Prefabs.Count);
                        if (_singleEnemyWavePower + enemiesWavePower <= GameManager.Instance.WavePower)
                        {
                            enemiesToSpawn.Add(enemyTier4Prefabs[enemyIndex]);
                            enemiesWavePower += _singleEnemyWavePower;
                        }
                        else
                        {
                            misses++;
                        }
                        break;
                    default:
                        misses++; 
                        break;
                }
            }

            return enemiesToSpawn;
        }

        void CalcEnemyTierWaveDifficulty()
        {
            _minWavePowerForTier = new List<int>(4) {0, 2, 3, 4};
        }

        void PopulateMap()
        {
            //TODO maybe we should change the way of obtaining enemies (GetEnemiesToSpawn is for waves purpose mainly)
            List<EnemyController> enemiesToSpawn = GetEnemiesToSpawn();
            MainBaseController mainBase = FindFirstObjectByType<MainBaseController>();
            float minDistanceFromBase = 20;
            bool spawned = false;

            foreach (EnemyController enemy in enemiesToSpawn)
            {
                spawned = false;
                while (!spawned)
                {
                    int spawnPointIndex = Random.Range(0, _enemySpawnPointsForLevelStart.Length);
                    if (Vector3.Distance(mainBase.transform.position, 
                        _enemySpawnPointsForLevelStart[spawnPointIndex].transform.position) >= minDistanceFromBase)
                    {
                        _enemySpawnPointsForLevelStart[spawnPointIndex].Spawn(enemy, enemiesParent);
                        spawned = true;
                    }
                }
            }
        }

        int GetMaxEnemyTier()
        {
            int maxEnemyTier = 0;
            int currentWavePower = GameManager.Instance.WavePower;

            foreach (int wavePower in _minWavePowerForTier)
            {
                if (currentWavePower >= wavePower)
                {
                    maxEnemyTier++;
                }
            }

            return maxEnemyTier;
        }
    }
}
