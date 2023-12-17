using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAndPlaySound : MonoBehaviour
{
    public Transform objectToMove; // 要移动的GameObject
    public Vector3 moveDirection; // 移动方向
    public float moveDistance = 1f; // 移动距离
    public float moveDuration = 1f; // 移动持续时间
    public AudioClip soundEffect; // 要播放的音效
    private AudioSource audioSource; // 音频源组件
    private bool DoOnce = true;
    void Awake()
    {
        // 获取或添加音频源组件
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
            // 播放音效
            if (soundEffect != null)
            {
                audioSource.PlayOneShot(soundEffect);
            }
            // 开始移动
            StartCoroutine(MoveObject(objectToMove, moveDirection * moveDistance, moveDuration));
            DoOnce = false;
        }
    }

    private IEnumerator MoveObject(Transform objectToMove, Vector3 endPosition, float duration)
    {
        // 计算每秒移动量
        Vector3 startPosition = objectToMove.position;
        Vector3 deltaPosition = endPosition / duration;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            objectToMove.position += deltaPosition * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 确保对象准确地到达最终位置
        objectToMove.position = startPosition + endPosition;
    }
}
