using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    GameObject Qbert;
    public Transform ACheckPoint;

    // Start is called before the first frame update
    void Start()
    {
        Qbert = GameObject.FindGameObjectWithTag("QBert");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject == Qbert)
        {
         
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }
}
