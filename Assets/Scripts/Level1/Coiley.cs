using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
        yield return new WaitForSeconds(0.10f);
        itCanMove = true;
        if (CoileyLevel == 7)
            CoileyHatched = true;
    }

    IEnumerator CoileyLookWhereYouGoing()
    {

        if (activateCoileyEyes)
        {

            CoileyDecides();

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

                    transform.position = MoveToPoint(new Vector3(MoveID.position.x, MoveID.position.y + 0.45f, MoveID.position.z));

                    if (transform.position == new Vector3(MoveID.position.x, MoveID.position.y + 0.45f, MoveID.position.z))
                    {
                        Debug.Log("REACHED");
                        activateCoileyEyes = true;
                    }
                }
            }
        }

        Debug.Log(CoileyID);

        itCanMove = false;
        yield return new WaitForSeconds(0.25f);
        itCanMove = true;
    }

    void CoileyDecides()
    {
        int TargetID = Qbert.GetComponent<QBert>().LocationID;
        float NextBlockIDA, NextBlockIDB;
        bool QbertIsOnTheRight = false;

        if (CoileyLevel > Qbert.GetComponent<QBert>().LevelID) // if coiley is lower than Qbert in the blocks
        {
            NextBlockIDA = CoileyID / 3;
            NextBlockIDB = CoileyID / 2;
            if (NextBlockIDA == TargetID)
            {
                CoileyID /= 3;
            }
            else if (NextBlockIDB == TargetID)
            {
                CoileyID /= 2;
            }
            else if (TargetID % NextBlockIDA == 0 && TargetID % NextBlockIDB == 0) // checking if coiley can jump down freely - but still he doesn't know which one to pick
            {
                if (GetQbertOnTheLeft(NextBlockIDA, NextBlockIDB, TargetID, true))
                    CoileyID /= 2;
                else
                    CoileyID /= 3;
            }
            else if (TargetID % NextBlockIDA == 0)
            {
                CoileyID /= 3;
            }
            else if (TargetID % NextBlockIDB == 0)
            {
                CoileyID /= 2;
            }
            else
            {
                // in this case TargetID is smaller than nextBlockIDs
                if (GetQbertOnTheLeft(NextBlockIDA, NextBlockIDB, TargetID, true))
                {
                    CoileyID /= 2;
                }
                else { CoileyID /= 3; }

            }
        }

        if (CoileyLevel == Qbert.GetComponent<QBert>().LevelID) // if == levels are same
        {
            if (CoileyID < Qbert.GetComponent<QBert>().LocationID) // Coiley is on the left.
            {
                QbertIsOnTheRight = true;
                CoileyID /= 2;
            }
            else
            {
                QbertIsOnTheRight = false;
                CoileyID /= 3;
            }
        }

        if (CoileyLevel < Qbert.GetComponent<QBert>().LevelID)
        {
            NextBlockIDA = CoileyID * 3;
            NextBlockIDB = CoileyID * 2;
            if (NextBlockIDA == TargetID)
            {
                CoileyID *= 3;
            }
            else if (NextBlockIDB == TargetID)
            {
                CoileyID *= 2;
            }
            else if (CoileyLevel + 1 == Qbert.GetComponent<QBert>().LevelID) // gotta check if Coiley is only 1 level above or more. because if yes we wanna do a different behavior.
            {
                // in this case TargetID is smaller than nextBlockIDs
                if (GetQbertOnTheLeft(NextBlockIDA, NextBlockIDB, TargetID, false))
                {
                    CoileyID *= 2;
                }
                else { CoileyID *= 3; }
            }
            else if (TargetID % NextBlockIDA == 0 && TargetID % NextBlockIDB == 0)
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
            else if (TargetID % NextBlockIDB == 0)
            {
                CoileyID *= 2;
            }
            else
            {
                // in this case TargetID is smaller than nextBlockIDs
                if (GetQbertOnTheLeft(NextBlockIDA, NextBlockIDB, TargetID, false))
                {
                    CoileyID *= 2;
                }
                else { CoileyID *= 3; }

            }
        }
    }

    bool GetQbertOnTheLeft(float IDA, float IDB, int TargetID, bool GoingUp)
    {
        if (GoingUp)
        {
            if (IDB > TargetID && IDA < TargetID)
            {
                if ((IDA - TargetID)  * -1> IDB - TargetID)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (IDA - TargetID > IDB - TargetID) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (IDB < TargetID && IDA > TargetID)
            {
                if (IDA - TargetID < (IDB - TargetID) * -1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (IDA - TargetID < IDB - TargetID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    Vector3 MoveToPoint(Vector3 point) // Making it move like so it won't teleport to the target
    {
        return Vector3.MoveTowards(transform.position, point, 0.1f);
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
