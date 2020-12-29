using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortItem : MonoBehaviour
{
    [SerializeField]
    public Sprite[] magazines;
    [SerializeField]
    public Sprite[] purses;

    SpriteRenderer spriteRenderer;

    public SortItemType itemType;

    public enum SortItemType
    {
        Purse,
        Magazine
    }


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -10)
            Destroy(gameObject);
    }

    public void setItem(SortItemType sortItemType)
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        itemType = sortItemType;

        int r = Random.Range(0, 4);
        if (sortItemType == SortItemType.Purse)
            spriteRenderer.sprite = purses[r];
        else
            spriteRenderer.sprite = magazines[r];

        gameObject.AddComponent<PolygonCollider2D>();
    }
}
