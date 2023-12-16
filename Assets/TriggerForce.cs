using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerForce : MonoBehaviour
{
    public Rigidbody targetRigidbody; // Ŀ�����
    public Vector3 force; // ʩ�ӵ���
    public ForceMode forceMode = ForceMode.Force; // ʩ�����ķ�ʽ

    private void OnTriggerEnter(Collider other)
    {
        // ����Ƿ�����Ϊ"FPSController"�Ľ�ɫ��Box Collider�����ص�
        if (other.gameObject.name == "FPSController")
        {
            // ��Ŀ�����ʩ����
            targetRigidbody.AddForce(force, forceMode);
        }
    }
}
