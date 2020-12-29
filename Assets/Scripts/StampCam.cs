using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampCam : MonoBehaviour
{
    public GameObject target;
    public float minimum = 3;
    public float maximum = 20;
    public float offset = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float posY = target.transform.position.y + offset;
        posY = Mathf.Clamp(posY, minimum, maximum);

        transform.position = new Vector3(0, posY, -10);
    }
}
