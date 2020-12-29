using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public string input = "Horizontal2";
    public float multiplier = -3;

    public float lerp = 0.1f;

    Rigidbody2D rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            float i = Input.GetAxis(input);

            rigidbody2D.MoveRotation(i * multiplier);

            //transform.rotation = Quaternion.Euler(0, 0, i * multiplier);
        }
        catch (System.Exception)
        {
        }
    }
}
