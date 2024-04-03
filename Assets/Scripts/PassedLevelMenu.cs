using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PassedLevelMenu : MonoBehaviour
{
    // MazeGenerator.mazeWidth = MazeGenerator.mazeWidth + 4; 
    // MazeGenerator.mazeHeight = MazeGenerator.mazeHeight + 4;
    public void NextLevel()
    {
        // Load Scene by name
        // SceneManager.LoadScene("MazeScene");
        // Load Scene by index
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void NextLevelAfterDead()
    {
        // Load Scene by name
        // SceneManager.LoadScene("MazeScene");
        // Load Scene by index
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }

    public void BackToMenu()
    {
        // Load Scene by index
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }

    public void BackToMenuAfterDead()
    {
        // Load Scene by index
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 3);
    }

    public void QuitScene()
    {
        // Application.Quit() only work on build. For it to work EditorApplication.isPlaying need to be set
        Application.Quit();
    }
}
