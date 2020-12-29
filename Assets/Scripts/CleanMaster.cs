using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CleanMaster : MonoBehaviour
{
    public GameObject[] Litter;
    public Vector2 Size = new Vector2();

    public float spawnInterval = 1;
    public float spawnTimer;

    public float timeLimit = 30;

    bool isStarted = false;

    public bool DrawCube = true;

    public float Score = 0;

    bool toBlack = false;

    public Material pokemonTransition;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;

    public GameObject tutorialCanvas;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isStarted && Input.GetButtonDown("Fire1") && !toBlack)
        {
            tutorialCanvas.SetActive(false);
            isStarted = true;
            FindObjectOfType<CleanController>().canMove = true;
        }


        if (isStarted)
        {
            if (spawnTimer <= 0)
            {
                SpawnLitter();
                spawnTimer = spawnInterval;
            }
            spawnTimer -= Time.deltaTime;

            timeLimit -= Time.deltaTime;

            if (timeLimit < 0)
            {
                toBlack = true;
                isStarted = false;
            }
        }

        scoreText.text = Score + " points";
        timerText.text = timeLimit.ToString("F0") + " seconds";
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
            GameObject.Find("Master").GetComponent<MasterScript>().MinigameDone(Score >= 15);
        }
    }

    public void SpawnLitter()
    {
        Vector3 pos = new Vector3(Random.Range(-Size.x / 2, Size.x / 2), Random.Range(-Size.y / 2, Size.y / 2), 1);
        int r = Random.Range(0, Litter.Length);
        Instantiate(Litter[r], pos, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        if (DrawCube)
            Gizmos.DrawCube(Vector3.zero, Size);
    }

    public void LitterCleaned()
    {
        Score++;
    }
}
