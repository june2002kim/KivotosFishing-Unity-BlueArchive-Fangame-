using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class OPManager : MonoBehaviour
{
    [SerializeField] private GameObject skipButton;
    [SerializeField] [TextArea(10, 10)] private string openingText;
    [SerializeField] TMP_Text openingTMP;
    [SerializeField] GameObject nextButton;

    private float typingSpeed;
    private bool endTalking = false;

    private void Start()
    {
        CheckPlayed();
        
        StartCoroutine(TypingText());
    }

    private void Update()
    {
        if((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && endTalking)
        {
            SceneManager.LoadScene("Tutorial");
        }
    }

    private void CheckPlayed()
    {
        if(PlayerPrefs.HasKey("OPplayed"))
        {
            skipButton.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetString("OPplayed", "hasPlayed");
        }
    }

    private IEnumerator TypingText()
    {
        int idx = 0;
        openingTMP.text = "";

        while(idx < openingText.Length)
        {
            openingTMP.text += openingText[idx];

            if(openingText[idx].Equals('\n'))
            {
                //Debug.Log("... detected ...");
                typingSpeed = 0.5f;
            }
            else
            {
                typingSpeed = 0.1f;
            }

            idx++;

            yield return new WaitForSeconds(typingSpeed);
        }

        endTalking = true;

        nextButton.SetActive(true);
    }
}
