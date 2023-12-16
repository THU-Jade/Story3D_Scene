using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public float minIntensity = 0f;
    public float maxIntensity = 1f;
    public float flickerSpeed = 0.1f;

    private Light myLight;
    private float random;

    void Start()
    {
        myLight = GetComponent<Light>();
        StartCoroutine(Flicker());
    }

    private IEnumerator Flicker()
    {
        while (true)
        {
            random = Random.Range(minIntensity, maxIntensity);
            myLight.intensity = random;
            yield return new WaitForSeconds(flickerSpeed);
        }
    }
}