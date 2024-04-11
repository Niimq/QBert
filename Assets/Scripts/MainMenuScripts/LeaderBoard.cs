using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
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
        DisplayLeaderboard();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void goBackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public TMP_Text leaderboardText;

    void DisplayLeaderboard()
    {
        string filePath = Application.persistentDataPath + "/leaderboard.txt";
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            string leaderboard = "";

            foreach (string line in lines)
            {
                leaderboard += line + "\n";
            }

            leaderboardText.text = leaderboard;
        }
        else
        {
            leaderboardText.text = "Leaderboard is empty.";
        }
    }
}
