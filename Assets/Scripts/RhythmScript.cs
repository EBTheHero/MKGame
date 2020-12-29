using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RhythmScript : MonoBehaviour
{
    public Image hitSprite;
    public enum Buttons { up, down, left, right }
    bool pressed = false;
    float diff = 0;
    string xmlDir = @"C:\Users\etien\Documents\_MYSTUFF\Unity\MKGamedir\MK\Assets\Resources\notes.xml";

    public float Score = 0;
    public bool saveNotes = true;

    Conductor conductor;

    List<Note> notes = new List<Note>();
    AudioSource audioSource;

    public GameObject visualNote;
    public Transform parentVisualNote;
    public float spawnBeats;
    public GameObject hitFeedback;

    public float PerfectNote = 0.1f;
    public Color PerfectColor;
    public float GreatNote = 0.2f;
    public Color GreatColor;
    public float OkNote = 0.3f;
    public Color OkColor;
    public Color OofColor;
    GameObject[] visualNotes = new GameObject[500];

    Color startingColor;

    public GameObject ScoreboardCanvas;
    public GameObject TutorialCanvas;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI LiveScoreText;
    public bool gameStarted = false;

    public Material pokemonTransition;

    public EventSystem eventSystem;

    bool toBlack = false;
    // Start is called before the first frame update
    void Start()
    {
        ScoreboardCanvas.SetActive(false);
        
        conductor = GetComponent<Conductor>();
        audioSource = GetComponent<AudioSource>();
        startingColor = hitSprite.color;
        readXML();
    }

    public void startGame()
    {
        foreach (GameObject item in visualNotes)
        {
            if (item)
                Destroy(item);
        }
        Score = 0;
        gameStarted = true; 
        TutorialCanvas.SetActive(false);
        ScoreboardCanvas.SetActive(false);
        visualNotes = new GameObject[500];
        conductor.startConductor();
        foreach (Note note in notes)
        {
            GameObject vNote = Instantiate(visualNote, parentVisualNote);
            VisualNote vscript = vNote.GetComponent<VisualNote>();
            vscript.note = note;
            vscript.conductor = conductor;
            visualNotes[(int)note.beat] = vNote;
        }



    }

    // Update is called once per frame
    void Update()
    {
        if (!gameStarted && Input.GetButtonDown("Fire1") && TutorialCanvas.activeSelf)
            startGame();
        //positive diff = late
        //negative diff = early
        diff = (conductor.songPositionInBeats + 0.5f) % 1 - 0.5f;

        if (Mathf.Abs(diff) < 0.05)
            hitSprite.color = Color.red;
        else
            hitSprite.color = startingColor;

        float xAxis = Input.GetAxisRaw("DPadX") + Input.GetAxisRaw("Horizontal");
        float yAxis = Input.GetAxisRaw("DPadY") + Input.GetAxisRaw("Vertical");
        if (!pressed)
        {
            if (xAxis == 1)
                buttonPressed(Buttons.right, diff);
            if (xAxis == -1)
                buttonPressed(Buttons.left, diff);
            if (yAxis == 1)
                buttonPressed(Buttons.up, diff);
            if (yAxis == -1)
                buttonPressed(Buttons.down, diff);
        }

        if (xAxis == 0 && yAxis == 0)
            pressed = false;


    }

    private void FixedUpdate()
    {


        if (pokemonTransition.GetFloat("_Cutoff") >= 0 && !toBlack)
        {

            pokemonTransition.SetFloat("_Cutoff", pokemonTransition.GetFloat("_Cutoff") - 0.03f);
        }

        if (pokemonTransition.GetFloat("_Cutoff") <= 1 && toBlack)
            pokemonTransition.SetFloat("_Cutoff", pokemonTransition.GetFloat("_Cutoff") + 0.03f);
        if (pokemonTransition.GetFloat("_Cutoff") >= 1 && toBlack)
        {
            GameObject.Find("Master").GetComponent<MasterScript>().MinigameDone(true);
        }
    }

    public void buttonPressed(Buttons button, float diff)
    {
        pressed = true;
        float beat = Mathf.Round(conductor.songPositionInBeats);
        //Debug.Log(button.ToString() + " pressed at " + diff.ToString("F2") + " different from beat " + beat);

        bool noteHit = (int)beat >= 1 && visualNotes[(int)beat] != null;
        if (noteHit)
        {
            VisualNote vNote = visualNotes[(int)beat].GetComponent<VisualNote>();
            if (button == vNote.note.direction && !vNote.hit)
            {
                vNote.getHit();
                if (Mathf.Abs(diff) < PerfectNote)
                {
                    Score += 100;
                    spawnFeedback("PERFECT!", PerfectColor);
                } else if (Mathf.Abs(diff) < GreatNote)
                {
                    Score += 75;
                    spawnFeedback("GREAT!", GreatColor);
                } else if (Mathf.Abs(diff) < OkNote)
                {
                    Score += 45;
                    spawnFeedback("OK", OkColor);
                } else
                {
                    Score += 15;
                    spawnFeedback("OOF", OofColor);
                }
                LiveScoreText.text = Score.ToString();
            }
            else
            {
                //Wrong direction!
            }
        }

        if (saveNotes && !noteHit)
        {
            Note newNote = new Note(Mathf.Round(conductor.songPositionInBeats), button);
            notes.Add(newNote);
        }
    }

    public void gameOver()
    {
        gameStarted = false;
        ScoreboardCanvas.SetActive(true);
        ScoreText.text = Score.ToString() + " points!";
        fixSelect();
    }

    public void exitGame()
    {
        toBlack = true;
    }

    public void spawnFeedback(string text, Color color)
    {
        GameObject gameObject = Instantiate(hitFeedback, parentVisualNote);
        TextMeshProUGUI textMesh = gameObject.GetComponent<TextMeshProUGUI>();
        textMesh.text = text;
        textMesh.color = color;
    }

    public void saveToXML()
    {
        XElement xmlElements = new XElement("Branches", notes.Select(i => new XElement("note", new XAttribute("beat", i.beat), new XAttribute("direction", i.direction))));

        xmlElements.Save(xmlDir);
    }

    public void readXML()
    {
        notes.Clear();
        TextAsset textFile = Resources.Load<TextAsset>("notes"); 
        
        XDocument doc = XDocument.Parse(textFile.text);
        //XDocument doc = XDocument.Load(xmlDir);
        foreach (var item in doc.Root.Elements())
        {
            Note newNote = new Note();
            newNote.beat = float.Parse(item.FirstAttribute.Value);
            newNote.direction = (Buttons)Enum.Parse(typeof(Buttons), item.LastAttribute.Value);
            notes.Add(newNote);
        }
    }

    public void fixSelect()
    {
        eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
    }
}
