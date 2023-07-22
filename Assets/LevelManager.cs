using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private EntitySpawner es;
	[SerializeField] private TextMeshProUGUI levelText;
	[SerializeField] private int levelAmount = 4;
    [SerializeField] private int[] enemiesPerLevel;


	[SerializeField] private GameObject entityPrefab;
	[SerializeField] private float cooldown;
	[SerializeField] private bool spawn = true;
	[SerializeField] private int amount;
	private bool levelDefeated = false;

	private void Start()
	{
		StartCoroutine(SpawnTimer(cooldown));
	}

	IEnumerator SpawnTimer(float seconds)
	{
		for (int i = 0; i < enemiesPerLevel.Length; i++)
		{
			amount = enemiesPerLevel[i];
			GameInfo.Instance.enemiesLeft = amount;

			levelText.text = "Level: " + (i + 1);

			levelText.enabled = true;
			yield return new WaitForSeconds(2.5f);
			levelText.enabled = false;

			while (spawn && amount > 0)
			{
				yield return new WaitForSeconds(seconds);
				Instantiate(entityPrefab, new Vector3(transform.position.x + Random.Range(-3f, 3f), transform.position.y, 0), Quaternion.identity);
				amount--;
			}

			yield return new WaitUntil(() => GameInfo.Instance.enemiesLeft == 0);

		}
		levelText.enabled = true;
		levelText.text = "Tutorial completed!";	
	}
}
