using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button Play, LeaderBoard, Quit;

    public void Start()
    {
        Play.onClick.AddListener(LoadGame);
        LeaderBoard.onClick.AddListener(LoadLeaderBoard);
        Quit.onClick.AddListener(OnApplicationQuit);
    }
    private void Update()
    {
        
    }

    private void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    private void LoadLeaderBoard()
    {
        SceneManager.LoadScene(2);
    }

    private void OnApplicationQuit()
    {
        Application.Quit(0);
    }
}