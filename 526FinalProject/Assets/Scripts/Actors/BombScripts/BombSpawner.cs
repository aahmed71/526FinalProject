using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private GameObject bombInstance;
    [SerializeField] private GameObject obstacle;
    void Update()
    {
        if (bombInstance == null && obstacle != null)
        {
            bombInstance = Instantiate(bombPrefab, transform.position, Quaternion.identity);
            GameManager.Instance.CountBombSpawns();
        }
    }
}
