using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_OC : MonoBehaviour
{
    public float openAngle = 90.0f;
    public float openSpeed = 2.0f;
    public bool ifBack = false;
    public GameObject Door;
    public AudioClip soundEffect;
    public AudioSource audioSource;

    private bool isOpening = false;
    private float currentAngle = 0.0f;

    void Start()
    {
        // 确保音频源组件已经被指定
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    void Update()
    {
        if (isOpening)
        {
            if (currentAngle < openAngle)
            {
                RotateDoor(openSpeed * Time.deltaTime);
            }
        }
        else
        {
            if (currentAngle > 0)
            {
                RotateDoor(-openSpeed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "FPSController")
        {
            isOpening = true;
        }
       // 如果指定了音效并且音频源组件存在
            if (soundEffect != null && audioSource != null)
        {
            audioSource.PlayOneShot(soundEffect); // 播放音效
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "FPSController")
        {
            isOpening = false;
        }
    }

    private void RotateDoor(float angleToRotate)
    {
        float rotationAngle = ifBack ? -angleToRotate : angleToRotate;
        Door.transform.Rotate(0, rotationAngle, 0);
        currentAngle += Mathf.Abs(angleToRotate);

        // 确保currentAngle不超过openAngle，并且在关闭时归零
        if (isOpening)
        {
            currentAngle = Mathf.Min(currentAngle, openAngle);
        }
        else
        {
            currentAngle = Mathf.Max(currentAngle - Mathf.Abs(angleToRotate) * 2, 0);
        }
    }
}
