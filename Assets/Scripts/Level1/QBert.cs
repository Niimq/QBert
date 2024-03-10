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

    public GameObject ElevatorA, ElevatorB;

    bool bCheckLocation;

    // Start is called before the first frame update
    void Start()
    {
        LocationID = 1;
        bCheckLocation = true;
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
            if (LocationID == 16)
            {
                OnElevatorA();
            }
            else
            { 
                LocationID /= 3;
                Debug.Log(LocationID);
            }  
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
            if (LocationID == 81)
            {
                OnElevatorB();
            }
            else
            {
                LocationID /= 2;
                Debug.Log(LocationID);
            }
            
        }
       
        if (bCheckLocation)
        { CheckLocation(LocationID); }
    }

    void OnElevatorA()
    {
        bCheckLocation = false;

        transform.position = new Vector3(ElevatorA.transform.position.x, ElevatorA.transform.position.y +
            0.3f, ElevatorA.transform.position.z);
    }
    
    void OnElevatorB()
    {
        bCheckLocation = false;

        transform.position = new Vector3(ElevatorB.transform.position.x, ElevatorB.transform.position.y +
            0.3f, ElevatorB.transform.position.z);
    }
}
