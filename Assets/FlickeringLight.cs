using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public Light targetLight; // 公开的灯光组件
    public float minIntensity = 0f; // 最小亮度
    public float maxIntensity = 1f; // 最大亮度
    public float flickerSpeed = 0.1f; // 闪烁速度

    private float originalIntensity; // 原始亮度
    private Coroutine flickerCoroutine; // 闪烁协程的引用

    void Start()
    {
        if (targetLight == null)
        {
            targetLight = GetComponent<Light>();
        }
        originalIntensity = targetLight.intensity; // 保存原始亮度
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "FPSController")
        {
            if (flickerCoroutine != null)
            {
                StopCoroutine(flickerCoroutine);
            }
            flickerCoroutine = StartCoroutine(Flicker());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "FPSController")
        {
            if (flickerCoroutine != null)
            {
                StopCoroutine(flickerCoroutine);
                targetLight.intensity = originalIntensity; // 恢复原始亮度
            }
        }
    }

    private IEnumerator Flicker()
    {
        while (true)
        {
            targetLight.intensity = Random.Range(minIntensity, maxIntensity);
            yield return new WaitForSeconds(flickerSpeed);
        }
    }
}