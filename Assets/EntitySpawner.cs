using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField] private GameObject entityPrefab;
    [SerializeField] private float cooldown;
    [SerializeField] private bool spawn = true;
    void Start()
    {
        StartCoroutine(SpawnTimer(cooldown));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator SpawnTimer(float seconds)
    {
        while (spawn)
        {
        yield return new WaitForSeconds(seconds);
            Instantiate(entityPrefab, new Vector3(transform.position.x + Random.Range(-3f, 3f), transform.position.y, 0), Quaternion.identity);

        }
        
    }
}
