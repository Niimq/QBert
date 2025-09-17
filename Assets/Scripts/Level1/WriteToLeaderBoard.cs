using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class WriteToLeaderBoard : MonoBehaviour
{
    TMP_InputField InputField;
    TMP_Text Score;
   

    // Start is called before the first frame update
    void Start()
    {
        InputField = GetComponent<TMP_InputField>();
        InputField.onEndEdit.AddListener(addscoretoleaderboard);

        Score = GameObject.Find("Score")?.GetComponent<TMP_Text>();
        
        if (Score == null) 
        {
            Debug.LogError("Score TMP_Text could not be found in the scene!"); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addscoretoleaderboard(string s)
    {
        string filePath = Path.Combine(Application.dataPath, "LeaderBoard.txt");
        using (StreamWriter sw = new StreamWriter(filePath, true))
        {
            sw.WriteLine(s + ',' + Score.text);
        }
    }
}
