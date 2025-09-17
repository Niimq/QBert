using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{
    
    public Button Back, reset;

    // Start is called before the first frame update
    void Start()
    {
        Back.onClick.AddListener(goBackToMainMenu);
        reset.onClick.AddListener(ResetLeaderboard);
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
        string filePath = Path.Combine(Application.dataPath, "LeaderBoard.txt");
        Debug.Log("Leaderboard file path: " + filePath);

        if (!File.Exists(filePath))
        {
            leaderboardText.text = "Leaderboard is empty.";
            return;
        }

        List<KeyValuePair<string, int>> leaderboard = new List<KeyValuePair<string, int>>();
        using (StreamReader sr = new StreamReader(filePath))
        {
            string line;
            string[] Parts;
            while ((line = sr.ReadLine()) != null)
            {
                Parts = line.Split(',');
                if (Parts.Length == 2) 
                { 
                    string name = Parts[0];
                    int score;
                    if (int.TryParse(Parts[1], out score))
                    {
                        leaderboard.Add(new KeyValuePair<string, int>(name, score));
                    }
                    else
                    {
                        Debug.LogWarning("Invalid score format: " + line);
                    }
                }
                else
                {
                    Debug.LogWarning("Invalid line format: " + line);
                }
            }
            // order the leaderBoard/ Lambda funcion for each x element in the leaderboard sort the list by x.value which is the score.
            leaderboard = leaderboard.OrderByDescending(x => x.Value).ToList();

            // add each line in leaderboard to a single string with newlines
            if (leaderboard.Count > 0)
            {
                string displayText = "";
                int i = 0;
                foreach (var x in leaderboard)
                {
                    i++;
                    displayText += i + ". "+ x.Key + " Score: " + x.Value + "\n";
                }
                leaderboardText.text = displayText;
            }
            else
            {
                leaderboardText.text = "Leaderboard is empty.";
            }
        }
    }

    public void ResetLeaderboard()
    {
        string filePath = Path.Combine(Application.dataPath, "LeaderBoard.txt");

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        // update UI immediately
        leaderboardText.text = "Leaderboard is empty.";
    }
}
