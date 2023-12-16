using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightFlickerTrigger : MonoBehaviour
{
    public float minFlickerSpeed = 0.1f;
    public float maxFlickerSpeed = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "FPSController")
        {
            foreach (GameObject lightbeamFog in GameObject.FindGameObjectsWithTag("LightFog"))
            {
                lightbeamFog.SetActive(false);
            }

            // �����ƹ���˸Э��
            foreach (Light light in FindObjectsOfType<Light>())
            {
                if (light.type != LightType.Directional)
                {
                    StartCoroutine(FlickerLight(light));
                }
            }
            // �����Է��������˸Э��
            foreach (Renderer renderer in FindObjectsOfType<Renderer>())
            {
                Material material = renderer.material;
                if (material != null && material.HasProperty("_EmissionColor"))
                {
                    StartCoroutine(FlickerMaterialEmissionColor(material));
                }
            }
        }
    }

    private IEnumerator FlickerLight(Light light)
    {
        while (light != null)
        {
            light.enabled = !light.enabled;
            yield return new WaitForSeconds(Random.Range(minFlickerSpeed, maxFlickerSpeed));
        }
    }

    private IEnumerator FlickerMaterialEmissionColor(Material material)
    {
        Color originalColor = material.GetColor("_EmissionColor");
        while (true)
        {
            material.SetColor("_EmissionColor", Random.value > 0.5f ? originalColor : Color.black);
            yield return new WaitForSeconds(Random.Range(minFlickerSpeed, maxFlickerSpeed));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "FPSController")
        {
            foreach (GameObject lightbeamFog in GameObject.FindGameObjectsWithTag("LightFog"))
            {
                lightbeamFog.SetActive(true);
            }
            StopAllCoroutines();
            // �ر����еƹ�
            foreach (Light light in FindObjectsOfType<Light>())
            {
                if (light.type != LightType.Directional)
                {
                    light.enabled = true;
                }
            }
            // �ָ����в��ʵ��Է���
            foreach (Renderer renderer in FindObjectsOfType<Renderer>())
            {
                Material material = renderer.material;
                if (material != null && material.HasProperty("_EmissionColor"))
                {
                    material.SetColor("_EmissionColor", material.GetColor("_EmissionColor"));
                }
            }
        }
    }
}


