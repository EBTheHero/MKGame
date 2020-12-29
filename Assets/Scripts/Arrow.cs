using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Arrow : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject goal;
    public Transform arrow;
    public float maxDistance = 100;
    public float resetDistance;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (goal != null)
        {
            PositionArrow();
            Vector3 vector = Camera.main.transform.position;
            vector.z = 0;
            if (Vector3.Distance(vector, goal.transform.position) < resetDistance)
            {
                goal = null;
                arrow.gameObject.SetActive(false);
            }
                
        }

    }

    void PositionArrow()
    {


        Camera cam = Camera.main;

        //Get the targets position on screen into a Vector3
        Vector3 targetPos = cam.WorldToScreenPoint(goal.transform.position);
        //Get the middle of the screen into a Vector3
        Vector3 screenMiddle = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        //Compute the angle from screenMiddle to targetPos
        var tarAngle = (Mathf.Atan2(targetPos.x - screenMiddle.x, Screen.height - targetPos.y - screenMiddle.y) * Mathf.Rad2Deg) + 90;
        if (tarAngle < 0) tarAngle += 360;


        //Calculate the angle from the camera to the target
        var targetDir = goal.transform.position - cam.transform.position;
        float distance = Vector3.Distance(targetPos, screenMiddle);
        var forward = cam.transform.forward;
        var angle = Vector3.Angle(targetDir, forward);
        //If the angle exceeds 90deg inverse the rotation to point correctly
        if (angle > 90){
            arrow.localRotation = Quaternion.Euler(0, 0, -tarAngle);
            if (distance > maxDistance)
                arrow.localPosition = DegreeToVector2(-tarAngle) * maxDistance;
            else
            {
                arrow.localPosition = targetPos;
            }

        } else
        {
            arrow.localRotation = Quaternion.Euler(0, 0, tarAngle);

            if (distance > maxDistance)
                arrow.localPosition = DegreeToVector2(tarAngle) * maxDistance * -1;
            else
            {
                arrow.localPosition = targetPos - screenMiddle;
            }
                
        }


    }

    [YarnCommand("setgoal")]
    public void SetGoal(string sgoal)
    {
        GameObject go = GameObject.Find(sgoal);
        if (go != null)
        {
            goal = go;
            arrow.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Set goal called, but couldn't find " + sgoal);
        }
    }

    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

}
