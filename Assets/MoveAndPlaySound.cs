using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAndPlaySound : MonoBehaviour
{
    public Transform objectToMove; // Ҫ�ƶ���GameObject
    public Vector3 moveDirection; // �ƶ�����
    public float moveDistance = 1f; // �ƶ�����
    public float moveDuration = 1f; // �ƶ�����ʱ��
    public AudioClip soundEffect; // Ҫ���ŵ���Ч
    private AudioSource audioSource; // ��ƵԴ���
    private bool DoOnce = true;
    void Awake()
    {
        // ��ȡ�������ƵԴ���
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "FPSController" && DoOnce)
        {
            // ������Ч
            if (soundEffect != null)
            {
                audioSource.PlayOneShot(soundEffect);
            }
            // ��ʼ�ƶ�
            StartCoroutine(MoveObject(objectToMove, moveDirection * moveDistance, moveDuration));
            DoOnce = false;
        }
    }

    private IEnumerator MoveObject(Transform objectToMove, Vector3 endPosition, float duration)
    {
        // ����ÿ���ƶ���
        Vector3 startPosition = objectToMove.position;
        Vector3 deltaPosition = endPosition / duration;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            objectToMove.position += deltaPosition * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }

        // ȷ������׼ȷ�ص�������λ��
        objectToMove.position = startPosition + endPosition;
    }
}
