//**************************QBERT**TABLE**********************\\\    
//          0000000000000000000000000000000000000000000000              
//          0                     1                      0  Left Up: /3 
//          0                 2       3                  0              
//          0              4     6      9                0  Left Down: *2
//          0           8    12     18     27            0              
//          0        16   24     36     54     81        0  Right Up: /2 
//          0     32   48     72    108    162   243     0                 
//          0   64   96   144    216    324   486  729   0  Right Down: *3
//          0000000000000000000000000000000000000000000000                 
//**************************QBERT**TABLE**********************\\\

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.FilePathAttribute;

public class QBert : MonoBehaviour
{
    public int LevelID;
    public float LocationID;

    Transform Blocktransform;
    public GameObject GreenBallPrefab;
    public Transform BlockSpawnPoint;

    public AudioClip[] audioClipArray;

    Vector3 InitialPos;

    static float yOffset = 0.6f;

    public List<GameObject> Blocks;

    public GameObject ElevatorA, ElevatorB;

    bool bCheckLocation, onElevatorA, onElevatorB;

    public bool ActivateCoiley, GreenBallEffect;

    bool GameIsRunning, GameOver, DeathAnimationWorking;
    bool enableInput, playonce = true;
    int whereToJump = 0, DeathAnimationOverNum;

    Animator animator;

    SpriteRenderer spriteRenderer;

    public GameObject GameOverPanel;

    public GameObject Curse;
    new Rigidbody2D rigidbody;

    public TMP_Text score, GameResultMsg;

    CapsuleCollider2D CapsuleCollider;

    AudioSource audioSource;

    [SerializeField]
    private List<GameObject> QbertHealthIcon;
    int QbertHealthIconIndex = 0;

    private string filePath;
    private List<KeyValuePair<string, int>> leaderboard;

