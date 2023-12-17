using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorrorLightingTrigger : MonoBehaviour
{
    public AudioClip soundEffect; // Ҫ���ŵ���Ч
    private AudioSource audioSource; // ��ƵԴ���
    private Light[] lights; // ���������еĵƹ�
    private Color originalColor; // ���ڴ洢ԭʼ��ɫ

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
                // �洢ԭʼ��ɫ������Ϊ��ɫ
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
                    // ����������Ƿ���˸
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
                    // �ָ�ԭʼ��ɫ��״̬
                    light.color = originalColor;
                    light.enabled = true;
                }
            }*/
        }
    }
}
