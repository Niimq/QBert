using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.SearchService;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    GameObject Qbert;
    public Transform ACheckPoint;

    Animator animator;

    bool ActivateElevator, QbertCanJump;

    // Start is called before the first frame update
    void Start()
    {
        Qbert = GameObject.FindGameObjectWithTag("QBert");
        ActivateElevator = false;
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Activate", ActivateElevator); // It will activate the animation for elevator

        if (ActivateElevator)
        {
            MoveElevator();
        }

        if (QbertCanJump)
        {
            Qbert.GetComponent<QBert>().ExtiElevator();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject == Qbert)
        {
            ActivateElevator = true;
            QbertCanJump = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == Qbert)
        {
            Destroy(gameObject);
        }
    }

    void MoveElevator()
    {
        transform.position = Vector3.Lerp(transform.position, ACheckPoint.position, Time.deltaTime);
        if (transform.position.y >= 3.4f)
        { 
            ActivateElevator = false;
            QbertCanJump = true;
        }
    }
}
