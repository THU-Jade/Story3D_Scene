using UnityEngine;

public class MaterialFlicker : MonoBehaviour
{
    public GameObject targetObject; // 公开变量，用于指定目标模型
    public bool startFlicker = false; // 控制闪烁的布尔变量
    public float maxIntensity = 1.0f; // 闪烁的最大强度
    public float minIntensity = 0; // 闪烁的最小强度
    public float flickerSpeed = 20.0f; // 闪烁速度
    public AudioClip flickerSound; // 闪烁时播放的音效
    private Material flickerMaterial;
    private AudioSource audioSource;
    private float flickerIntensity;
    private float targetIntensity; // 目标强度，用于创建随机闪烁效果

    void Start()
    {
        if (targetObject != null)
        {
            Renderer renderer = targetObject.GetComponent<Renderer>();
            if (renderer != null && renderer.materials.Length > 1)
            {
                flickerMaterial = renderer.materials[1]; // 获取第二个材质
                flickerIntensity = minIntensity; // 初始化闪烁强度
                targetIntensity = Random.Range(minIntensity, maxIntensity); // 初始化目标强度
            }
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = flickerSound;
        audioSource.loop = true; // 设置音效循环播放
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
            // 调整闪烁强度，使其逐渐接近目标强度
            flickerIntensity = Mathf.MoveTowards(flickerIntensity, targetIntensity, flickerSpeed * Time.deltaTime);

            // 如果达到目标强度，则选择新的随机目标强度
            if (Mathf.Approximately(flickerIntensity, targetIntensity))
            {
                targetIntensity = Random.Range(minIntensity, maxIntensity);
            }

            flickerMaterial.SetColor("_EmissionColor", new Color(flickerIntensity, flickerIntensity, flickerIntensity, 1));
        }
    }
}