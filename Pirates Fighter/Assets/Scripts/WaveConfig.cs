using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "WaveConfig")]
public class WaveConfig : ScriptableObject
{
    // Asset file variables to be customized within unity
    [SerializeField] GameObject enemyType;
    [SerializeField] GameObject pathPrefab;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float spawnRate = 0.5f;
    [SerializeField] float spawnRandomFactor = 0.3f;
    [SerializeField] int spawnCount = 5;


    public GameObject GetEnemyType()  
    {
        return enemyType;
    }    // Enemy object from Wave Asset
    public List<Transform> GetWayPoints()                                    // get transform (position info of all waves in list)
    {
        var waveWaypoints = new List<Transform>();                           // create new empty list
        foreach (Transform child in pathPrefab.transform)                   // create variable child within the path prefab as it includes sub-game objects in a for loop
        {
            waveWaypoints.Add(child);                                       // add each position to the list created
        }
        return waveWaypoints;                                               // return all list
    }
    public float GetMoveSpeed()
    {
        return moveSpeed;
    }    // Enemy move speed from Wave Asset
    public float GetTimeBetweenSpawns()
    {
        return spawnRate;
    }   // spawn rate // time between spawns from Wave Asset
    public float GetSpawnRandomFactor()
    {
        return spawnRandomFactor;
    }   // random factor for spawn rate from Wave Asset
    public int GetSpawnCount()
    {
        return spawnCount;
    }   // Enemy count from Wave Asset
}
