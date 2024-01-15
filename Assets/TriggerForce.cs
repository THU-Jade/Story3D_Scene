using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerForce : MonoBehaviour
{
    public Rigidbody targetRigidbody; // Ŀ�����
    public Vector3 force; // ʩ�ӵ���
    public ForceMode forceMode = ForceMode.Force; // ʩ�����ķ�ʽ
    public AudioClip soundEffect; // Ҫ���ŵ���Ч
    public AudioSource audioSource; // ��ƵԴ���
    public bool Play = false; 
    private bool DoOnce = true;

    private void Start()
    {
        // ȷ����ƵԴ����Ѿ���ָ��
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (Play && DoOnce)
        {
            // ���ָ������Ч������ƵԴ�������
            if (soundEffect != null && audioSource != null)
            {
                audioSource.PlayOneShot(soundEffect); // ������Ч
            }

            // ��Ŀ�����ʩ����
            targetRigidbody.AddForce(force, forceMode);
            DoOnce = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // ����Ƿ�����Ϊ"FPSController"�Ľ�ɫ��Box Collider�����ص�
        if (other.gameObject.name == "FPSController" && DoOnce)
        {
            // ���ָ������Ч������ƵԴ�������
            if (soundEffect != null && audioSource != null)
            {
                audioSource.PlayOneShot(soundEffect); // ������Ч
            }

            // ��Ŀ�����ʩ����
            targetRigidbody.AddForce(force, forceMode);
            DoOnce = false;
        }
    }
}
