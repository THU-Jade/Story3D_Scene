using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoosePath : MonoBehaviour
{
    GameObject choiceCanvas;
    OVRCameraRig ovr;
    Transform _centerEyeAnchor;

    public void ClickButton1() {
        Debug.Log("Button 1 clicked!");
        choiceCanvas.SetActive(false);

    }

    public void ClickButton2() {
        Debug.Log("Button 2 clicked!");
        choiceCanvas.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        choiceCanvas = GameObject.Find("ChoiceCanvas");
        choiceCanvas.SetActive(false);
        ovr = FindObjectOfType<OVRCameraRig>();
        _centerEyeAnchor = ovr.centerEyeAnchor;
    }

    // Update is called once per frame
    void Update()
    {
        choiceCanvas.transform.position = _centerEyeAnchor.transform.position + _centerEyeAnchor.transform.forward;
        choiceCanvas.transform.rotation = _centerEyeAnchor.transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "FPSController")
        {
            Debug.Log("triggered");
            choiceCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
       
    }
}
