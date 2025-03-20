using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set;}
    public int frameRate;
    public float gameTimescale;

    void Awake() {
        if (Instance != null) {
            Debug.LogWarning("More than one GameManager in scene");
        }

        Instance = this;
    }
    
    void Start()
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Kunai"), LayerMask.NameToLayer("Kunai"));
    }

    void Update() {
        Application.targetFrameRate = frameRate;

        if (Input.GetKeyDown(KeyCode.Escape)
        && SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Menu"))
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        if(!gameObject.GetComponent<CanvasManager>().pauseMenu.activeSelf) {
            gameTimescale = Time.timeScale;
            gameObject.GetComponent<CanvasManager>().pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }

        else {
            gameObject.GetComponent<CanvasManager>().pauseMenu.SetActive(false);
            Time.timeScale = gameTimescale;
        }
    }

    public void StartGame() {
        SceneManager.LoadScene("Level 1");
    }

    public void GoToMenu() {
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void RestartLevel() {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
