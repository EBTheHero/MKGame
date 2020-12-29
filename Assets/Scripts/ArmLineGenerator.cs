using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmLineGenerator : MonoBehaviour
{
    LineRenderer lineRenderer;
    GameObject hand;
    HandController handController;
    public float lineInterval = 1f;
    float handTimer;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        hand = GameObject.Find("Hand");
        handController = hand.GetComponent<HandController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (handController.canMove)
        {
            handTimer += Time.deltaTime;
            Vector3 vector3 = hand.transform.position + (Vector3)DegreeToVector2(hand.transform.rotation.eulerAngles.z + 90) / 8;

            if (handTimer >= lineInterval)
            {
                handTimer = 0;
                lineRenderer.positionCount += 1;
            }
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, vector3);
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
}
