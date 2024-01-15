using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public Light targetLight; // �����ĵƹ����
    public float minIntensity = 0f; // ��С����
    public float maxIntensity = 3f; // �������
    public float flickerSpeed = 0.1f; // ��˸�ٶ�
    public bool Play = false;

    private float originalIntensity; // ԭʼ����
    private Coroutine flickerCoroutine; // ��˸Э�̵�����

    void Start()
    {
        if (targetLight == null)
        {
            targetLight = GetComponent<Light>();
        }
        originalIntensity = targetLight.intensity; // ����ԭʼ����
    }

    void Update()
    {
        if (Play)
        {
            if (flickerCoroutine != null)
            {
                StopCoroutine(flickerCoroutine);
            }
            flickerCoroutine = StartCoroutine(Flicker());
        }
        else
        {
            if (flickerCoroutine != null)
            {
                StopCoroutine(flickerCoroutine);
                targetLight.intensity = originalIntensity; // �ָ�ԭʼ����
            }
        }
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
                targetLight.intensity = originalIntensity; // �ָ�ԭʼ����
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