using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting.Dependencies.NCalc;
using System.Security.Cryptography;
using Unity.VisualScripting;

public class UniversalBar : MonoBehaviour
{
	public Text infoText;
	public Image infoBar;

	float barValue;
	public float maxValue = 20f;

	private void Start()
	{
		barValue = maxValue;
	}
	public void SetBarValue(float value)
	{
		barValue = value;
		if (value >= maxValue)
		{
			infoBar.color = Color.cyan;
		}
		else infoBar.color = Color.white;
		infoBar.fillAmount = barValue / maxValue;
	}
	public void IncreaseBarValue(float howMuch)
	{
		if (barValue < maxValue)
		{
			barValue += howMuch;
		}
		else barValue = maxValue;

		infoBar.fillAmount = barValue / maxValue;
	}
	public void DecreaseBarValue(float howMuch)
	{
		if (barValue > 0)
		{
			barValue -= howMuch;
		}
		else barValue = 0;

		infoBar.fillAmount = barValue / maxValue;
	}
}
