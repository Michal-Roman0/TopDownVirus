using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField] private GameObject entityPrefab;
    [SerializeField] private float cooldown;
    [SerializeField] private bool spawn = true;
    [SerializeField] private int amount;


    public void SetupSpawner(float delay, int amount)
    {
        StartCoroutine(SpawnTimer(2f));
    }
    IEnumerator SpawnTimer(float seconds)
    {
        while (spawn && amount > 0)
        {
        yield return new WaitForSeconds(seconds);
            Instantiate(entityPrefab, new Vector3(transform.position.x + Random.Range(-3f, 3f), transform.position.y, 0), Quaternion.identity);
            amount--;
        }
        
    }
}
