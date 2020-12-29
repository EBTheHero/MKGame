using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MasterSorter : MonoBehaviour
{
    public AnimationCurve speedCurve;

    float summonTimerLeft = 2f;
    float summonTimerRight = 0;
    public GameObject sortItem;



    public TextMeshProUGUI textMeshScore;
    public TextMeshProUGUI textMeshTimer;
    public float TimeLimit = 30f;
    float TimeElapsed = 0;

    public Vector2 spawn = new Vector2();

    public bool isStarted = false;

    int score = 0;

    public Material pokemonTransition;
    public Sprite newTransition;

    public bool toBlack = false;
    private void Start()
    {
        pokemonTransition.SetTexture("_TransitionTex", newTransition.texture);
        pokemonTransition.SetFloat("_Cutoff", 1f);
    }

    private void Update()
    {
        if (isStarted)
            TimeElapsed += Time.deltaTime;

        textMeshScore.text = score + " points";



        
        textMeshTimer.text = (TimeLimit - TimeElapsed).ToString("F1") + " seconds left";


        if (TimeLimit - TimeElapsed <= 0) {
            isStarted = false;
            toBlack = true;
        }

        if (Input.GetButtonDown("Fire1") && !isStarted)
        {
            isStarted = true;
            GameObject.Find("SortTutorial").SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (isStarted)
        {
            float summonSpeed = speedCurve.Evaluate(TimeElapsed / TimeLimit);

            summonTimerLeft += Time.fixedDeltaTime;
            summonTimerRight += Time.fixedDeltaTime;
            if (summonTimerLeft > summonSpeed)
            {
                SummonItem(0);
                summonTimerLeft = 0.2f;
            }

            if (summonTimerRight > summonSpeed)
            {
                SummonItem(1);
                summonTimerRight = 0;
            }
        }



        if (pokemonTransition.GetFloat("_Cutoff") >= 0 && !toBlack)
        {

            pokemonTransition.SetFloat("_Cutoff", pokemonTransition.GetFloat("_Cutoff") - 0.03f);
        }

        if (pokemonTransition.GetFloat("_Cutoff") <= 1 && toBlack)
            pokemonTransition.SetFloat("_Cutoff", pokemonTransition.GetFloat("_Cutoff") + 0.03f);
        if (pokemonTransition.GetFloat("_Cutoff") >= 1 && toBlack)
        {
            GameObject.Find("Master").GetComponent<MasterScript>().MinigameDone(score >= 25);
        }
    }

    void SummonItem(int oneIsRight)
    {
        SortItem.SortItemType purseOrMagazine = (SortItem.SortItemType)Random.Range(0, 2);

        Vector2 pos = spawn;

        if (oneIsRight == 0)
            pos.x = pos.x * -1;

        GameObject GO = Instantiate(sortItem, pos, Quaternion.identity);

        GO.GetComponent<SortItem>().setItem(purseOrMagazine);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(spawn, 0.1f);
    }

    public void AddScore(int i)
    {
        if (isStarted)
            score += i;
    }
}
