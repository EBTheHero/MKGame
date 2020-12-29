using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soundbox : MonoBehaviour
{
    public AudioClip[] clips;
    AudioSource audioSource;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound(string soundname)
    {
        AudioClip clip = null;
        foreach (var item in clips)
        {
            if (item.name.ToLower().Contains(soundname.ToLower()))
                clip = item;
        }

        if (clip != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(clip);
        }
    } 
}
