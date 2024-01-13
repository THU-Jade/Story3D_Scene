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

    public GameObject CanvasText;
    private bool canvasActive = false;
    private float textTime;

    private bool isOpening = false;
    private float currentAngle = 0.0f;

    void Start()
    {
        // ȷ����ƵԴ����Ѿ���ָ��
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
        if (canvasActive)
        {
            textTime += Time.deltaTime;
            print(textTime);
            if(textTime >= 5.0f)
            {
                CanvasText.SetActive(false);
                textTime = 0;
                canvasActive = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "FPSController")
        {
            isOpening = true;
            CanvasText.SetActive(true);
            canvasActive = true;
        }
       // ���ָ������Ч������ƵԴ�������
            if (soundEffect != null && audioSource != null)
        {
            audioSource.PlayOneShot(soundEffect); // ������Ч
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

        // ȷ��currentAngle������openAngle�������ڹر�ʱ����
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
