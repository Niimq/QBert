using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QBert : MonoBehaviour
{
    public int LevelID;
    public float LocationID;

    Transform Blocktransform;

    Vector3 InitialPos;

    static float yOffset = 0.6f;

    public List<GameObject> Blocks;

    public GameObject ElevatorA, ElevatorB;

    bool bCheckLocation, onElevatorA, onElevatorB;

    public bool ActivateCoiley;

    bool GameIsRunning, GameOver, DeathAnimationWorking;

    int whereToJump = 0, DeathAnimationOverNum;

    Animator animator;

    SpriteRenderer spriteRenderer;

    public GameObject GameOverPanel;

    public GameObject Curse;
    new Rigidbody2D rigidbody;

    CapsuleCollider2D CapsuleCollider;

    [SerializeField]
    private List<GameObject> QbertHealthIcon;
    int QbertHealthIconIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        LocationID = 1;
        bCheckLocation = true;
        ActivateCoiley = false;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
        GameIsRunning = true;
        GameOver = false;
        Curse.SetActive(false);
        DeathAnimationWorking = true;
        DeathAnimationOverNum = 0;
        GameOverPanel.SetActive(false);
        rigidbody.isKinematic = false;
        InitialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.position.y < -5)
        {
            SetGameIsRunning(false);
            DecreamentHealth();
            transform.position = InitialPos;
            // and we also have to disable falling effect.
            CapsuleCollider.isTrigger = false;
            bCheckLocation = true;
            rigidbody.gravityScale = 0; 
            spriteRenderer.sortingOrder = 2;
        }

        if (DeathAnimationOverNum > 1000) // this is to flicker the character for the death animation.
        {
            DeathAnimationWorking = false;
        }

        if (!GameOver)
        {

            if (LocationID % 1 == 0)
            {
                if (GetGameIsRunning())
                {
                    GetInputs();
                }
                else
                {
                    StartCoroutine(ResetSimulation());
                }

                if (onElevatorA)
                    OnElevatorA();

                if (onElevatorB)
                    OnElevatorB();

                if (LevelID == 4)
                {
                    ActivateCoiley = true;
                }
            }
            else
            {
                if (whereToJump == 1)
                {
                    // if left up was pressed.
                    DyingJumpDirection(1, true);
                }

                if (whereToJump == 4)
                {
                    // Right Up Idle Transation Set
                    DyingJumpDirection(0, true);
                }               
            }
        }
        else
        {
            //Time.timeScale = 0.0f;
            if (DeathAnimationWorking)
            {
                StartCoroutine(PlayDeathAnimation());
            }
            else
            {
                StartCoroutine(DisplayGameOver());
            }
        }
    }

    public bool GetGameIsRunning() // Public - Getter
    { return GameIsRunning; }

    void SetGameIsRunning(bool condition) // Setter
    { GameIsRunning = condition; }

    Vector3 MoveToPoint(Vector3 point) // Making it move like so it won't teleport to the target
    {
        return Vector3.MoveTowards(transform.position, point, 0.03f);
    }

    IEnumerator ResetSimulation()
    {
        yield return new WaitForSeconds(3);

        SetGameIsRunning(true);
        ActivateCoiley = true;
        Curse.SetActive(false);
    }

    void UpdateLocation(float id)
    {

        for (int i = 0; i < Blocks.Count; i++)
        {
            if (id == Blocks[i].gameObject.GetComponent<Block>().blockID)
            {
                Blocktransform = Blocks[i].gameObject.GetComponent<Transform>().transform;
                transform.position = MoveToPoint(new Vector3(Blocktransform.position.x, Blocktransform.position.y + yOffset, Blocktransform.position.z));
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

    void DyingJumpDirection(int index, bool IsNotLevel7)
    {
        switch (index)
        { 
            case 0:
            // He jumped right down from pyramid.
            LocationID = 1;
            CapsuleCollider.isTrigger = true;
            bCheckLocation = false;
            rigidbody.AddForce(new Vector2(0.7f, 1.5f), ForceMode2D.Impulse);
            rigidbody.gravityScale = 1;
                if (IsNotLevel7)
                    spriteRenderer.sortingOrder = 0;
                break;

            case 1:
            // He jumped left down from pyramid.
            LocationID = 1;
            CapsuleCollider.isTrigger = true;
            bCheckLocation = false;
            rigidbody.AddForce(new Vector2(-0.7f, 1.5f), ForceMode2D.Impulse);
            rigidbody.gravityScale = 1;
                if (IsNotLevel7)
                    spriteRenderer.sortingOrder = 0;
                break;
        }
        
    }

    void SetWhereToJump(int num)
    {
        whereToJump = num;
    }

    IEnumerator PlayDeathAnimation()
    {
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(0.2f);
        DeathAnimationOverNum++;
        spriteRenderer.enabled = true;
    }

    IEnumerator DisplayGameOver()
    {
        GameOverPanel.SetActive(true);
        yield return new WaitForSeconds(4.0f);
        SceneManager.LoadScene(0);
    }

    void GetInputs()
    {
        animator.SetBool("OnElevatorA", onElevatorA);
        animator.SetBool("OnElevatorB", onElevatorB);

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

            if (LevelID != 7) // making sure that we are not at level 7 so we can know if this function gets called when we are at level 7 meaning that we jumped down.
            {
                
                LocationID *= 3;
                Debug.Log(LocationID);
                bCheckLocation = true;
            }
            else
            {

                DyingJumpDirection(0, false);
            }
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SetWhereToJump(3);
            animator.SetInteger("WhereToJump", whereToJump); // 3 meaning Left Down
            if (LevelID != 7) // making sure that we are not at level 7
            {
                
                LocationID *= 2;
                Debug.Log(LocationID);
                bCheckLocation = true;
            }
            else
            {
                DyingJumpDirection(1, false);
            }
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


        if (bCheckLocation) // Switch for if we want apply checklocation or not.
        { UpdateLocation(LocationID); }
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
        onElevatorB = false;
        bCheckLocation = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (onElevatorA == false && onElevatorB == false) //  we don't care if Qbert is on Elevator.
            {
                SetGameIsRunning(false);
                ActivateCoiley = false; // de activating coiley
                Curse.SetActive(true);
                
                DecreamentHealth();
            }
        }

        if (collision.gameObject.tag == "GreenBall")
        {
            Destroy(collision.gameObject);
        }
    }
    public void DecreamentHealth()
    {
        if (QbertHealthIconIndex < 3) // after his lives are out if he dies Game is over.
        {
            Destroy(QbertHealthIcon[QbertHealthIconIndex]);

            QbertHealthIconIndex++;
        }
        else
        {
            GameOver = true;
        }
    }
}
