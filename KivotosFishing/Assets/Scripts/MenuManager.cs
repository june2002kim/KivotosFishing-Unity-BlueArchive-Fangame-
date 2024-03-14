using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject audioslider;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider volumeslider;
    [SerializeField] private GameObject journal;
    private bool isVolumeButton;
    private bool isJournalButton;

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
        isJournalButton = false;
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
        if(!isJournalButton)
        {
            journal.SetActive(true);
            isJournalButton = true;
        }
        else
        {
            journal.SetActive(false);
            isJournalButton = false;
        }
    }

    public void Skip()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Home()
    {
        SceneManager.LoadScene("Home");
    }
}
