using UnityEngine;

public class ContinuousRotation : MonoBehaviour
{
    public GameObject[] targetObjects; // 用于指定需要旋转的模型数组
    public bool startRotation = false; // 控制旋转的布尔变量
    public float rotationSpeed = 100.0f; // 旋转速度
    public AudioClip rotationSound; // 旋转时播放的音效

    private AudioSource audioSource;

    void Start()
    {
        // 检查是否指定了目标对象
        if (targetObjects == null || targetObjects.Length == 0)
        {
            Debug.LogError("No target objects for rotation are assigned!");
            return;
        }
        // 获取或添加音频源组件
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = rotationSound;
        audioSource.loop = true; // 设置音效循环播放
    }

    void Update()
    {
        if (startRotation)
        {
            // 对每个目标对象应用旋转
            foreach (GameObject obj in targetObjects)
            {
                if (obj != null)
                {
                    obj.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
                }
            }

            // 播放音效
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            // 停止播放音效
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}
