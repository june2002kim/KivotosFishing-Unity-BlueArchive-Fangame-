using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinManager : MonoBehaviour
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
    [SerializeField] private TextData textData;
    [SerializeField] private Image profileImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogText;

    [Header("------UGUI------")]
    [SerializeField] private Slider finTimer;
    [SerializeField] private float finMaxTime;
    [SerializeField] private float currentTime;
    [SerializeField] public bool speedAudioUp = false;
    [SerializeField] private bool stopTimer;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private GameObject retryPanel;

    [Header("------Audios------")]
    [SerializeField] private AudioClip tadaClip;
    [SerializeField] private AudioClip explodeClip;

    [Header("------Variables------")]
    [SerializeField] private bool isMaguroCaught = false;

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
        if(fishingManager.shirokoPhase == fishingPhase.BLOCKRECORD)
        {
            if(shirokoScoreManager.fishName == "골드 마구로" && SceneManager.GetActiveScene().name == "Fin" && !isMaguroCaught)
            {
                isMaguroCaught = true;
                StartCoroutine(HappyEnd());
            }
        }  
    }

    private void CheckProgress()
    {
        PlayerPrefs.SetString("Tutorialplayed", "hasCleared");
        PlayerPrefs.SetString("Lakeplayed", "hasCleared");
        PlayerPrefs.SetString("Seaplayed", "hasCleared");
        PlayerPrefs.SetString("Amazonplayed", "hasCleared");
        PlayerPrefs.SetString("Iceplayed", "hasCleared");
    }

    private void CheckPlayed()
    {
        if (PlayerPrefs.HasKey("Finplayed"))
        {
            if (PlayerPrefs.GetString("Finplayed") == "hasCleared")
            {
                skipButton.SetActive(true);
            }
        }
        else
        {
            PlayerPrefs.SetString("Finplayed", "isPlaying");
        }
    }

    private void ConfirmCleared()
    {
        PlayerPrefs.SetString("Finplayed", "hasCleared");
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
        finMaxTime = 360f;
        finTimer.maxValue = finMaxTime;
        currentTime = finMaxTime;
        stopTimer = false;
    }

    private IEnumerator Timer()
    {
        while (!stopTimer)
        {
            currentTime -= Time.deltaTime;

            //yield return new WaitForSeconds(math.EPSILON);
            yield return secEpsilon;

            if (currentTime <= finMaxTime * 0.1f && !speedAudioUp)
            {
                finTimer.fillRect.GetComponent<Image>().color = Color.red;
                speedAudioUp = true;
            }

            if (currentTime <= 0)
            {
                stopTimer = true;
            }

            if (!stopTimer)
            {
                finTimer.value = currentTime;
            }
        }

        if (stopTimer && currentTime <= 0)
        {
            Debug.Log("TIME OUT!");
            speedAudioUp = false;
            StartCoroutine(SadEnd());
        }
    }

    private IEnumerator HappyEnd()
    {
        bgmManager.GetComponent<AudioSource>().pitch = 1.0f;
        // Stop Shiroko
        qTEManager.resetQTE();
        qTEManager.QTECanvas.SetActive(false);
        dBDManager.dbdCanvas.SetActive(false);
        turboManager.turboCanvas.SetActive(false);
        fishingManager.resetPhase();
        fishingManager.shirokoPhase = fishingPhase.TALKING;

        audioSource.clip = tadaClip;
        audioSource.Play();

        fishingManager.resetAnimBool();
        fishingManager.shirokoAnimator.SetBool("isIdle", false);
        fishingManager.shirokoAnimator.SetBool("isJumping", true);
        fishingManager.emojiAboveLocation.GetComponent<SpriteRenderer>().sprite = fishingManager.heartEmoji;
        fishingManager.emojiAboveLocation.SetActive(true);

        yield return new WaitForSeconds(2.5f);

        skipFin();
    }

    private IEnumerator SadEnd()
    {
        bgmManager.GetComponent<AudioSource>().pitch = 1.0f;
        // Stop Shiroko
        qTEManager.resetQTE();
        qTEManager.QTECanvas.SetActive(false);
        dBDManager.dbdCanvas.SetActive(false);
        turboManager.turboCanvas.SetActive(false);
        fishingManager.resetPhase();
        fishingManager.shirokoPhase = fishingPhase.TALKING;

        audioSource.clip = explodeClip;
        audioSource.Play();

        fishingManager.resetAnimBool();
        fishingManager.shirokoAnimator.SetBool("isIdle", true);
        fishingManager.emojiAboveLocation.GetComponent<SpriteRenderer>().sprite = fishingManager.dizzyEmoji;
        fishingManager.emojiAboveLocation.SetActive(true);

        // show profile image
        profileImage.sprite = textData.dialogueStrings[0].CharacterInfo.ProfileImage[textData.dialogueStrings[0].profileIndex];
        // write name
        nameText.text = textData.dialogueStrings[0].CharacterInfo.CharacterName.ToString() + " <#87CEFA><sub>" + textData.dialogueStrings[0].CharacterInfo.CharacterSchool.ToString() + "</color></sub>";
        // write text
        dialogText.text = textData.dialogueStrings[0].DialogueText.ToString();
        resultPanel.SetActive(true);

        yield return new WaitForSeconds(1.2f);

        retryPanel.SetActive(true);
    }

    public void retryFin()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void skipFin()
    {
        ConfirmCleared();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
