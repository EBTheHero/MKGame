using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PokemonMasterScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject buttons;
    public TMPro.TextMeshProUGUI text = new TMPro.TextMeshProUGUI();
    public Animator stellaAnimator;
    public Animator burgerAnimator;
    public EventSystem eventSystem;

    public float StellaMaxHealth = 10;
    public float StellaHealth = 10;
    public float StellaBaseAttack = 2;
    public float StellaAttackBonus = 2;
    public float StellaDefense = 2;
    int nbOfBoxes = 0;


    public float BurgerMaxHealth = 20;
    public float BurgerHealth = 20;
    public float BurgerAttackBonus = 2;
    public float BurgerBaseAttack = 2;
    public float BurgerDefense = 2;
    public int BurgerStunned = 0;

    public Slider StellaSlider;
    public Image StellaHealthImage;

    public Slider BurgerSlider;
    public Image BurgerHealthImage;

    public Material pokemonTransition;
    public Sprite newTransition;

    public bool toBlack = false;

    bool BurgerDefeated = false;

    void Start()
    {
        StellaSlider.maxValue = StellaHealth;
        BurgerSlider.maxValue = BurgerMaxHealth;
        pokemonTransition.SetTexture("_TransitionTex", newTransition.texture);
        //inputUI.ActivateModule();
        pokemonTransition.SetFloat("_Cutoff", 1f);
        BurgerDone();
    }

    // Update is called once per frame
    void Update()
    {
        StellaSlider.value = Mathf.Lerp(StellaSlider.value, StellaHealth, 0.1f);
        float healthPercent = StellaHealth / StellaMaxHealth;
        if (healthPercent > 0.60f)
            StellaHealthImage.color = Color.green;
        else if (healthPercent > 0.20f)
            StellaHealthImage.color = Color.yellow;
        else
            StellaHealthImage.color = Color.red;

        healthPercent = BurgerHealth / BurgerMaxHealth;
        if (healthPercent > 0.60f)
            BurgerHealthImage.color = Color.green;
        else if (healthPercent > 0.20f)
            BurgerHealthImage.color = Color.yellow;
        else
            BurgerHealthImage.color = Color.red;

        BurgerSlider.value = Mathf.Lerp(BurgerSlider.value, BurgerHealth, 0.1f);
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
            GameObject.Find("Master").GetComponent<MasterScript>().MinigameDone(BurgerDefeated);
        }
    }

    public void StellaPunch()
    {
        buttons.SetActive(false);
        text.text = "Stella punches!";
        stellaAnimator.Play("StellaPunch");
        Invoke("StellaPunchResult", 2f);
    }

    void StellaPunchResult()
    {
        float Damage = DamageCalculator(StellaBaseAttack, StellaAttackBonus, BurgerDefense);
        if (BurgerStunned == 0)
            text.text = "It did " + Damage + " damage!";
        else
        {
            text.text = "It did " + Damage + " damage and snapped out the burger!";
            BurgerStunned = 0;
        }

        BurgerHealth -= Damage;
        Invoke("BurgerAttack", 2f);
    }

    private float DamageCalculator(float BaseAttack, float AttackBonus, float EnemyDefense)
    {
        return Mathf.Round(Random.Range(BaseAttack * 0.8f, BaseAttack * 1.2f) * (1f + 0.2f * AttackBonus - EnemyDefense * 0.2f));
    }

    public void StellaBox()
    {
        buttons.SetActive(false);
        nbOfBoxes++;
        if (nbOfBoxes == 1)
            text.text = "Stella puts a cardboard box on her head!";
        else
            text.text = "Stella puts a " + nbOfBoxes + GetOrdinalSuffix(nbOfBoxes) + " cardboard box on her head!";



        stellaAnimator.Play("BoxShield");
        Invoke("StellaBoxResult", 2f);
    }

    void StellaBoxResult()
    {
        text.text = "Stella's defense rose!";
        StellaDefense++;
        Invoke("BurgerAttack", 2f);
    }

    public void StellaRoast()
    {
        buttons.SetActive(false);
        stellaAnimator.Play("StellaRoast");

        text.text = "Stella's roasts the burger with some spicy insults!";
        Invoke("StellaRoastResult", 2f);
    }

    void StellaRoastResult()
    {
        text.text = "The burger's defense (and ego) fell!";
        BurgerDefense--;
        Invoke("BurgerAttack", 2f);
    }

    public void StellaScream()
    {
        buttons.SetActive(false);
        stellaAnimator.Play("StellaScream");
        burgerAnimator.Play("BurgerStunned");
        text.text = "Stella's screams as loud as she can!";
        Invoke("StellaScreamResult", 2f);
    }

    void StellaScreamResult()
    {
        text.text = "The burger is stunned from the scream!";
        BurgerStunned = 2;
        Invoke("BurgerAttack", 2f);
    }

    void BurgerDone()
    {
        if (StellaHealth <= 0)
        {
            text.text = "Stella fainted!";
            stellaAnimator.Play("StellaDeath");
            Invoke("FadeToBlack", 2f);
        } else
        {
            text.text = "";
            buttons.SetActive(true);
            eventSystem.SetSelectedGameObject(null);
            eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
        }

    }

    //Or StellaDone
    void BurgerAttack()
    {
        if (BurgerHealth <= 0)
        {
            text.text = "The Burger fainted!";
            BurgerDefeated = true;
            burgerAnimator.Play("BurgerDeath");
            Invoke("FadeToBlack", 2f);
        }
        else
        {
            if (BurgerStunned >= 1)
                BurgerStunAction();
            else if (StellaDefense >= 0 && Random.Range(0, 3) == 0) //if stella's defense is above 0, it has 1/3 chance to spit
                BurgerSpit();
            else
                BurgerBite();
        }

    }

    void BurgerStunAction()
    {
        text.text = "The burger is stunned and can't do shit!";
        burgerAnimator.Play("BurgerStunned");
        BurgerStunned--;
        Invoke("BurgerDone", 2f);
    }

    void BurgerBite()
    {
        text.text = "The burger bites!";
        burgerAnimator.Play("BurgerBite");
        Invoke("BurgerBiteResult", 2f);
    }

    void BurgerBiteResult()
    {
        float damage = DamageCalculator(BurgerBaseAttack, BurgerAttackBonus, StellaDefense);
        text.text = "Stella took " + damage + " damage";
        StellaHealth -= damage;
        Invoke("BurgerDone", 2f);
    }

    void BurgerSpit()
    {
        text.text = "The burger spits ketchup on Stella!";
        burgerAnimator.Play("BurgerSpit");
        Invoke("BurgerSpitResult", 2f);

    }

    void BurgerSpitResult()
    {
        text.text = "Stella's defense fell!";
        StellaDefense -= 1;
        Invoke("BurgerDone", 2f);
    }

    void FadeToBlack()
    {
        toBlack = true;
    }



    private static string GetOrdinalSuffix(int num)
    {
        string number = num.ToString();
        if (number.EndsWith("11")) return "th";
        if (number.EndsWith("12")) return "th";
        if (number.EndsWith("13")) return "th";
        if (number.EndsWith("1")) return "st";
        if (number.EndsWith("2")) return "nd";
        if (number.EndsWith("3")) return "rd";
        return "th";
    }
}
