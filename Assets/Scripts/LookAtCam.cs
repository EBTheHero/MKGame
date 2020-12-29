using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCam : MonoBehaviour
{
    GameObject cam;
    public Vector3 up = new Vector3();
    void Start()
    {
        cam = GameObject.Find("ShowCam");
    }

    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(cam.transform, up);

        //Vector3 rot = transform.rotation.eulerAngles;
        //rot.x = 0;
        //transform.rotation = Quaternion.Euler(rot);
        //transform.Rotate(new Vector3(0,1,0), 180);

        transform.rotation = Quaternion.Euler(0, cam.transform.rotation.eulerAngles.y, 0);

    }
}
