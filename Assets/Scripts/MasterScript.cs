using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class MasterScript : MonoBehaviour
{

    public enum Minigame { Pokemon, Sorting, FPS, Stamp, Rhythm, Clean}

    public Minigame? currentMinigame;

    public GameObject letter;
    DialogueRunner dialogueRunner;
    public EventSystem eventSystem;
    public GameObject button;
    MusicManager musicManager;
    Scene pokemonScene;

    public CanvasGroup endCanvasGroup;

    public GameObject[] loadunloadMinigame;

    public AudioClip QuackFX;

    public Material pokemonTransition;
    public Sprite transition;
    bool FadeToBlack = false;
    float pokemontransExp = 0;

    bool FadeIn = false;
    public bool isTheEnd = false;



    // Start is called before the first frame update
    void Start()
    {
        dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
        //Cursor.visible = false;
        musicManager = GetComponent<MusicManager>();
        Application.targetFrameRate = 60;
        pokemonTransition.SetFloat("_Cutoff", 0);
        //startminigame("pokemon");
        pokemonTransition.SetTexture("_TransitionTex", transition.texture);
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;



        musicManager.PlayMusic("wholesome");
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        SceneManager.SetActiveScene(arg0);
        TurnoffMainWorld();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && letter.activeSelf)
        {
            //first a press
            letter.SetActive(false);
            dialogueRunner.StartDialogue("DimitriFirst");
        }

        if (Input.GetButtonDown("Start"))
            GetComponent<AudioSource>().PlayOneShot(QuackFX);

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (isTheEnd)
        {
            endCanvasGroup.alpha += 0.001f;
        }
        else
            endCanvasGroup.alpha = 0;
    }

    private void FixedUpdate()
    {
        if (FadeToBlack && pokemonTransition.GetFloat("_Cutoff") <= 1)
        {
            if (pokemonTransition.GetFloat("_Cutoff") == 0)
                pokemontransExp = 0.00025f;
            pokemonTransition.SetFloat("_Cutoff", pokemonTransition.GetFloat("_Cutoff") + pokemontransExp);
            pokemontransExp += 0.0003f;
        }
            
        if (FadeToBlack && pokemonTransition.GetFloat("_Cutoff") >= 1)
            FadeToBlack = false;

        if (FadeIn && pokemonTransition.GetFloat("_Cutoff") >= 0)
        {
            pokemonTransition.SetFloat("_Cutoff", pokemonTransition.GetFloat("_Cutoff") - 0.03f);
        }

        if (FadeIn && pokemonTransition.GetFloat("_Cutoff") <= 0)
            FadeIn = false;
    }


    public void FixSelection()
    {
        //fix
        EventSystem.current.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
    }

    [YarnCommand("startminigame")]
    public void startminigame(string game)
    {
        
        switch (game.ToLower())
        {
            case "pokemon":
                currentMinigame = Minigame.Pokemon;
                musicManager.PlayMusic("pokemon");
                FadeToBlack = true;
                Invoke("LoadPokemon", 2f);
                break;

            case "sorting":
                currentMinigame = Minigame.Sorting;
                musicManager.PlayMusic("fuzzball");
                FadeToBlack = true;
                Invoke("LoadSorting", 2f);
                break;
            case "stamp":
                currentMinigame = Minigame.Stamp;
                musicManager.PlayMusic("sneaky");
                FadeToBlack = true;
                Invoke("LoadStamp", 2f);
                break;
            case "rhythm":
                currentMinigame = Minigame.Rhythm;
                musicManager.stopMusic();
                FadeToBlack = true;
                Invoke("LoadRhythm", 2f);
                break;
            case "clean":
                currentMinigame = Minigame.Clean;
                musicManager.PlayMusic("Cannery");
                FadeToBlack = true;
                Invoke("LoadClean", 2f);
                break;
            default:
                break;
        }
    }

    private void TurnoffMainWorld()
    {
        foreach (GameObject item in loadunloadMinigame)
        {
            if (item != null)
                item.SetActive(false);
        }
    }

    void LoadPokemon()
    {
        SceneManager.LoadSceneAsync("Pokemon", LoadSceneMode.Additive);
    }

    void LoadStamp()
    {
        SceneManager.LoadSceneAsync("Stamp", LoadSceneMode.Additive);
    }

    void LoadSorting()
    {
        SceneManager.LoadSceneAsync("Sorting", LoadSceneMode.Additive);
    }

    void LoadRhythm()
    {
        SceneManager.LoadSceneAsync("Rhythm", LoadSceneMode.Additive);
    }

    void LoadClean()
    {
        SceneManager.LoadSceneAsync("Clean", LoadSceneMode.Additive);
    }

    public void MinigameDone(bool success)
    {
        MinigameOver();
        musicManager.PlayMusic("wholesome");
        FadeIn = true;

        switch (currentMinigame)
        {
            case Minigame.Pokemon:
                SceneManager.UnloadSceneAsync("Pokemon");
                if (success)
                    dialogueRunner.StartDialogue("PokemonSuccess");
                else
                    dialogueRunner.StartDialogue("PokemonFailed");
                break;
            case Minigame.Sorting:
                SceneManager.UnloadSceneAsync("Sorting");
                if (success)
                    dialogueRunner.StartDialogue("SortingSuccess");
                else
                    dialogueRunner.StartDialogue("SortingFailed");
                break;
            case Minigame.FPS:
                break;
            case Minigame.Stamp:
                SceneManager.UnloadSceneAsync("Stamp");
                if (success)
                    dialogueRunner.StartDialogue("StampSuccess");
                else
                    dialogueRunner.StartDialogue("StampFailed");
                break;
            case Minigame.Rhythm:
                SceneManager.UnloadSceneAsync("Rhythm");
                    dialogueRunner.StartDialogue("RhythmSuccess");
                break;
            case Minigame.Clean:
                SceneManager.UnloadSceneAsync("Clean");
                if (success)
                    dialogueRunner.StartDialogue("CleanSuccess");
                else
                    dialogueRunner.StartDialogue("CleanFailed");
                break;
            default:
                Debug.LogError("Minigame done called, but minigame wasn't set");
                break;
        }
        currentMinigame = null;
    }

    void MinigameOver()
    {
        foreach (GameObject item in loadunloadMinigame)
        {
            if (item != null)
                item.SetActive(true);
        }
    }

    [YarnCommand("endgame")]
    public void endgame()
    {
        isTheEnd = true;
        musicManager.PlayMusic("end");
        Debug.Log("endgame");
    }
}
