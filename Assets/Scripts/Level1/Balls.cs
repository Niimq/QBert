using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Balls : MonoBehaviour
{
    public Sprite redBallAir, redBallIdle, greenBallAir, greenBallIdle;

    [SerializeField] 
    public Transform SpawnPoint;

    public GameObject Qbert, Coiley;
    GameObject newBall;

    SpriteRenderer spriteRenderer;

    public float MovementSpeedByTime = 0.3f;

    int chosenNumber, SpawnBlockID, Level;

    Transform MoveID;
    public bool isGreenBall;
    private bool b_itCanMove;
    bool activateBallDecision;
    bool OriginalMovementDone, b_InsitCanMove;

    private void Start()
    {
        b_itCanMove = true;
        OriginalMovementDone = true;
        chosenNumber = 1;
        activateBallDecision = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Qbert.GetComponent<QBert>().GreenBallEffect)
        {
            StartCoroutine(ApplyGreenBallEffect());
        }
        else if (Coiley.GetComponent<Coiley>().CoileyAnimationDone && !isGreenBall)
        {
            StartCoroutine(ApplyCoileyEffect());
        }
        else
        { 
            if (Qbert.GetComponent<QBert>().GetGameIsRunning())
            {
                if (OriginalMovementDone && b_itCanMove)
                {
                    StartCoroutine(MoveBallsDown());
                }

                if (b_InsitCanMove == true)
                {
                    Debug.Log("Reached");
                    StartCoroutine(MoveInstantiateDown());
                }
            }
            else
            {
                ResetSimulation();
            }
        }      
    }

    IEnumerator ApplyCoileyEffect()
    {   
        ResetSimulation();
        b_itCanMove = false;
        yield return new WaitForSeconds(5.5f);
        b_itCanMove = true;
    }

    IEnumerator ApplyGreenBallEffect()
    {
        yield return new WaitForSeconds(3.0f);
        Qbert.GetComponent<QBert>().GreenBallEffect = false;
    }

    void MyOwnAnimator(int index)
    {
        if (isGreenBall)
        {
            switch (index)
            {
                case 0:
                    {
                        spriteRenderer.sprite = greenBallAir;
                    }
                    break;
                case 1:
                    {
                        spriteRenderer.sprite = greenBallIdle;
                    }
                    break;
            }
        }
        else
        {
            switch (index)
            {
                case 0:
                    {
                        spriteRenderer.sprite = redBallAir;
                    }
                    break;
                case 1:
                    {
                        spriteRenderer.sprite = redBallIdle;
                    }
                    break;

            }
        }
    }

    IEnumerator MoveBallsDown()
    {
        if (activateBallDecision)
        {        
            if (Random.value < 0.5f)

                chosenNumber *= 2;
            else
                chosenNumber *= 3;

            activateBallDecision = false;
            MyOwnAnimator(0);
        }

        if (Qbert != null && Qbert.GetComponent<QBert>() != null && Qbert.GetComponent<QBert>().Blocks != null)
        {
            for (int i = 0; i < Qbert.GetComponent<QBert>().Blocks.Count; i++)
            {
                if (chosenNumber == Qbert.GetComponent<QBert>().Blocks[i].GetComponent<Block>().blockID)
                {
                    MoveID = Qbert.GetComponent<QBert>().Blocks[i].GetComponent<Block>().transform;
                    Level = Qbert.GetComponent<QBert>().Blocks[i].GetComponent<Block>().Level;

                    transform.position = MoveToPoint(new Vector3(MoveID.position.x, MoveID.position.y + 0.35f, MoveID.position.z));

                    if (transform.position == new Vector3(MoveID.position.x, MoveID.position.y + 0.35f, MoveID.position.z))
                    {
                        MyOwnAnimator(1);
                        activateBallDecision = true;
                    }
                }
            }
        }

        Debug.Log(chosenNumber);
        b_itCanMove = false;

        yield return new WaitForSeconds(0.3f);
        if (Level == 7 || chosenNumber > 729 && activateBallDecision)
        {
            Respawn(); // Respawn a new ball
            Destroy(gameObject); // Destroy the current ball
            OriginalMovementDone = false;           
        }
            b_itCanMove = true; // Continue moving the ball   
    }

    IEnumerator MoveInstantiateDown()
    {
        bool LandedOn7thFloor = false;
        if (activateBallDecision)
        {

            if (Random.value < 0.5f)

                chosenNumber *= 2;
            else
                chosenNumber *= 3;

            activateBallDecision = false;
            MyOwnAnimator(0);
        }
        if (Qbert != null && Qbert.GetComponent<QBert>() != null && Qbert.GetComponent<QBert>().Blocks != null)
        {
            for (int i = 0; i < Qbert.GetComponent<QBert>().Blocks.Count; i++)
            {
                if (chosenNumber == Qbert.GetComponent<QBert>().Blocks[i].GetComponent<Block>().blockID)
                {
                    MoveID = Qbert.GetComponent<QBert>().Blocks[i].GetComponent<Block>().transform;
                    Level = Qbert.GetComponent<QBert>().Blocks[i].GetComponent<Block>().Level;

                    newBall.transform.position = MoveToPoint(new Vector3(MoveID.position.x, MoveID.position.y + 0.35f, MoveID.position.z));

                    if (transform.position == new Vector3(MoveID.position.x, MoveID.position.y + 0.35f, MoveID.position.z))
                    {
                        MyOwnAnimator(1);
                        activateBallDecision = true;
                        if (Level == 7) // TODO: not working and it doesn't reach the last floor, they teleport back before reaching to the 7th floor.
                        {
                            LandedOn7thFloor = true;
                        }
                    }
                }
            }
            //transform.position = _LandPoint.position;

            //Debug.Log(_LandPoint.position);
        }
        Debug.Log("Ins" + chosenNumber);
        b_itCanMove = false;

        yield return new WaitForSeconds(0.5f);
        if (Level == 7 || chosenNumber > 729 && LandedOn7thFloor)
        {   
            
            Destroy(newBall); // Destroy the current ball
            Respawn(); // Respawn a new ball
            
        }      
            b_itCanMove = true; // Continue moving the ball       
    }

    public void Respawn()
    {
       // transform.position = SpawnPoint.transform.position;

        if (Random.value < 0.5f)

            SpawnBlockID = 1;
        else
            SpawnBlockID = 2;

        // Check if Qbert is not null before accessing its member
        if (Qbert != null && Qbert.GetComponent<QBert>() != null && Qbert.GetComponent<QBert>().Blocks != null)
        {
            GameObject block = Qbert.GetComponent<QBert>().Blocks[SpawnBlockID];

            if (block != null && block.GetComponent<Block>() != null)
            {
                newBall = Instantiate(gameObject, SpawnPoint.transform.position, Quaternion.identity);
                b_itCanMove = true;

                chosenNumber = block.GetComponent<Block>().blockID;
                //transform.position = _LandPoint.position;
               
                //Debug.Log(_LandPoint.position);
            }
        }
        b_InsitCanMove = true;

    }

    Vector3 MoveToPoint(Vector3 point) // Making it move like so it won't teleport to the target
    {
        return Vector3.MoveTowards(transform.position, point, MovementSpeedByTime);
    }

    void ResetSimulation()
    {
        activateBallDecision = true;
        chosenNumber = 1;
        OriginalMovementDone = true;
        b_InsitCanMove = false;
        transform.position = SpawnPoint.position;
    }
}

