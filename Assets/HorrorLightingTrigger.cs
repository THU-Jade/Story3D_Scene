using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorrorLightingTrigger : MonoBehaviour
{
    public AudioClip soundEffect; // 要播放的音效
    private AudioSource audioSource; // 音频源组件
    private Light[] lights; // 场景中所有的灯光
    private Color originalColor; // 用于存储原始颜色

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        lights = FindObjectsOfType<Light>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "FPSController")
        {
            if (soundEffect != null)
            {
                audioSource.PlayOneShot(soundEffect);
            }
            StartCoroutine(FlickerLights());
        }
    }

    private IEnumerator FlickerLights()
    {
        foreach (Light light in lights)
        {
            if (light.type != LightType.Directional)
            {
                // 存储原始颜色并设置为红色
                originalColor = light.color;
                light.color = Color.red;
            }
        }

        while (true)
        {
            foreach (Light light in lights)
            {
                if (light.type != LightType.Directional)
                {
                    // 随机决定灯是否闪烁
                    light.enabled = Random.Range(0, 2) > 0;
                }
            }
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "FPSController")
        {
            /*StopAllCoroutines();
            foreach (Light light in lights)
            {
                if (light.type != LightType.Directional)
                {
                    // 恢复原始颜色和状态
                    light.color = originalColor;
                    light.enabled = true;
                }
            }*/
        }
    }
}
