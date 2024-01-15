using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone : MonoBehaviour
{
    public bool startRotation = false; // 公开的bool变量，用于控制旋转
    public float rotationSpeed = 200.0f; // 旋转速度
    public float zAngleRange = 10.0f;
    public AudioClip rotationSound; // 旋转时播放的音效
    public GameObject model;

    private AudioSource audioSource;
    private float zAngle = 0; // 当前Z轴角度
    private bool rotatingRight = true; // 旋转方向

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // 如果未找到音频源组件，则添加一个
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
