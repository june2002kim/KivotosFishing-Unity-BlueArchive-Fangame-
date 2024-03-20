using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AmazonManager : MonoBehaviour
{
    [Header("------References------")]
    [SerializeField] private GameObject skipButton;
    [SerializeField] private GameObject goalPanel;
    [SerializeField] private TextMeshProUGUI goalText;
    [SerializeField] private ScoreManager shirokoScoreManager;
    [SerializeField] private FisingManager fishingManager;
    [SerializeField] private QTEManager qTEManager;
    [SerializeField] private DBDManager dBDManager;
    [SerializeField] private TurboManager turboManager;
    [SerializeField] private BGMManager bgmManager;

    [Header("------Dialogue------")]
    [SerializeField] private GameObject Iori;
    [SerializeField] private GameObject runningIori;
    [SerializeField] private TextData textData;
    [SerializeField] private Image profileImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogText;

    [Header("------UGUI------")]
    [SerializeField] private Slider amazonTimer;
    [SerializeField] private float amazonMaxTime;
    [SerializeField] private float currentTime;
    [SerializeField] private TextMeshProUGUI caughtCnt;
    [SerializeField] public bool speedAudioUp = false;
    [SerializeField] private bool stopTimer;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private GameObject[] retryPanels;

    [Header("------Audios------")]
    [SerializeField] private AudioClip tadaClip;
    [SerializeField] private AudioClip failClip;
    [SerializeField] private AudioClip whistleClip;

    [Header("------Variables------")]
    [SerializeField] public bool stopFishing = false;
    [SerializeField] private int resultIdx;
    [SerializeField] private int ioriGood = 2;
    [SerializeField] private int ioriBad = 1;
    [SerializeField] private bool isAmazonFinished = false;

    private AudioSource audioSource;

    // timeKeeper
    WaitForSeconds secEpsilon = new WaitForSeconds(math.EPSILON);
    WaitForSecondsRealtime realsecEpsilon = new WaitForSecondsRealtime(math.EPSILON);

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        CheckProgress();
        CheckPlayed();

        StartCoroutine(ShowGoal());
    }

    void Update()
    {
        if(isAmazonFinished && Input.GetKeyDown(KeyCode.Space))
        {
            retryPanels[resultIdx].SetActive(true);
        }

        caughtCnt.text = shirokoScoreManager.totalCnt.ToString();
    }

    private void CheckProgress()
    {
        PlayerPrefs.SetString("Tutorialplayed", "hasCleared");
        PlayerPrefs.SetString("Lakeplayed", "hasCleared");
    }

    private void CheckPlayed()
    {
        if (PlayerPrefs.HasKey("Amazonplayed"))
        {
            if (PlayerPrefs.GetString("Amazonplayed") == "hasCleared")
            {
                skipButton.SetActive(true);
            }
        }
        else
        {
            PlayerPrefs.SetString("Amazonplayed", "isPlaying");
        }
    }

    private void ConfirmCleared()
    {
        PlayerPrefs.SetString("Amazonplayed", "hasCleared");
    }

    private IEnumerator ShowGoal()
    {
        float goalAlpha = 0f;

        Time.timeScale = 0;
        fishingManager.shirokoPhase = fishingPhase.BLOCKCASTING;

        goalPanel.SetActive(true);

        while (goalAlpha < 1)
        {
            goalText.color = new Color(1, 1, 1, goalAlpha);
            goalAlpha += 0.05f;
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

        ResetTimer();
        StartCoroutine(Timer());
    }

    private void ResetTimer()
    {
        amazonMaxTime = 360f;
        amazonTimer.maxValue = amazonMaxTime;
        currentTime = amazonMaxTime;
        stopTimer = false;
    }

    private IEnumerator Timer()
    {
        while (!stopTimer)
        {
            currentTime -= Time.deltaTime;

            //yield return new WaitForSeconds(math.EPSILON);
            yield return secEpsilon;

            if (currentTime <= amazonMaxTime * 0.1f && !speedAudioUp)
            {
                amazonTimer.fillRect.GetComponent<Image>().color = Color.red;
                speedAudioUp = true;
            }

            if (currentTime <= 0)
            {
                stopTimer = true;
            }

            if (!stopTimer)
            {
                amazonTimer.value = currentTime;
            }
        }

        if (stopTimer && currentTime <= 0)
        {
            Debug.Log("TIME OUT!");
            speedAudioUp = false;
            StartCoroutine(EndAmazon());
        }
    }

    private IEnumerator EndAmazon()
    {
        bgmManager.GetComponent<AudioSource>().pitch = 1.0f;
        // Stop Shiroko
        qTEManager.resetQTE();
        qTEManager.QTECanvas.SetActive(false);
        dBDManager.dbdCanvas.SetActive(false);
        turboManager.turboCanvas.SetActive(false);
        fishingManager.resetPhase();
        fishingManager.shirokoPhase = fishingPhase.TALKING;

        //yield return null;
        audioSource.clip = whistleClip;
        audioSource.Play();
        yield return new WaitForSeconds(1.2f);

        StartCoroutine(IoriReaction());
    }

    private IEnumerator IoriReaction()
    {
        fishingManager.resetAnimBool();

        if (shirokoScoreManager.totalCnt >= ioriGood)
        {
            resultIdx = 0;
            audioSource.clip = tadaClip;

            fishingManager.shirokoAnimator.SetBool("isIdle", false);
            fishingManager.shirokoAnimator.SetBool("isJumping", true);
            fishingManager.emojiAboveLocation.GetComponent<SpriteRenderer>().sprite = fishingManager.heartEmoji;
        }
        else if (shirokoScoreManager.totalCnt >= ioriBad)
        {
            resultIdx = 1;
            audioSource.clip = tadaClip;

            fishingManager.shirokoAnimator.SetBool("isIdle", false);
            fishingManager.shirokoAnimator.SetBool("isJumping", true);
            fishingManager.emojiAboveLocation.GetComponent<SpriteRenderer>().sprite = fishingManager.heartEmoji;
        }
        else
        {
            resultIdx = 2;
            audioSource.clip = failClip;

            fishingManager.shirokoAnimator.SetBool("isIdle", true);
            fishingManager.emojiAboveLocation.GetComponent<SpriteRenderer>().sprite = fishingManager.dizzyEmoji;
        }
        audioSource.Play();
        fishingManager.emojiAboveLocation.SetActive(true);

        // show profile image
        profileImage.sprite = textData.dialogueStrings[resultIdx].CharacterInfo.ProfileImage[textData.dialogueStrings[resultIdx].profileIndex];
        // write name
        nameText.text = textData.dialogueStrings[resultIdx].CharacterInfo.CharacterName.ToString() + " <#87CEFA><sub>" + textData.dialogueStrings[resultIdx].CharacterInfo.CharacterSchool.ToString() + "</color></sub>";
        // write text
        dialogText.text = textData.dialogueStrings[resultIdx].DialogueText.ToString();
        // emoji
        textData.dialogueStrings[resultIdx].UpEmojiLocation.GetComponent<SpriteRenderer>().sprite = textData.dialogueStrings[resultIdx].UpEmoji;

        Iori.SetActive(true);
        runningIori.SetActive(false);
        resultPanel.SetActive(true);

        yield return null;

        isAmazonFinished = true;
    }

    public void retryAmazon()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void skipAmazon()
    {
        ConfirmCleared();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
