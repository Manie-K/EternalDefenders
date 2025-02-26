using System;
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
        [SerializeField] int wavePowerIncreasePerWave = 1;
        [SerializeField] float timeToFisrtWave = 5;
        [SerializeField] float minTimeBetweenWaves = 5;

        Coroutine _enemyWavesCoroutine;
        SpawnManager _spawnManager;

        void Start()
        {
            _spawnManager = GetComponent<SpawnManager>();
            if (spawningEnabled)
                _enemyWavesCoroutine = StartCoroutine(SpawnEnemyWaves());
        }

        IEnumerator SpawnEnemyWaves()
        {
            yield return new WaitForSeconds(timeToFisrtWave);

            while (spawningEnabled)
            {
                yield return StartCoroutine(_spawnManager.SpawnWave());
                yield return new WaitForSeconds(CalcTimeBetweenWaves());
                GameManager.Instance.WavePower += wavePowerIncreasePerWave;
            }
        }

        float CalcTimeBetweenWaves()
        {
            float timeBetweenWaves = 5;

            if (timeBetweenWaves > minTimeBetweenWaves)
            {
                return timeBetweenWaves;
            }
            else
            { 
                return minTimeBetweenWaves;
            }
        }
    }
}
