using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject Pausepanel;

    public Button Resume, MainMenu, Exit;

    // Start is called before the first frame update
    void Start()
    {
        Pausepanel.SetActive(false);
        Resume.onClick.AddListener(Continue);
        MainMenu.onClick.AddListener(LoadMainMenu);
        Exit.onClick.AddListener(Quit);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInput();    
    }

    void UpdateInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("p Pressed");
            Pause();
        }
    }

    public void Pause()
    { 
        Pausepanel.SetActive(true);
        Time.timeScale = 0.0f;
    }
    
    public void Continue()
    { 
        Pausepanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void Quit()
    {
        // save any game data here
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
    }
}
