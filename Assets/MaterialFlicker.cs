using UnityEngine;

public class MaterialFlicker : MonoBehaviour
{
    public GameObject targetObject; // ��������������ָ��Ŀ��ģ��
    public bool startFlicker = false; // ������˸�Ĳ�������
    public float maxIntensity = 1.0f; // ��˸�����ǿ��
    public float minIntensity = 0; // ��˸����Сǿ��
    public float flickerSpeed = 20.0f; // ��˸�ٶ�
    public AudioClip flickerSound; // ��˸ʱ���ŵ���Ч
    private Material flickerMaterial;
    private AudioSource audioSource;
    private float flickerIntensity;
    private float targetIntensity; // Ŀ��ǿ�ȣ����ڴ��������˸Ч��

    void Start()
    {
        if (targetObject != null)
        {
            Renderer renderer = targetObject.GetComponent<Renderer>();
            if (renderer != null && renderer.materials.Length > 1)
            {
                flickerMaterial = renderer.materials[1]; // ��ȡ�ڶ�������
                flickerIntensity = minIntensity; // ��ʼ����˸ǿ��
                targetIntensity = Random.Range(minIntensity, maxIntensity); // ��ʼ��Ŀ��ǿ��
            }
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = flickerSound;
        audioSource.loop = true; // ������Чѭ������
    }

    void Update()
    {
        if (startFlicker)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }

            FlickerMaterial();
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            if (flickerMaterial != null)
            {
                flickerMaterial.SetColor("_EmissionColor", new Color(minIntensity, minIntensity, minIntensity, 1));
            }
        }
    }

    private void FlickerMaterial()
    {
        if (flickerMaterial != null)
        {
            // ������˸ǿ�ȣ�ʹ���𽥽ӽ�Ŀ��ǿ��
            flickerIntensity = Mathf.MoveTowards(flickerIntensity, targetIntensity, flickerSpeed * Time.deltaTime);

            // ����ﵽĿ��ǿ�ȣ���ѡ���µ����Ŀ��ǿ��
            if (Mathf.Approximately(flickerIntensity, targetIntensity))
            {
                targetIntensity = Random.Range(minIntensity, maxIntensity);
            }

            flickerMaterial.SetColor("_EmissionColor", new Color(flickerIntensity, flickerIntensity, flickerIntensity, 1));
        }
    }
}