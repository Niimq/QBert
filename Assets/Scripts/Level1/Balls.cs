using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balls : MonoBehaviour
{
    [SerializeField]
    private Transform _spawnPoint;

    public GameObject Qbert;
    GameObject newBall;

    [SerializeField] private int value = 1;

    float timeToSpawn = 15;

    int chosenNumber, SpawnBlockID, Level;

    Transform MoveID;

    private bool b_itCanSpawn, b_itCanMove;

    bool OriginalMovementDone, b_InsitCanMove;

    public int Value
    {
        get
        {
            return value;
        }
    }

    private void Start()
    {
        b_itCanSpawn = true;
        b_itCanMove = true;
        OriginalMovementDone = true;
    }

    private void Update()
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

    IEnumerator MoveBallsDown()
    {

        if (chosenNumber == 0)
            chosenNumber = 1;

        if (Random.value < 0.5f)

            chosenNumber *= 2;
        else
            chosenNumber *= 3;

        if (Qbert != null && Qbert.GetComponent<QBert>() != null && Qbert.GetComponent<QBert>().Blocks != null)
        {
            for (int i = 0; i < Qbert.GetComponent<QBert>().Blocks.Count; i++)
            {
                if (chosenNumber == Qbert.GetComponent<QBert>().Blocks[i].GetComponent<Block>().blockID)
                {
                    MoveID = Qbert.GetComponent<QBert>().Blocks[i].GetComponent<Block>().transform;
                    Level = Qbert.GetComponent<QBert>().Blocks[i].GetComponent<Block>().Level;

                    transform.position = new Vector3(MoveID.position.x, MoveID.position.y + 0.35f);
                }
            }
                //transform.position = _spawnPoint.position;

                //Debug.Log(_spawnPoint.position);
        }

        Debug.Log(chosenNumber);
        b_itCanMove = false;

        yield return new WaitForSeconds(1);
        if (Level == 7 || chosenNumber > 729)
        {
            Respawn(); // Respawn a new ball
            Destroy(gameObject); // Destroy the current ball
            OriginalMovementDone = false;
            Debug.Log(OriginalMovementDone);
            
          
        }
        else
        {
            b_itCanMove = true; // Continue moving the ball
        }
    }

    IEnumerator MoveInstantiateDown()
    {

        if (chosenNumber == 0)
            chosenNumber = 1;

        if (Random.value < 0.5f)

            chosenNumber *= 2;
        else
            chosenNumber *= 3;

        if (Qbert != null && Qbert.GetComponent<QBert>() != null && Qbert.GetComponent<QBert>().Blocks != null)
        {
            for (int i = 0; i < Qbert.GetComponent<QBert>().Blocks.Count; i++)
            {
                if (chosenNumber == Qbert.GetComponent<QBert>().Blocks[i].GetComponent<Block>().blockID)
                {
                    MoveID = Qbert.GetComponent<QBert>().Blocks[i].GetComponent<Block>().transform;
                    Level = Qbert.GetComponent<QBert>().Blocks[i].GetComponent<Block>().Level;

                    newBall.transform.position = new Vector3(MoveID.position.x, MoveID.position.y + 0.35f);
                }
            }
            //transform.position = _spawnPoint.position;

            //Debug.Log(_spawnPoint.position);
        }

        Debug.Log("Ins" + chosenNumber);
        b_itCanMove = false;

        yield return new WaitForSeconds(3);
        if (Level == 7 || chosenNumber > 729)
        {
            Destroy(newBall); // Destroy the current ball
            Respawn(); // Respawn a new ball
        }
        else
        {
            b_itCanMove = true; // Continue moving the ball
        }
    }

    public void Respawn()
    {
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
                _spawnPoint = block.GetComponent<Block>().transform;

                newBall = Instantiate(gameObject, _spawnPoint.position, Quaternion.identity);
                b_itCanMove = true;

                chosenNumber = block.GetComponent<Block>().blockID;
                //transform.position = _spawnPoint.position;
               
                //Debug.Log(_spawnPoint.position);
            }
        }
        b_InsitCanMove = true;
        Debug.Log(b_InsitCanMove);
    }
}
