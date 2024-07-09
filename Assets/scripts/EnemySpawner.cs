using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int enemyCount = 3;

    void Start()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Instantiate(enemyPrefab);
        }
    }
}
