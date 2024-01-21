using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone : MonoBehaviour
{
    public bool startRotation = false; // ������bool���������ڿ�����ת
    public float rotationSpeed = 200.0f; // ��ת�ٶ�
    public float zAngleRange = 10.0f;
    public AudioClip rotationSound; // ��תʱ���ŵ���Ч
    public GameObject model;

    private AudioSource audioSource;
    private float zAngle = 0; // ��ǰZ��Ƕ�
    private bool rotatingRight = true; // ��ת����

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // ���δ�ҵ���ƵԴ����������һ��
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = rotationSound;
    }

    void Update()
    {
        if (startRotation)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }

            RotateObjectFunction();
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    private void RotateObjectFunction()
    {
        if (rotatingRight)
        {
            zAngle += rotationSpeed * Time.deltaTime;
            if (zAngle > zAngleRange)
            {
                zAngle = zAngleRange;
                rotatingRight = false;
            }
        }
        else
        {
            zAngle -= rotationSpeed * Time.deltaTime;
            if (zAngle < -zAngleRange)
            {
                zAngle = -zAngleRange;
                rotatingRight = true;
            }
        }

        model.transform.rotation = Quaternion.Euler(0, 0, zAngle);
    }
}
