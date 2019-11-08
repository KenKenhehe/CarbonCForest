using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public static bool GameIsPause = false;

    public GameObject pauseMenuUI;

    private void Start()
    {
        pauseMenuUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPause)
            {
                Resume();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        GameIsPause = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        GameIsPause = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
    }

    public void LoadMenu() {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        GameIsPause = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
