using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Sprite CompletedBlock, RawBlockSprite;
    static GameObject QBert;
    SpriteRenderer SpriteRenderer;

    public int SwitchValue;
    public int Level;

    public int blockID; // This ID will be overloaded based off of a pattern
    bool Switch;

    void Start()
    { 
        Switch = false;
        SwitchValue = 0;

        if (QBert == null)
        {
            QBert = GameObject.FindWithTag("QBert");
        }

        SpriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (QBert.GetComponent<QBert>().playerHasWon)
        {
           StartCoroutine(victoryAnimation());
        }
 
        ChangeColor();
    }

    IEnumerator victoryAnimation()
    {
        SpriteRenderer.sprite = RawBlockSprite;
        yield return new WaitForSeconds(0.1f);
        SpriteRenderer.sprite = CompletedBlock;
    }

    void ChangeColor() 
    {
        if (Switch)
        {
            SpriteRenderer.sprite = CompletedBlock;
            QBert.GetComponent<QBert>().AddScore(25);
            SwitchValue = 1;
            Switch = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == QBert && SpriteRenderer.sprite != CompletedBlock) // and make sure the block is not a completed block.
        {
            Switch = true; // Setting the switch to true to it can change color.
        }
    }
}
