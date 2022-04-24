using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

    public static bool GameIsPause = false;

    public GameObject pauseMenuUI;
    public GameObject confirmMenu;

    bool returnToMenu = true;

    private void Start()
    {
        pauseMenuUI.SetActive(false);
        confirmMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Menu"))
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
        PlayerGeneralHandler.instance.DeactivateControl();
        GameIsPause = true;
        pauseMenuUI.SetActive(true);
        print(pauseMenuUI.GetComponentInChildren<Button>());
        pauseMenuUI.GetComponentInChildren<Button>().Select();
        Time.timeScale = 0;
    }

    public void Resume()
    {
        PlayerGeneralHandler.instance.DeactivateControl();
        GameIsPause = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
    }

    public void LoadMenu() {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        GameIsPause = false;
    }

    public void PopConfirmMenu(bool returnToMenu)
    {
        this.returnToMenu = returnToMenu;
        confirmMenu.SetActive(true);
        confirmMenu.GetComponentInChildren<Button>().Select();
        pauseMenuUI.SetActive(false);
    }

    public void RetrunToPauseMenu()
    {
        confirmMenu.SetActive(false);
        pauseMenuUI.SetActive(true);
        pauseMenuUI.GetComponentInChildren<Button>().Select();
    }

    public void SaveAndQuitGame()
    {
        if (returnToMenu)
        {
            print("Game Saved");
            LoadMenu();
        }
        else
        {
            //implement save code here
            print("game should quit now in build");
            QuitGame();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
