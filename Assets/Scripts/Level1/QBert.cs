using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class QBert : MonoBehaviour
{

    int LocationID;

    Transform Blocktransform;

    static float yOffset = 0.6f;

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
                transform.position = new Vector3(Blocktransform.position.x, Blocktransform.position.y + yOffset, Blocktransform.position.z);
            }
        }
    }

    void GetInputs()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            LocationID /= 3;
            Debug.Log(LocationID);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            LocationID *= 3;
            Debug.Log(LocationID);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            LocationID *= 2;
            Debug.Log(LocationID);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            LocationID /= 2;
            Debug.Log(LocationID);
        }

        CheckLocation(LocationID);

    }

}
