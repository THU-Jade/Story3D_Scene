using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerForce : MonoBehaviour
{
    public Rigidbody targetRigidbody; // 目标刚体
    public Vector3 force; // 施加的力
    public ForceMode forceMode = ForceMode.Force; // 施加力的方式

    private void OnTriggerEnter(Collider other)
    {
        // 检查是否是名为"FPSController"的角色与Box Collider发生重叠
        if (other.gameObject.name == "FPSController")
        {
            // 给目标刚体施加力
            targetRigidbody.AddForce(force, forceMode);
        }
    }
}
