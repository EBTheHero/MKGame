using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MasterStamp : MonoBehaviour
{
    public GameObject canvasFailure;
    public GameObject canvasStart;
    public HandController handController;
    public EventSystem eventSystem;

    public Material transition;
    bool toBlack = false;
    bool success = false;
    // Start is called before the first frame update
    void Start()
    {
        //Invoke("CollisionGenerator", 0.2f);

        canvasFailure.SetActive(false);
    }

    private void CollisionGenerator()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("PaintItem");
        foreach (GameObject item in items)
        {
            item.AddComponent<PolygonCollider2D>();
        }

        transition.SetFloat("_Cutoff", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && canvasStart.activeSelf)
        {
            canvasStart.SetActive(false);
            handController.ResetHand();
            handController.canMove = true;
        }
            

    }

    private void FixedUpdate()
    {
        if (transition.GetFloat("_Cutoff") >= 0 && !toBlack)
        {

            transition.SetFloat("_Cutoff", transition.GetFloat("_Cutoff") - 0.03f);
        }

        if (transition.GetFloat("_Cutoff") <= 1 && toBlack)
            transition.SetFloat("_Cutoff", transition.GetFloat("_Cutoff") + 0.03f);
        if (transition.GetFloat("_Cutoff") >= 1 && toBlack)
        {
            GameObject.Find("Master").GetComponent<MasterScript>().MinigameDone(success);
        }
    }

    public void ResetLevel()
    {
        canvasFailure.SetActive(false);
        handController.ResetHand();
        handController.canMove = true;
    }

    public void StampSuccess()
    {
        Debug.Log("Stamp get");
        success = true;
        toBlack = true;
    }

    public void StampFailed()
    {
        Debug.Log("Item hit");
        canvasFailure.SetActive(true);
        handController.canMove = false;
        eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
    }

    public void ExitMinigame()
    {
        toBlack = true;
    }
}
