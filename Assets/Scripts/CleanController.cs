using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanController : MonoBehaviour
{
    public bool canMove = false;

    Rigidbody2D rb2d;
    public float walkStrength = 1;
    public float speedLimit = 3;
    SpriteRenderer spriteRenderer;

    CleanMaster cleanMaster;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cleanMaster = FindObjectOfType<CleanMaster>();
    }

    // Update is called once per frame
    void Update()
    {


    }

    private void FixedUpdate()
    {
        Vector2 vector2 = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (vector2.magnitude > 1)
            vector2.Normalize();
        if (canMove)
        {
            rb2d.AddForce(vector2 * walkStrength);
            if (rb2d.velocity.magnitude > speedLimit)
                rb2d.velocity = rb2d.velocity.normalized * speedLimit;
            if (vector2.magnitude > 0)
                spriteRenderer.flipX = vector2.x <= 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Litter")
        {
            cleanMaster.Score++;
            Destroy(collision.gameObject);
        }
    }
}
