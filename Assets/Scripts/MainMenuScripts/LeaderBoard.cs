using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{

    public Button Back;

    // Start is called before the first frame update
    void Start()
    {
        Back.onClick.AddListener(goBackToMainMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void goBackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
