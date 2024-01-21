using UnityEngine;

public class ContinuousRotation : MonoBehaviour
{
    public GameObject[] targetObjects; // ����ָ����Ҫ��ת��ģ������
    public bool startRotation = false; // ������ת�Ĳ�������
    public float rotationSpeed = 100.0f; // ��ת�ٶ�
    public AudioClip rotationSound; // ��תʱ���ŵ���Ч

    private AudioSource audioSource;

    void Start()
    {
        // ����Ƿ�ָ����Ŀ�����
        if (targetObjects == null || targetObjects.Length == 0)
        {
            Debug.LogError("No target objects for rotation are assigned!");
            return;
        }
        // ��ȡ�������ƵԴ���
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = rotationSound;
        audioSource.loop = true; // ������Чѭ������
    }

    void Update()
    {
        if (startRotation)
        {
            // ��ÿ��Ŀ�����Ӧ����ת
            foreach (GameObject obj in targetObjects)
            {
                if (obj != null)
                {
                    obj.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
                }
            }

            // ������Ч
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            // ֹͣ������Ч
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}
