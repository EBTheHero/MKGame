using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualNote : MonoBehaviour
{
    public Conductor conductor;
    public Note note;
    RectTransform rect;
    public float speed;
    Image img;

    public Sprite[] sprites;
    public Color[] Colors;

    public bool hit = false;
    void Start()
    {
        rect = GetComponent<RectTransform>();
        img = GetComponent<Image>();
        img.sprite = sprites[(int)note.direction];
        img.color = Colors[(int)note.direction];
    }

    // Update is called once per frame
    void Update()
    {
        if (rect && !hit)
            rect.anchoredPosition = new Vector2(0, (note.beat - conductor.songPositionInBeats) * speed);
    }

    public void getHit()
    {
        hit = true;
        GetComponent<Animation>().Play();
        Invoke("Die", 0.5f);
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
