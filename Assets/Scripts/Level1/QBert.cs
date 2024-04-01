using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class QBert : MonoBehaviour
{
    public int LocationID, LevelID;

    Transform Blocktransform;

    static float yOffset = 0.6f;

    public List<GameObject> Blocks;

    public GameObject ElevatorA, ElevatorB;

    bool bCheckLocation, onElevatorA, onElevatorB;

    public bool ActivateCoiley;

    // Start is called before the first frame update
    void Start()
    {
        LocationID = 1;
        bCheckLocation = true;
        ActivateCoiley = false;
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();

        if (onElevatorA)
            OnElevatorA();

        if (onElevatorB)
            OnElevatorB();

        if (LevelID == 4)
        {
            ActivateCoiley = true;
        }
            
    }

    Vector3 MoveToPoint(Vector3 point) // Making it move like so it won't teleport to the target
    {
        return Vector3.Lerp(transform.position, point, 0.05f);
    }

    void CheckLocation(int id)
    {

        for (int i = 0; i < Blocks.Count; i++)
        {
            if (id == Blocks[i].gameObject.GetComponent<Block>().blockID)
            {
                Blocktransform = Blocks[i].gameObject.GetComponent<Transform>().transform;
                transform.position = MoveToPoint( new Vector3(Blocktransform.position.x, Blocktransform.position.y + yOffset, Blocktransform.position.z));
                LevelID = Blocks[i].GetComponent<Block>().Level;  
            }
        }
    }

    void GetInputs()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (LocationID == 16 && ElevatorA != null)
            {
                onElevatorA = true;
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
            if (LocationID == 81 && ElevatorB != null)
            {
               onElevatorB = true;
            }
            else
            {
                LocationID /= 2;
                Debug.Log(LocationID);
            }  
        }
       
        if (bCheckLocation) // when on elevator don't check the location.
        { CheckLocation(LocationID); }
    }

    void OnElevatorA()
    {
        bCheckLocation = false;

        transform.position = new Vector3(ElevatorA.transform.position.x, ElevatorA.transform.position.y +
            0.3f, transform.position.z);
    }
    
    void OnElevatorB()
    {
        bCheckLocation = false;

        transform.position = new Vector3(ElevatorB.transform.position.x, ElevatorB.transform.position.y +
            0.3f, transform.position.z);
    }

    public void ExtiElevator()
    {
        transform.position = new Vector3(Blocks[0].transform.position.x,
            Blocks[0].transform.position.y + yOffset, Blocks[0].transform.position.z); // move the Qbert to top most block. 0 in the list
        LocationID = 1;
        onElevatorA = false;
        onElevatorB =false;
        bCheckLocation = true;

    }
}
