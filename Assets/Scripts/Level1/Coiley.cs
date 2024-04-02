using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coiley : MonoBehaviour
{
    public Sprite HatchedCoiley;
    SpriteRenderer SpriteRenderer;
    static GameObject Qbert;

    private Transform _spawnPoint;

    GameObject NewCoiley;

    Transform MoveID;

    int SpawnBlockID, CoileyID, CoileyLevel;

    bool CoileyHatched, itCanMove;
    bool activateCoileyEyes; // :D Coiley Eyes 0~0

    // Start is called before the first frame update
    void Start()
    {
        if (Qbert == null)
        {
            Qbert = GameObject.FindWithTag("QBert");
        }
        CoileyHatched = false;
        itCanMove = true;
        activateCoileyEyes = true;
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Qbert != null && itCanMove)
        {
            bool activeCoiley = Qbert.GetComponent<QBert>().ActivateCoiley; // Coiley gets spawned
            if (activeCoiley && CoileyHatched != true)
            {
                Debug.Log("Coiley!!");
                StartCoroutine(MoveCoileyDown());
            }

            if (CoileyHatched == true)
            {
                SpriteRenderer.sprite = HatchedCoiley;
                StartCoroutine(CoileyLookWhereYouGoing());
            }
        }
    }

    IEnumerator MoveCoileyDown()
    {

        if (CoileyID == 0)
            CoileyID = 1;

        if (Random.value < 0.5f)

            CoileyID *= 2;
        else
            CoileyID *= 3;

        if (Qbert != null && Qbert.GetComponent<QBert>() != null && Qbert.GetComponent<QBert>().Blocks != null)
        {
            for (int i = 0; i < Qbert.GetComponent<QBert>().Blocks.Count; i++)
            {
                if (CoileyID == Qbert.GetComponent<QBert>().Blocks[i].GetComponent<Block>().blockID)
                {
                    MoveID = Qbert.GetComponent<QBert>().Blocks[i].GetComponent<Block>().transform;
                    CoileyLevel = Qbert.GetComponent<QBert>().Blocks[i].GetComponent<Block>().Level;
                    transform.position = new Vector3(MoveID.position.x, MoveID.position.y + 0.35f);
                    
                }
            }
        }

        Debug.Log(CoileyID);
        itCanMove = false;
        yield return new WaitForSeconds(3);
        itCanMove = true;
        if (CoileyLevel == 7)
            CoileyHatched = true;
    }

    IEnumerator CoileyLookWhereYouGoing()
    {

        if (activateCoileyEyes)
        {

            int TargetID = Qbert.GetComponent<QBert>().LocationID;
            int NextBlockIDA, NextBlockIDB;

            if (CoileyLevel >= Qbert.GetComponent<QBert>().LevelID)
            {
                NextBlockIDA = CoileyID / 3;
                NextBlockIDB = CoileyID / 2;
                if (TargetID % NextBlockIDA == 0 && TargetID % NextBlockIDB == 0)
                {
                    if (Random.value < 0.5f)
                        CoileyID /= 2;
                    else
                        CoileyID /= 3;
                }
                else if (TargetID % NextBlockIDA == 0)
                {
                    CoileyID /= 3;
                }
                else
                {
                    CoileyID /= 2;
                }
            }
            else
            {
                NextBlockIDA = CoileyID * 3;
                NextBlockIDB = CoileyID * 2;
                if (TargetID % NextBlockIDA == 0 && TargetID % NextBlockIDB == 0)
                {
                    if (Random.value < 0.5f)
                        CoileyID *= 2;
                    else
                        CoileyID *= 3;
                }
                else if (TargetID % NextBlockIDA == 0)
                {
                    CoileyID *= 3;
                }
                else
                {
                    CoileyID *= 2;
                }
            }
            activateCoileyEyes = false;
        }

        if (Qbert != null && Qbert.GetComponent<QBert>() != null && Qbert.GetComponent<QBert>().Blocks != null)
        {        
            for (int i = 0; i < Qbert.GetComponent<QBert>().Blocks.Count; i++)
            {
                if (CoileyID == Qbert.GetComponent<QBert>().Blocks[i].GetComponent<Block>().blockID)
                {
                    MoveID = Qbert.GetComponent<QBert>().Blocks[i].GetComponent<Block>().transform;
                    CoileyLevel = Qbert.GetComponent<QBert>().Blocks[i].GetComponent<Block>().Level;

                    transform.position = MoveToPoint(new Vector3(MoveID.position.x, MoveID.position.y + 0.35f, MoveID.position.z));

                    if (transform.position == new Vector3(MoveID.position.x, MoveID.position.y + 0.35f))
                    {
                        activateCoileyEyes = true;
                    }
                }
            }
        }
        
        Debug.Log(CoileyID);
        itCanMove = false;
        yield return new WaitForSeconds(0.75f);
        itCanMove = true;
    }

    Vector3 MoveToPoint(Vector3 point) // Making it move like so it won't teleport to the target
    {
        return Vector3.MoveTowards(transform.position, point, 0.5f);
    }


    //public void Respawn()
    //{
    //    if (Random.value < 0.5f)

    //        SpawnBlockID = 1;
    //    else
    //        SpawnBlockID = 2;

    //    // Check if Qbert is not null before accessing its member
    //    if (Qbert != null && Qbert.GetComponent<QBert>() != null && Qbert.GetComponent<QBert>().Blocks != null)
    //    {
    //        GameObject block = Qbert.GetComponent<QBert>().Blocks[SpawnBlockID];

    //        if (block != null && block.GetComponent<Block>() != null)
    //        {
    //            _spawnPoint = block.GetComponent<Block>().transform;

    //            NewCoiley = Instantiate(gameObject, _spawnPoint.position, Quaternion.identity);

    //            CoileyID = block.GetComponent<Block>().blockID;

    //        }
    //    }
    //}
}
