using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note
{
    public float beat;
    public RhythmScript.Buttons direction;

    public Note(float beat, RhythmScript.Buttons direction)
    {
        this.beat = beat;
        this.direction = direction;
    }

    public Note() { }
}
