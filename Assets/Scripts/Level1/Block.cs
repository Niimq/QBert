using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Sprite CompletedBlock;
    static GameObject QBert;
    SpriteRenderer SpriteRenderer;

    public int blockID; // This ID will be overloaded based off of a pattern
    bool Switch;

    void Start()
    {
        Switch = false;

        if (QBert == null)
        {
            QBert = GameObject.FindWithTag("QBert");
        }

        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeColor();
    }

    void ChangeColor() 
    {
        if (Switch)
        {
            SpriteRenderer.sprite = CompletedBlock;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == QBert)
        {
            Switch = true; // Setting the switch to true to it can change color.
        }
    }
}
