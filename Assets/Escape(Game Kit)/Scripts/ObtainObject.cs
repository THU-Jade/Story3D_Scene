using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtainObject : MonoBehaviour
{
    public Transform laserOrigin;
    public float maxRaycastDistance = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray laserRay = new Ray(laserOrigin.position, laserOrigin.forward);
        if (Physics.Raycast(laserRay, out RaycastHit hit, maxRaycastDistance)) {
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.CompareTag("Collectable") && OVRInput.GetDown(OVRInput.Button.One)) {
                hitObject.SetActive(false);
            }
        }
    }
}
