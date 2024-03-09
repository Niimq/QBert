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

    float timeToSpawn = 10;

    int chosenNumber;

    Transform MoveID;

    private bool b_itCanSpawn, b_itCanMove;

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
        b_itCanMove = false;
        
    }

    private void Update()
    {
        if (b_itCanSpawn)
        { 
             Invoke("Respawn", timeToSpawn);
            b_itCanSpawn = false;
            Debug.Log("Reached");
        }

        if (b_itCanMove)
        {
            StartCoroutine(MoveBallsDown());
        }
    }

    IEnumerator MoveBallsDown()
    {
        

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

                    newBall.transform.position = MoveID.position;
                }
            }
                //transform.position = _spawnPoint.position;

                //Debug.Log(_spawnPoint.position);
            
        }

        Debug.Log(chosenNumber);
        b_itCanMove = false;
        yield return new WaitForSeconds(5);
        b_itCanMove = true;
    }

    public void Respawn()
    {

        if (Random.value < 0.5f)
            chosenNumber = 1;
        else
            chosenNumber = 2;

        // Check if Qbert is not null before accessing its member
        if (Qbert != null && Qbert.GetComponent<QBert>() != null && Qbert.GetComponent<QBert>().Blocks != null)
        {
            GameObject block = Qbert.GetComponent<QBert>().Blocks[chosenNumber];

            if (block != null && block.GetComponent<Block>() != null)
            {
                _spawnPoint = block.GetComponent<Block>().transform;

                newBall = Instantiate(gameObject, _spawnPoint.position, Quaternion.identity);
                b_itCanMove = true;
                //transform.position = _spawnPoint.position;

                //Debug.Log(_spawnPoint.position);
            }
        }
    }
}

