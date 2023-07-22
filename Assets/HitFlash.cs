using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HitFlash : MonoBehaviour
{
    //[SerializeField] private float flashIntensity;
    [SerializeField] private float returnSpeed;
	[SerializeField] private Light2D shieldLight;

	[SerializeField] private float startIntensity;
    void Awake()
    {
        startIntensity = shieldLight.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        shieldLight.intensity = Mathf.Lerp(shieldLight.intensity, startIntensity, returnSpeed * Time.deltaTime);
    }
    public void Flash(float flashIntensity)
    {
        shieldLight.intensity = flashIntensity;
    }
}
