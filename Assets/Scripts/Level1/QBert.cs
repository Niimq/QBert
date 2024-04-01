using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEditor.Animations;
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

    int whereToJump = 0;

    Animator animator;
    AnimatorControllerParameter Parameter;
    AnimatorController Controller;

    // Start is called before the first frame update
    void Start()
    {
        LocationID = 1;
        bCheckLocation = true;
        ActivateCoiley = false;
        animator = GetComponent<Animator>();  
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
        return Vector3.MoveTowards(transform.position, point, 0.03f);
       
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
                if (transform.position == new Vector3(Blocktransform.position.x, Blocktransform.position.y + yOffset, Blocktransform.position.z)) 
                {
                    animator.SetInteger("WhereToJump", GetWhereToJumpAfterLand());
                    bCheckLocation = false; // making sure that player doesn't jitter.
                }
            }
        }
    }

    int GetWhereToJumpAfterLand()
    {
        if (whereToJump == 1)
        { 
            // if left up was pressed.
            SetWhereToJump(5); // translation to Idle.
        }
        
        if (whereToJump == 2)
        { 
            // Right Down Idle Transation Set
            SetWhereToJump(6);
        }
        
        if (whereToJump == 3)
        {
            // Left Down Idle Transation Set
            SetWhereToJump(7);
        }
        
        if (whereToJump == 4)
        {
            // Right Up Idle Transation Set
            SetWhereToJump(8);
        }

        return whereToJump;
    }

    void SetWhereToJump(int num)
    {
        whereToJump = num;
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
                SetWhereToJump(1);
                animator.SetInteger("WhereToJump", whereToJump); // 1 meaning Left Up 
                LocationID /= 3;
                Debug.Log(LocationID);
                bCheckLocation = true;
            }  
        }
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            SetWhereToJump(2);
            animator.SetInteger("WhereToJump", whereToJump); // 2 meaning Right Down
            LocationID *= 3;
            Debug.Log(LocationID);
            bCheckLocation = true;
        }
        if (Input.GetKeyDown(KeyCode.Z)) 
        {
            SetWhereToJump(3);
            animator.SetInteger("WhereToJump", whereToJump); // 3 meaning Left Down
            LocationID *= 2;
            Debug.Log(LocationID);
            bCheckLocation = true;            
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (LocationID == 81 && ElevatorB != null)
            {
               onElevatorB = true;
            }
            else
            {
                SetWhereToJump(4);
                animator.SetInteger("WhereToJump", whereToJump); // 4 meaning Right Up
                LocationID /= 2;
                Debug.Log(LocationID);
                bCheckLocation = true;              
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
