using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    //singleton
    public static UIManager Instance { get; private set; }
    public Tools tools;
    public Button SettingsButton;
    public GameObject StartButton;
    public GameObject CreditsButton;
    public GameObject ExitButton;
    public GameObject fadeImage;
    public GameObject creditsPanel;
    public GameObject pauseMenu;
    public GameObject plantingToolUI;
    public GameObject wateringToolUI;
    public Slider volumeSlider;
    public GameObject[] equapmentBar;
    public bool isTransitioning;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    void Start()
    {
        if(volumeSlider != null)
        {
            volumeSlider.value = AudioManager.Instance.globalVolume;
        }
    }

    public void StartGame()
    {
        fadeImage.SetActive(true);
        GetComponent<SceneTransition>().LoadScene("GameScene");
    }

    public void RestartGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
        Time.timeScale = 1;
        fadeImage.SetActive(true);
        GetComponent<SceneTransition>().LoadScene("GameScene");
    }

    public void ShowCredits()
    {
        creditsPanel.SetActive(true);
        StartButton.SetActive(false);
        CreditsButton.SetActive(false);
        ExitButton.SetActive(false);
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
        StartButton.SetActive(true);
        CreditsButton.SetActive(true);
        ExitButton.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        foreach (GameObject item in equapmentBar)
        {
            item.SetActive(false);
        }   
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
        foreach (GameObject item in equapmentBar)
        {
            item.SetActive(true);
        }
        
    }

    public void ReturnMainMenu()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        GetComponent<SceneTransition>().LoadScene("MainMenu");
    }

    public void SwitchToPlantingTool()
    {
        tools.isPlantingTool = true;
        tools.isWateringTool = false;
        plantingToolUI.SetActive(true);
        wateringToolUI.SetActive(false);
    }

    public void SwitchToWateringTool()
    {
        tools.isPlantingTool = false;
        tools.isWateringTool = true;
        plantingToolUI.SetActive(false);
        wateringToolUI.SetActive(true);
    }

    public void SetVolume()
    {
        AudioManager.Instance.SetGlobalVolume(volumeSlider.value);
    }
}
