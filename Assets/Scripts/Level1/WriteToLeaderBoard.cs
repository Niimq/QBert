using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class WriteToLeaderBoard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void AddScoreToLeaderBoard()
    //{
    //    using (var sw = new StreamWriter(filePath, true))
    //    {
    //        sw.WriteLine(txtInput.text + "," + Score);
    //    }
    //}

    //public void LoadLeaderboard()
    //{
    //    leaderboard = new List<KeyValuePair<string, int>>();

    //    if (File.Exists(filePath))
    //    {
    //        string[] lines = File.ReadAllLines(filePath);

    //        foreach (string line in lines)
    //        {
    //            string[] parts = line.Split(',');
    //            if (parts.Length == 2)
    //            {
    //                string name = parts[0];
    //                int score;
    //                if (Int32.TryParse(parts[1], out score))
    //                {
    //                    leaderboard.Add(new KeyValuePair<string, int>(name, score));
    //                }
    //                else
    //                {
    //                    Debug.LogWarning("Invalid score format in line: " + line);
    //                }
    //            }
    //            else
    //            {
    //                Debug.LogWarning("Invalid line format: " + line);
    //            }
    //        }

    //        // Sort the leaderboard by score in descending order
    //        leaderboard = leaderboard.OrderByDescending(x => x.Value).ToList();
    //    }
    //}
}
