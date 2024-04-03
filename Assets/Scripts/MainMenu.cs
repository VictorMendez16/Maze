using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    private void Start()
    {
        // Make sure the cursos is locked in the middle of the screen and not visible
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlayGame()
    {
        // Load Scene by name
        // SceneManager.LoadScene("MazeScene");
        // Load Scene by index
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitScene(){
        // Application.Quit() only work on build. For it to work EditorApplication.isPlaying need to be set
        Application.Quit(); 
    }
}
