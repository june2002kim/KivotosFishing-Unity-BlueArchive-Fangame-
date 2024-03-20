using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
//using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SeaManager : MonoBehaviour
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
    [SerializeField] private GameObject Hanako;
    [SerializeField] private GameObject swimmingHanako;
    [SerializeField] private TextData textData;
    [SerializeField] private Image profileImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogText;

    [Header("------UGUI------")]
    [SerializeField] private TextMeshProUGUI caughtCnt;
    [SerializeField] private GameObject resultPanel;

    [Header("------Audios------")]
    [SerializeField] private AudioClip tadaClip;

    [Header("------Variables------")]
    [SerializeField] public bool stopFishing = false;
    [SerializeField] private int resultIdx;
    [SerializeField] private int hanakoGood;
    [SerializeField] private int hanakoBad;
    [SerializeField] private bool isSeaFinished = false;
    [SerializeField] private bool isHaniwaCaught = false;

    private AudioSource audioSource;

    // timeKeeper
    WaitForSecondsRealtime realsecEpsilon = new WaitForSecondsRealtime(math.EPSILON);

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        CheckProgress();
        CheckPlayed();

        swimmingHanako.SetActive(true);

        StartCoroutine(ShowGoal());
    }

    void Update()
    {
        if(fishingManager.shirokoPhase == fishingPhase.BLOCKRECORD)
        {
            if(shirokoScoreManager.fishName == "수정 하니와 물풍선" && SceneManager.GetActiveScene().name == "Sea" && !isHaniwaCaught)
            {
                isHaniwaCaught = true;
                StartCoroutine(EndSea());
            }
        }  

        if(isSeaFinished && Input.GetKeyDown(KeyCode.Space))
        {
            ConfirmCleared();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        caughtCnt.text = shirokoScoreManager.totalCnt.ToString();
    }

    private void CheckProgress()
    {
        PlayerPrefs.SetString("Tutorialplayed", "hasCleared");
        PlayerPrefs.SetString("Lakeplayed", "hasCleared");
        PlayerPrefs.SetString("Amazonplayed", "hasCleared");
    }

    private void CheckPlayed()
    {
        if (PlayerPrefs.HasKey("Seaplayed"))
        {
            if (PlayerPrefs.GetString("Seaplayed") == "hasCleared")
            {
                skipButton.SetActive(true);
            }
        }
        else
        {
            PlayerPrefs.SetString("Seaplayed", "isPlaying");
        }
    }

    private void ConfirmCleared()
    {
        PlayerPrefs.SetString("Seaplayed", "hasCleared");
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
    }

    private IEnumerator EndSea()
    {
        // Stop Shiroko
        qTEManager.resetQTE();
        qTEManager.QTECanvas.SetActive(false);
        dBDManager.dbdCanvas.SetActive(false);
        turboManager.turboCanvas.SetActive(false);
        fishingManager.resetPhase();
        fishingManager.shirokoPhase = fishingPhase.TALKING;

        yield return null;
        StartCoroutine(HanakoReaction());
    }

    private IEnumerator HanakoReaction()
    {
        fishingManager.resetAnimBool();

        if (shirokoScoreManager.totalCnt <= hanakoGood)
        {
            resultIdx = 0;
        }
        else if (shirokoScoreManager.totalCnt <= hanakoBad)
        {
            resultIdx = 1;
        }
        else
        {
            resultIdx = 2;
        }
        audioSource.clip = tadaClip;
        audioSource.Play();
        fishingManager.shirokoAnimator.SetBool("isIdle", false);
        fishingManager.shirokoAnimator.SetBool("isJumping", true);
        fishingManager.emojiAboveLocation.GetComponent<SpriteRenderer>().sprite = fishingManager.heartEmoji;
        fishingManager.emojiAboveLocation.SetActive(true);

        // show profile image
        profileImage.sprite = textData.dialogueStrings[resultIdx].CharacterInfo.ProfileImage[textData.dialogueStrings[resultIdx].profileIndex];
        // write name
        nameText.text = textData.dialogueStrings[resultIdx].CharacterInfo.CharacterName.ToString() + " <#87CEFA><sub>" + textData.dialogueStrings[resultIdx].CharacterInfo.CharacterSchool.ToString() + "</color></sub>";
        // write text
        dialogText.text = textData.dialogueStrings[resultIdx].DialogueText.ToString();
        // emoji
        textData.dialogueStrings[resultIdx].UpEmojiLocation.GetComponent<SpriteRenderer>().sprite = textData.dialogueStrings[resultIdx].UpEmoji;

        Hanako.SetActive(true);
        swimmingHanako.SetActive(false);
        resultPanel.SetActive(true);

        yield return new WaitForSeconds(2f);

        isSeaFinished = true;
    }

    public void skipAmazon()
    {
        ConfirmCleared();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
