using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    float Rotation;
    public float turnSpeed = 1;
    public float baseSpeed = 0.05f;
    public float acceleration;

    public bool canMove = false;

    Vector3 startPosition;

    Rigidbody2D rb2D;
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (canMove)
        {


            Rotation = Input.GetAxis("Horizontal") * turnSpeed;

            //rb2D.AddTorque(Rotation);
            //if (rb2D.velocity.magnitude < speed)
            //    rb2D.AddRelativeForce(new Vector2(0, acceleration));
            transform.Rotate(new Vector3(0, 0, Rotation));

            float speed = baseSpeed * (1 + Mathf.Abs(Input.GetAxis("Triggers") * 3));
            transform.Translate(Vector3.up * speed, Space.Self);
            //rb2D.MovePosition(transform.position + transform.up * speed);
        }
    }

    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    public void ResetHand()
    {
        transform.position = startPosition;
        Vector3[] list =
        {
            new Vector3(0,-8,-1),
            new Vector3(0,-8,-1)
        };
        GameObject.Find("ArmRenderer").GetComponent<LineRenderer>().positionCount = 2;
        GameObject.Find("ArmRenderer").GetComponent<LineRenderer>().SetPositions(list);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PaintItem")
        {

            FindObjectOfType<MasterStamp>().StampFailed();
        }
        else if (collision.gameObject.name == "stamp")
            FindObjectOfType<MasterStamp>().StampSuccess();
        else
        {
            Vector2 angle = transform.up;
            Vector2 refl = Vector2.Reflect(angle, collision.contacts[0].normal);
            float angle2 = Vector2.Angle(refl, Vector2.zero);
            transform.Translate(Vector3.up, Space.Self);
            transform.up = refl;
        }
    }
}
