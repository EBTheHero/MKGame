using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    //Song beats per minute
    //This is determined by the song you're trying to sync up to
    public float songBpm;

    //The number of seconds for each song beat
    public float secPerBeat;

    //Current song position, in seconds
    public float songPosition;

    //Current song position, in beats
    public float songPositionInBeats;

    //How many seconds have passed since the song started
    public float dspSongTime;

    //an AudioSource attached to this GameObject that will play the music.
    public AudioSource musicSource;

    //The offset to the first beat of the song in seconds
    public float firstBeatOffset;

    public TextMeshProUGUI beatUI;

    public float startTime = 1f;
    float startTimer = float.MaxValue;

    RhythmScript rhythmScript;

    void Start()
    {
        //Load the AudioSource attached to the Conductor GameObject
        musicSource = GetComponent<AudioSource>();

        //Calculate the number of seconds in each beat
        secPerBeat = 60f / songBpm;

        //Record the time when the music starts
        dspSongTime = (float)AudioSettings.dspTime;

        rhythmScript = FindObjectOfType<RhythmScript>();
    }

    public void startConductor()
    {
        startTimer = startTime;
        musicSource.Stop();
    }

    void Update()
    {
        if (!musicSource.isPlaying)
        {
            if (rhythmScript.gameStarted && startTimer == -69)
            {
                rhythmScript.gameOver();
                startTimer = float.MaxValue;
            }
                
            startTimer -= Time.deltaTime;
            songPosition = -startTimer;
        } else
        {
            //determine how many seconds since the song started
            
            songPosition = (float)((AudioSettings.dspTime - dspSongTime) * musicSource.pitch - firstBeatOffset);
        }

        if (startTimer - firstBeatOffset <= 0 && !musicSource.isPlaying)
        {
            //Record the time when the music starts
            dspSongTime = (float)AudioSettings.dspTime;
            GameObject.Find("ShowCam").GetComponent<Animation>().Stop();
            GameObject.Find("ShowCam").GetComponent<Animation>().Play();
            //Start the music
            musicSource.Play();
            startTimer = -69;
        }

        //determine how many beats since the song started
        songPositionInBeats = songPosition / secPerBeat;
        //beatUI.text = songPositionInBeats.ToString("F1");
    }
}