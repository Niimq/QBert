using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QBert : MonoBehaviour
{

    int LocationID;

    Transform Blocktransform;

    public List<GameObject> Blocks;

    // Start is called before the first frame update
    void Start()
    {
        LocationID = 1;
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
    }

    void CheckLocation(int id) 
    {

        for (int i = 0; i < Blocks.Count; i++)
        {
            if (id == Blocks[i].gameObject.GetComponent<Block>().blockID)
            {
                Blocktransform = Blocks[i].gameObject.GetComponent<Transform>().transform;  
            }
        }
    }

    void GetInputs()
    {
        
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            LocationID /= 3;
            Debug.Log(LocationID);
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            LocationID *= 3;
            Debug.Log(LocationID);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            LocationID *= 2;
            Debug.Log(LocationID);
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            LocationID /= 2;
            Debug.Log(LocationID);
        }

        CheckLocation(LocationID);

    }
}
