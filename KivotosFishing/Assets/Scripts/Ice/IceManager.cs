using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
//using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IceManager : MonoBehaviour
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

    [Header("------UGUI------")]
    [SerializeField] private GameObject retryPanel;

    [Header("------Audios------")]
    [SerializeField] private AudioClip tadaClip;
    [SerializeField] private AudioClip loseClip;

    private AudioSource audioSource;

    // timeKeeper
    WaitForSecondsRealtime realsecEpsilon = new WaitForSecondsRealtime(math.EPSILON);

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
        PlayerPrefs.SetString("Lakeplayed", "hasCleared");
        PlayerPrefs.SetString("Amazonplayed", "hasCleared");
        PlayerPrefs.SetString("Iceplayed", "hasCleared");
    }

    private void CheckPlayed()
    {
        if (PlayerPrefs.HasKey("Iceplayed"))
        {
            if (PlayerPrefs.GetString("Iceplayed") == "hasCleared")
            {
                skipButton.SetActive(true);
            }
        }
        else
        {
            PlayerPrefs.SetString("Iceplayed", "isPlaying");
        }
    }

    private void ConfirmCleared()
    {
        PlayerPrefs.SetString("Iceplayed", "hasCleared");
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

    public IEnumerator EndIce()
    {
        // Stop Shiroko
        qTEManager.resetQTE();
        qTEManager.QTECanvas.SetActive(false);
        dBDManager.dbdCanvas.SetActive(false);
        turboManager.turboCanvas.SetActive(false);
        fishingManager.resetPhase();
        fishingManager.shirokoPhase = fishingPhase.TALKING;

        ConfirmCleared();

        yield return null;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public IEnumerator ShowRetryPanel()
    {
        // Stop Shiroko
        qTEManager.resetQTE();
        qTEManager.QTECanvas.SetActive(false);
        dBDManager.dbdCanvas.SetActive(false);
        turboManager.turboCanvas.SetActive(false);
        fishingManager.resetPhase();
        fishingManager.shirokoPhase = fishingPhase.TALKING;

        audioSource.clip = loseClip;
        audioSource.Play();

        retryPanel.SetActive(true);
        yield return null;
    }

    public void RetryIce()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
