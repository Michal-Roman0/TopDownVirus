using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfo : MonoBehaviour
{
	public static GameInfo Instance { get; private set; }
	public int enemiesLeft;

	private void Awake()
	{
		Instance = this;
	}
}
