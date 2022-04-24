using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] int startingWave = 0;                                                       // serialized in case we want to start a different wave level later on
    [SerializeField] bool looping = false;                                                        // looping boolean, serialized to turn on off from unity manually

    // Start is called before the first frame update
    IEnumerator Start()                                                                           // starting coroutine code ( do - while) 
    {
        do
        {
            yield return StartCoroutine(SpawnAllWaves());
        }
        while (looping);                                                                         // if on, loops coroutine indefnitely, if off, would do coroutine once and stop
    }

    private IEnumerator SpawnAllWaves()                                                          // spawn all waves 
    {
        for (int waveIndex = startingWave; waveIndex < waveConfigs.Count; waveIndex++)           // looping all waves remaining starting with the "starting wave number" 
        {
            var currentWave = waveConfigs[waveIndex];                                             // variable of type WaveConfig defining which wave asset file using atm, starting with the first one
            yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));                      // start the spawnallenemies coroutine and wait till it finishes before going back to the for loop
        }
    }
    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)                             // spawn all enemies in wave
    {
        for (int enemyCount = 0; enemyCount < waveConfig.GetSpawnCount(); enemyCount++)          // for each enemies in wave asset file
        {
            var newEnemy = Instantiate(
                waveConfig.GetEnemyType(),
                waveConfig.GetWayPoints()[0].transform.position,
                Quaternion.identity);                                                            // create an emey object depending on linked prefab enemy to wave, that spawns on intial position of path prefab
            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);                            // set waveconfig to the enemy created so it can be used in "Enemy.cs"
            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());                  // wait time between each enemy
        }
    }
}
