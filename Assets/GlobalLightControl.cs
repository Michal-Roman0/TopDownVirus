using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GlobalLightControl : MonoBehaviour
{
	[SerializeField] private float intens = 0.3f;
	private bool lightsOn = true;
	public void SwitchLights()
	{
		if (lightsOn) gameObject.GetComponent<Light2D>().intensity = 0f;
		else gameObject.GetComponent<Light2D>().intensity = intens;

		lightsOn = !lightsOn;
	}
}
