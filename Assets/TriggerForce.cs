using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerForce : MonoBehaviour
{
    public Rigidbody targetRigidbody; // 目标刚体
    public Vector3 force; // 施加的力
    public ForceMode forceMode = ForceMode.Force; // 施加力的方式
    public AudioClip soundEffect; // 要播放的音效
    public AudioSource audioSource; // 音频源组件
    public bool Play = false; 
    private bool DoOnce = true;

    private void Start()
    {
        // 确保音频源组件已经被指定
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (Play && DoOnce)
        {
            // 如果指定了音效并且音频源组件存在
            if (soundEffect != null && audioSource != null)
            {
                audioSource.PlayOneShot(soundEffect); // 播放音效
            }

            // 给目标刚体施加力
            targetRigidbody.AddForce(force, forceMode);
            DoOnce = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 检查是否是名为"FPSController"的角色与Box Collider发生重叠
        if (other.gameObject.name == "FPSController" && DoOnce)
        {
            // 如果指定了音效并且音频源组件存在
            if (soundEffect != null && audioSource != null)
            {
                audioSource.PlayOneShot(soundEffect); // 播放音效
            }

            // 给目标刚体施加力
            targetRigidbody.AddForce(force, forceMode);
            DoOnce = false;
        }
    }
}
