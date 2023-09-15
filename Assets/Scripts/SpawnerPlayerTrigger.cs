using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnerPlayerTrigger : MonoBehaviour
{
    public SpawnPointManager spawnPointManager;

    private bool hasSpawned;

    void OnStart()
    {
        hasSpawned = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") && !hasSpawned)
        {
            hasSpawned = true;
            spawnPointManager.SpawnAllEnemies();
        }
    }
}
