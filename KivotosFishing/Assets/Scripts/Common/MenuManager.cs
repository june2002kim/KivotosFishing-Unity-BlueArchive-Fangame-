using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject audioslider;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider volumeslider;
    [SerializeField] private GameObject journal;
    [SerializeField] private GameObject homeConfirmPanel;
    [SerializeField] private GameObject skipConfirmPanel;
    [SerializeField] private GameObject pausedPanel;
    private bool isVolumeButton;
    private bool isReadingJournal;
    private bool isGoingHome;
    private bool isSkipping;
    private bool isPaused;

    private void Start()
    {
        if(PlayerPrefs.HasKey("MasterVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetVolumeSlider();
        }

        isVolumeButton = false;
        isReadingJournal = false;
        isGoingHome = false;
        isSkipping = false;
        isPaused = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isReadingJournal)
            {
                ShowJournal();
            }
            else if(isGoingHome)
            {
                HomeNo();
            }
            else if(isSkipping)
            {
                SkipNo();
            }
            else
            {
                Pause();
            }
        }
    }

    public void SetVolumeSlider()
    {
        float volume = volumeslider.value;
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    private void LoadVolume()
    {
        volumeslider.value = PlayerPrefs.GetFloat("MasterVolume");

        SetVolumeSlider();
    }

    public void ShowVolumeSlider()
    {
        if(!isVolumeButton)
        {
            audioslider.SetActive(true);
            isVolumeButton = true;
        }
        else
        {
            audioslider.SetActive(false);
            isVolumeButton = false;
        }
    }

    public void ShowJournal()
    {
        if(!isReadingJournal)
        {
            Time.timeScale = 0;
            journal.SetActive(true);
            isReadingJournal = true;
        }
        else
        {
            Time.timeScale = 1;
            journal.SetActive(false);
            isReadingJournal = false;
        }
    }

    public void Skip()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        skipConfirmPanel.SetActive(true);
        isSkipping = true;
        Time.timeScale = 0;
    }

    public void HomeYes()
    {
        isGoingHome = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("Home");
    }

    public void HomeNo()
    {
        isGoingHome = false;
        Time.timeScale = 1;
        homeConfirmPanel.SetActive(false);
    }

    public void SkipYes()
    {
        isSkipping = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SkipNo()
    {
        isSkipping = false;
        Time.timeScale = 1;
        skipConfirmPanel.SetActive(false);
    }

    public void ShowHomeConfirmPanel()
    {
        homeConfirmPanel.SetActive(true);
        isGoingHome = true;
        Time.timeScale = 0;
    }

    public void Pause()
    {
        if(!isPaused)
        {
            isPaused = true;
            pausedPanel.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            isPaused = false;
            pausedPanel.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
