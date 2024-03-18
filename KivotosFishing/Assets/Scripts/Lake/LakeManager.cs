using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LakeManager : MonoBehaviour
{
    [Header("------References------")]
    [SerializeField] private GameObject skipButton;
    [SerializeField] private GameObject goalPanel;
    [SerializeField] private TextMeshProUGUI goalText;
    [SerializeField] private ScoreManager shirokoScoreManager;
    [SerializeField] private MiyuFisingMan miyuFisingMan;
    [SerializeField] private FisingManager fishingManager;
    [SerializeField] private QTEManager qTEManager;
    [SerializeField] private DBDManager dBDManager;
    [SerializeField] private TurboManager turboManager;
    [SerializeField] private BGMManager bgmManager;

    [Header("------UGUI------")]
    [SerializeField] private Slider lakeTimer;
    [SerializeField] private float lakeMaxTime;
    [SerializeField] private float currentTime;
    [SerializeField] public bool speedAudioUp = false;
    [SerializeField] private bool stopTimer;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TextMeshProUGUI shirokoScoreText;
    [SerializeField] private TextMeshProUGUI miyuScoreText;
    [SerializeField] private GameObject losePanel;

    [Header("------Audios------")]
    [SerializeField] private AudioClip waitClip;
    [SerializeField] private AudioClip tadaClip;
    [SerializeField] private AudioClip failClip;
    [SerializeField] private AudioClip whistleClip;

    [Header("------Variables------")]
    [SerializeField] public bool stopFishing = false;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        CheckProgress();
        CheckPlayed();

        StartCoroutine(ShowGoal());
    }

    private void CheckProgress()
    {
        PlayerPrefs.SetString("Tutorialplayed", "hasCleared");
    }

    private void CheckPlayed()
    {
        if(PlayerPrefs.HasKey("Lakeplayed"))
        {
            if(PlayerPrefs.GetString("Lakeplayed") == "hasCleared")
            {
                skipButton.SetActive(true);
            }
        }
        else
        {
            PlayerPrefs.SetString("Lakeplayed", "isPlaying");
        }
    }

    private void ConfirmCleared()
    {
        PlayerPrefs.SetString("Lakeplayed", "hasCleared");
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
            Debug.Log("alpha changed");

            yield return new WaitForSecondsRealtime(math.EPSILON);
        }

        Debug.Log("start waiting");
        yield return new WaitForSecondsRealtime(3f);
        Debug.Log("end waiting");

        goalPanel.SetActive(false);

        Time.timeScale = 1;
        fishingManager.shirokoPhase = fishingPhase.BEFORECASTING;
        bgmManager.GetComponent<AudioSource>().Play();

        ResetTimer();
        StartCoroutine(Timer());
    }

    private void ResetTimer()
    {
        lakeMaxTime = 30f;
        lakeTimer.maxValue = lakeMaxTime;
        currentTime = lakeMaxTime;
        stopTimer = false;
    }

    private IEnumerator Timer()
    {
        while(!stopTimer)
        {
            currentTime -= Time.deltaTime;
            yield return new WaitForSeconds(math.EPSILON);

            if(currentTime <= lakeMaxTime * 0.1f && !speedAudioUp)
            {
                lakeTimer.fillRect.GetComponent<Image>().color = Color.red;
                speedAudioUp = true;
            }

            if(currentTime <= 0)
            {
                stopTimer = true;
            }

            if(!stopTimer)
            {
                lakeTimer.value = currentTime;
            }
        }

        if(stopTimer && currentTime<=0)
        {
            Debug.Log("TIME OUT!");
            speedAudioUp = false;
            StartCoroutine(EndLake());
        }
    }

    private IEnumerator EndLake()
    {
        bgmManager.GetComponent<AudioSource>().pitch = 1.0f;
        // Stop Shiroko
        qTEManager.resetQTE();
        qTEManager.QTECanvas.SetActive(false);
        dBDManager.dbdCanvas.SetActive(false);
        turboManager.turboCanvas.SetActive(false);
        fishingManager.resetPhase();
        fishingManager.shirokoPhase = fishingPhase.TALKING;
        // Stop Miyu
        stopFishing = true;
        miyuFisingMan.resetAnimBool();

        //yield return null;
        audioSource.clip = whistleClip;
        audioSource.Play();
        yield return new WaitForSeconds(1.2f);

        StartCoroutine(RevealScore(shirokoScoreManager.totalScore, miyuFisingMan.MiyuScore));
    }

    private IEnumerator RevealScore(int shirokoscore, int miyuscore)
    {
        int shirokoCount = 0;
        int miyuCount = 0;

        resultPanel.SetActive(true);

        audioSource.clip = waitClip;
        audioSource.Play();

        while(shirokoCount <= shirokoscore || miyuCount <= miyuscore)
        {
            if(shirokoCount <= shirokoscore)
            {
                shirokoScoreText.text = shirokoCount.ToString();
                shirokoCount+=10;
            }
            if(miyuCount <= miyuscore)
            {
                miyuScoreText.text = miyuCount.ToString();
                miyuCount+=10;
            }

            yield return new WaitForSeconds(math.EPSILON);
        }

        yield return new WaitForSeconds(1.5f);

        if(shirokoCount >= miyuCount)
        {
            // Shiroko Wins!
            audioSource.clip = tadaClip;
            audioSource.Play();
            shirokoScoreText.GetComponent<RectTransform>().localScale = new Vector2(2f, 2f);

            fishingManager.resetAnimBool();
            fishingManager.shirokoAnimator.SetBool("isIdle", false);
            fishingManager.shirokoAnimator.SetBool("isJumping", true);
            fishingManager.emojiAboveLocation.GetComponent<SpriteRenderer>().sprite = fishingManager.heartEmoji;
            fishingManager.emojiAboveLocation.SetActive(true);

            miyuFisingMan.emojiAboveLocation.GetComponent<SpriteRenderer>().sprite = miyuFisingMan.dizzyEmoji;
            miyuFisingMan.emojiAboveLocation.SetActive(true);
        }
        else
        {
            // Shiroko Lose...
            audioSource.clip = failClip;
            audioSource.Play();
            miyuScoreText.GetComponent<RectTransform>().localScale = new Vector2(2f, 2f);

            fishingManager.emojiAboveLocation.GetComponent<SpriteRenderer>().sprite = fishingManager.dizzyEmoji;
            fishingManager.emojiAboveLocation.SetActive(true);

            miyuFisingMan.emojiAboveLocation.GetComponent<SpriteRenderer>().sprite = miyuFisingMan.heartEmoji;
            miyuFisingMan.emojiAboveLocation.SetActive(true);
        }

        yield return new WaitForSeconds(2f);

        if(shirokoCount >= miyuCount)
        {
            // Shiroko Wins!
            ConfirmCleared();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            // Shiroko Lose...
            losePanel.SetActive(true);
        }
    }

    public void retryLake()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
