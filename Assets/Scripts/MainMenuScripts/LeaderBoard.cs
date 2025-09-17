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
        //    leaderboard = new list<keyvaluepair<string, int>>();

        //    if (file.exists(filepath))
        //    {
        //        string[] lines = file.readalllines(filepath);

        //        foreach (string line in lines)
        //        {
        //            string[] parts = line.split(',');
        //            if (parts.length == 2)
        //            {
        //                string name = parts[0];
        //                int score;
        //                if (int32.tryparse(parts[1], out score))
        //                {
        //                    leaderboard.add(new keyvaluepair<string, int>(name, score));
        //                }
        //                else
        //                {
        //                    debug.logwarning("invalid score format in line: " + line);
        //                }
        //            }
        //            else
        //            {
        //                debug.logwarning("invalid line format: " + line);
        //            }
        //        }

        //        // sort the leaderboard by score in descending order
        //        leaderboard = leaderboard.orderbydescending(x => x.value).tolist();
        //    }
        //}
        List<KeyValuePair<string, int>> leaderboard = new List<KeyValuePair<string, int>>();
        using (StreamReader sr = new StreamReader("LeaderBoard.txt"))
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
}
