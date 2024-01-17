using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notification : MonoBehaviour
{
    GameObject notificationCanvas;
    OVRCameraRig ovr;
    Transform _centerEyeAnchor;
    private bool canvasActive = false;
    private float textTime;

    // Start is called before the first frame update
    void Start()
    {
        notificationCanvas = GameObject.Find("NotificationCanvas");
        notificationCanvas.SetActive(false);
        ovr = FindObjectOfType<OVRCameraRig>();
        _centerEyeAnchor = ovr.centerEyeAnchor;
    }

    // Update is called once per frame
    void Update()
    {
        notificationCanvas.transform.position = _centerEyeAnchor.transform.position + _centerEyeAnchor.transform.forward;
        notificationCanvas.transform.rotation = _centerEyeAnchor.transform.rotation;

        if (canvasActive)
        {
            textTime += Time.deltaTime;
            if(textTime >= 5.0f)
            {
                notificationCanvas.SetActive(false);
                textTime = 0;
                canvasActive = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "FPSController")
        {
            canvasActive = true;
            notificationCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
       
    }
}
