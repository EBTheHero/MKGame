using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    public AudioClip[] musics;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void stopMusic()
    {
        audioSource.Stop();
    }

    public void PlayMusic(string name)
    {
        AudioClip clip = null;
        foreach (var item in musics)
        {
            if (item.name.ToLower().Contains(name.ToLower()))
                clip = item;
        }

        if (clip != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(clip);
        }
    }
}
