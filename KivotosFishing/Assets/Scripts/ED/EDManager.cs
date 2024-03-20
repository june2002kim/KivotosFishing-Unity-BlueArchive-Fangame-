using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class EDManager : MonoBehaviour
{
    [SerializeField] [TextArea(10, 10)] private string EndingText;
    [SerializeField] TextMeshProUGUI endingTMP;
    [SerializeField] GameObject stamp;
    [SerializeField] CommentPopup commentPopup;
    [SerializeField] GameObject live;
    [SerializeField] GameObject title;

    private float typingSpeed;
    private bool nowCutscene = false;

    private void Start()
    {
        PlayerPrefs.SetString("EDplayed", "hasPlayed");
    }

    private void Update()
    {
        if(commentPopup.lastComment && !nowCutscene)
        {
            nowCutscene = true;
            live.SetActive(false);
            title.SetActive(false);
            StartCoroutine(TypingText());
        }
    }

    private IEnumerator TypingText()
    {
        int idx = 0;
        endingTMP.text = "";

        while(idx < EndingText.Length)
        {
            endingTMP.text += EndingText[idx];

            if(EndingText[idx].Equals('\n'))
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

        stamp.SetActive(true);

        if(PlayerPrefs.HasKey("hasCleared"))
        {
            PlayerPrefs.SetInt("hasCleared", PlayerPrefs.GetInt("hasCleared", 0) + 1);
        }

        yield return new WaitForSeconds(4f);

        SceneManager.LoadScene("Home");
    }
}
