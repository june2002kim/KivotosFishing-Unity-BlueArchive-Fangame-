using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneManager : MonoBehaviour
{
    [SerializeField] DialogueManager dialogueManager;
    [SerializeField] GameObject skipButton;

    void Start()
    {
        CheckPlayed();
    }

    void Update()
    {
        if(dialogueManager.lastLine)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private void CheckPlayed()
    {
        if(PlayerPrefs.HasKey(SceneManager.GetActiveScene().name))
        {
            skipButton.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetString(SceneManager.GetActiveScene().name, "hasPlayed");
        }
    }
}
