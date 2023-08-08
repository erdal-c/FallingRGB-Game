using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text Score;
    public Text MultipleFactor;
    public Text HighScoreText;
    public Text RecordText;

    public GameObject InGamePanel;
    public GameObject MainMenuPanel;
    public GameObject DeathMenu;
    public GameObject PauseMenu;

    public Slider DashSlider;

    public bool isGameStart;

    GameManager gameManager;
    PlayerController playerController;
    BackgroundManager backgroundManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        backgroundManager = FindObjectOfType<BackgroundManager>();
        HighScoreText.text = "High Score : " + PlayerPrefs.GetFloat("HighScore").ToString("0");

        playerController = FindObjectOfType<PlayerController>();
        PlayerController.PlayerChange += PlayerChanged; // Registering Action in PlayerController
    }

    void Update()
    {
        if (InGamePanel.activeSelf)
        {
            Score.text = gameManager.Point.ToString("0");
            MultipleFactor.text = gameManager.multiplefactor.ToString() + "x";
        }
    }
    private void FixedUpdate()
    {
        DashSliderControll();
    }

    void DashSliderControll()
    {
        if (playerController.isDash)
        {
            DashSlider.value -= 0.1f;
        }
        else
        {
            DashSlider.value += 0.02f;
        }
    }

    public void PlayButtonPressed()
    {
        MainMenuPanel.SetActive(false);
        InGamePanel.SetActive(true);
        PauseMenu.SetActive(false);
        DeathMenu.SetActive(false);
        isGameStart = true;
        Time.timeScale = 1;
    }

    public void RestartButtonPressed()
    {
        playerController.GetComponent<PlayerController>().enabled = false;
        gameManager.GetComponent<GameManager>().enabled = false;
        backgroundManager.GetComponent<BackgroundManager>().enabled = false;
        PlayButtonPressed();
        playerController.GetComponent<PlayerController>().enabled = true;
        gameManager.GetComponent<GameManager>().enabled = true;
        backgroundManager.GetComponent<BackgroundManager>().enabled = true;

        playerController.GetComponent<DimensionEdit>().MeshDimensionReset();
        playerController.GetComponent<DimensionEdit>().MeshDimensionEdit();
    }


    public void ExitButtonPressed()
    {
        Application.Quit();
    }

    public void MainMenuButtonPressed()
    {
        SceneManager.LoadScene(0);
        MainMenuPanel.SetActive(true);
        InGamePanel.SetActive(false);
        isGameStart = false;
        Time.timeScale = 1;
    }

    public void PauseMenuButtonPressed()
    {
        if (!PauseMenu.activeSelf)
        {
            Time.timeScale = 0;
            PauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            PauseMenu.SetActive(false);
        }
    }

    void PlayerChanged(PlayerController playerObject)
    {
        playerController = playerObject;
    }
}
