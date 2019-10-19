using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSpinner : MonoBehaviour
{

    public float swingSpeed = 120f;
    // Update is called once per frame
    void Update()
    {
        float nexty = transform.eulerAngles.y + (swingSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, nexty, transform.eulerAngles.z);
    }
}
