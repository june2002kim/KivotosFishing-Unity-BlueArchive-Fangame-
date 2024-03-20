using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [Header("------References------")]
    [SerializeField] private GameObject skipButton;
    [SerializeField] private GameObject goalPanel;
    [SerializeField] private TextMeshProUGUI goalText;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private FisingManager fishingManager;
    [SerializeField] private BGMManager bgmManager;

    [Header("------Audios------")]
    [SerializeField] private AudioClip tadaClip;

    private AudioSource audioSource;

    // timeKeeper
    WaitForSecondsRealtime realsecEpsilon = new WaitForSecondsRealtime(math.EPSILON);

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        CheckPlayed();

        StartCoroutine(ShowGoal());
    }

    void Update()
    {
        if(fishingManager.shirokoPhase == fishingPhase.BLOCKRECORD)
        {
            if(scoreManager.totalCnt == 1 && SceneManager.GetActiveScene().name == "Tutorial")
            {
                scoreManager.totalCnt++;
                StartCoroutine(EndTutorial());
            }
        }    
    }

    private void CheckPlayed()
    {
        if(PlayerPrefs.HasKey("Tutorialplayed"))
        {
            if(PlayerPrefs.GetString("Tutorialplayed") == "hasCleared")
            {
                skipButton.SetActive(true);
            }
        }
        else
        {
            PlayerPrefs.SetString("Tutorialplayed", "isPlaying");
        }
    }

    private void ConfirmCleared()
    {
        PlayerPrefs.SetString("Tutorialplayed", "hasCleared");
        Debug.Log(PlayerPrefs.GetString("Tutorialplayed").ToString());
    }

    private IEnumerator ShowGoal()
    {
        float goalAlpha = 0f;

        Time.timeScale = 0;
        fishingManager.shirokoPhase = fishingPhase.BLOCKCASTING;

        goalPanel.SetActive(true);

        while(goalAlpha < 1)
        {
            goalText.color = new Color(1, 1, 1, goalAlpha);
            goalAlpha+=0.05f;
            //Debug.Log("alpha changed");

            //yield return new WaitForSecondsRealtime(math.EPSILON);
            yield return realsecEpsilon;
        }

        //Debug.Log("start waiting");
        yield return new WaitForSecondsRealtime(3f);
        //Debug.Log("end waiting");

        goalPanel.SetActive(false);

        Time.timeScale = 1;
        fishingManager.shirokoPhase = fishingPhase.BEFORECASTING;
        bgmManager.GetComponent<AudioSource>().Play();
    }

    private IEnumerator EndTutorial()
    {
        ConfirmCleared();

        yield return new WaitForSecondsRealtime(2f);

        audioSource.clip = tadaClip;
        audioSource.Play();

        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(1.5f);

        Time.timeScale = 1;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
