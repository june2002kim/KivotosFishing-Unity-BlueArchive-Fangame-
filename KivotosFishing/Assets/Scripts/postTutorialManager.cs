using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class postTutorialManager : MonoBehaviour
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
        if(PlayerPrefs.HasKey("Posttutorialplayed"))
        {
            skipButton.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetString("Posttutorialplayed", "hasPlayed");
        }
    }
}
