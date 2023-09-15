using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
    public List<Transform> spawnPoints;
    public GameObject enemyPrefab;

    public float upperLimit;
    public float lowerLimit;

    public void SpawnAllEnemies()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            StartCoroutine(
                SpawnEnemies(
                    (int)Mathf.Round(UnityEngine.Random.Range(lowerLimit, upperLimit)),
                    enemyPrefab,
                    spawnPoint
                )
            );
        }
    }

    IEnumerator SpawnEnemies(int numberToSpawn, GameObject enemy, Transform spawnPoint)
    {
        for (int i = 0; i < numberToSpawn; i++)
        {
            Instantiate(
                enemy,
                new Vector3(
                    spawnPoint.position.x + UnityEngine.Random.Range(-3f, 3f),
                    spawnPoint.position.y,
                    100
                ),
                spawnPoint.rotation
            );
            yield return new WaitForSeconds(1.5f);
        }
    }
}
