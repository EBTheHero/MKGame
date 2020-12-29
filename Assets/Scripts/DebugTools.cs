using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Yarn.Unity;

public class DebugTools : MonoBehaviour
{
    public GameObject debugCanvas;
    public TMP_InputField debugInputField;

    DialogueRunner dialogueRunner;
    // Start is called before the first frame update
    void Start()
    {
        debugCanvas.SetActive(false);
        dialogueRunner = FindObjectOfType<DialogueRunner>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            debugCanvas.SetActive(!debugCanvas.activeSelf);

    }

    public void DialogueJump()
    {
        string node = debugInputField.text;

        dialogueRunner.StartDialogue(node);
    }
}