    int Score;
    public bool playerHasWon;

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
        audioSource = GetComponent<AudioSource>();
        GameIsRunning = true;
        GameOver = false;
        Curse.SetActive(false);
        DeathAnimationWorking = true;
        DeathAnimationOverNum = 0;
        GameOverPanel.SetActive(false);
        rigidbody.isKinematic = false;
        GreenBallEffect = false;
        InitialPos = transform.position;
        onElevatorA = false;
        onElevatorB = false;
        enableInput = true;
        playerHasWon = false;
        score.text = ""+Score;
        if (audioClipArray.Length != 0)
        {
            audioSource.PlayOneShot(audioClipArray[5]);
        }
        filePath = Application.persistentDataPath + "/leaderboard.txt";
    }

    public void AddScore(int ScoreAmount) // Public Function.
    {
        Score += ScoreAmount;
        score.text = ""+Score;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfPlayerHasWon();
        if (!playerHasWon)
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
                        if (bCheckLocation) // Switch for if we want apply checklocation or not.
                        { UpdateLocation(LocationID); }

                        if (enableInput)
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
                if (playonce) // we should play the GameOver sound effect only once for Qbert.
                {
                    audioSource.PlayOneShot(audioClipArray[6]);
                    playonce = false;
                }

                if (DeathAnimationWorking)
                {
                    StartCoroutine(PlayDeathAnimation());
                }
                else
                {
                    UpdateGameStatsMsg();
                    StartCoroutine(DisplayGameOver());
                }
            }
        }
        else
        {
            UpdateGameStatsMsg();
            StartCoroutine (DisplayGameOver());
        }
    }

    void UpdateGameStatsMsg()
    {
        if (GameOver == true)
        {
            GameResultMsg.text = "Game Over.";
        }
        if (playerHasWon == true)
        {
            GameResultMsg.text = "You Won!";
        }
    }

    public bool GetGameIsRunning() // Public - Getter
    { return GameIsRunning; }

    public void SetGameIsRunning(bool condition) // Public - Setter
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
                    enableInput = true;
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
        // we should play the falling sound effect only once for Qbert.              
            audioSource.PlayOneShot(audioClipArray[3]);
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

    AudioClip ReturnRandomJumpSoundEffect()
    {
        if (UnityEngine.Random.value < 0.5f)
        {
            return audioClipArray[0];
        }
        else if (UnityEngine.Random.value < 0.5f)
        {
            return audioClipArray[1];
        }
        else
        {
            return audioClipArray[2];
        }
              
    }
    
    AudioClip ReturnRandomCurseSoundEffect()
    {
        if (UnityEngine.Random.value < 0.5f)
        {
            return audioClipArray[8];
        }
        else
        {
            return audioClipArray[9];
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
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            enableInput = false;
            if (LocationID == 16 && ElevatorA != null)
            {
                onElevatorA = true;
                animator.SetBool("OnElevatorA", onElevatorA);
                audioSource.PlayOneShot(audioClipArray[4]);
            }
            else
            {
                if (audioClipArray.Length != 0)
                {
                    audioSource.PlayOneShot(ReturnRandomJumpSoundEffect());
                }
                SetWhereToJump(1);
                animator.SetInteger("WhereToJump", whereToJump); // 1 meaning Left Up 
                LocationID /= 3;
                Debug.Log(LocationID);
                bCheckLocation = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            enableInput = false;
            if (audioClipArray.Length != 0)
            {
                audioSource.PlayOneShot(ReturnRandomJumpSoundEffect());
            }

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
            enableInput = false;
            if (audioClipArray.Length != 0)
            {
                audioSource.PlayOneShot(ReturnRandomJumpSoundEffect());
            }

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
            enableInput = false;
            if (LocationID == 81 && ElevatorB != null)
            {
                onElevatorB = true;
                animator.SetBool("OnElevatorB", onElevatorB);
                audioSource.PlayOneShot(audioClipArray[4]);
            }
            else
            {
                if (audioClipArray.Length != 0)
                {
                    audioSource.PlayOneShot(ReturnRandomJumpSoundEffect());
                }

                SetWhereToJump(4);
                animator.SetInteger("WhereToJump", whereToJump); // 4 meaning Right Up
                LocationID /= 2;
                Debug.Log(LocationID);
                bCheckLocation = true;
            }
        }
        
    }

    void CheckIfPlayerHasWon()
    {
        int BlockValue = 0;
        for (int i = 0; i < Blocks.Count; i++)
        {           
            if (Blocks[i].gameObject.GetComponent<Block>().SwitchValue != 0)
            {
                BlockValue += Blocks[i].gameObject.GetComponent<Block>().SwitchValue;
            }
            if (BlockValue == 28)
            { 
                playerHasWon = true;
            }
        }
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

    public bool GetOnElavtor() // public getter to know if Qbert is on Elevator.
    {
        if (onElevatorA == true || onElevatorB == true)
        { return true; }
        else
        { return false; }
    }

    public void ExtiElevator()
    {
        transform.position = new Vector3(Blocks[0].transform.position.x,
            Blocks[0].transform.position.y + yOffset, Blocks[0].transform.position.z); // move the Qbert to top most block. 0 in the list
        LocationID = 1;
        onElevatorA = false;
        onElevatorB = false;
        animator.SetBool("OnElevatorA", onElevatorA);
        animator.SetBool("OnElevatorB", onElevatorB);
        bCheckLocation = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (onElevatorA == false && onElevatorB == false) //  we don't care if Qbert is on Elevator.
        {
            if (collision.gameObject.tag == "Enemy")
            {
                audioSource.PlayOneShot(ReturnRandomCurseSoundEffect());
                SetGameIsRunning(false);
                ActivateCoiley = false; // de activating coiley
                Curse.SetActive(true);

                DecreamentHealth();

            }

            if (collision.gameObject.tag == "GreenBall")
            {
                // spawning a new green ball.
                audioSource.PlayOneShot(audioClipArray[7]); // greenBall Effect
                AddScore(100);
                var instantiatedObj = GameObject.Instantiate(collision.gameObject, new Vector3(0.0f, 7.0f, 0.0f), BlockSpawnPoint.rotation);
                if (instantiatedObj != null) { instantiatedObj.gameObject.tag = "GreenBall"; }
                Destroy(collision.gameObject);
                GreenBallEffect = true;
            }
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
