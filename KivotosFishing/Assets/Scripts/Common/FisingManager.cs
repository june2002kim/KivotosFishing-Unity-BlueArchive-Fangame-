using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum fishingPhase
{
    BLOCKCASTING,
    BEFORECASTING,
    WAITING,
    CATCHING,
    ENTERGACHA,
    BLOCKGACHA,
    ENTERQTE,
    BLOCKQTE,
    ENTERDBD,
    BLOCKDBD,
    ENTERTURBO,
    BLOCKTURBO,
    ENTERRECORD,
    BLOCKRECORD,
    TALKING
}

public class FisingManager : MonoBehaviour
{
    [Header("------References------")]
    [SerializeField] private GachaManager gachaManager;
    [SerializeField] private PenguinSpawner penguinSpawner;

    [Header("------Variables------")]
    public fishingPhase shirokoPhase;

    [SerializeField] private float gachaRate;
    public float GachaRate { get { return gachaRate; } }

    [SerializeField] private float minGachaRate;
    public float MinGachaRate { get { return minGachaRate; } }

    [SerializeField] private float maxGachaRate;
    public float MaxGachaRate { get { return maxGachaRate; } }

    [SerializeField] private float reactionTime;
    public float ReactionTime { get { return reactionTime; } }

    [Header("------UGUI------")]
    [SerializeField] private GameObject QTECam;
    [SerializeField] private GameObject DBDCam;
    [SerializeField] private GameObject TURBOCam;
    [SerializeField] private GameObject QTECam_;
    [SerializeField] private GameObject DBDCam_;
    [SerializeField] private GameObject TURBOCam_;

    [Header("------Emoji------")]
    [SerializeField] public GameObject emojiAboveLocation;
    [SerializeField] private GameObject surpriseEmoji;
    [SerializeField] public Sprite heartEmoji;
    [SerializeField] public Sprite dizzyEmoji;
    [SerializeField] private GameObject emojiBelowLocation;
    [SerializeField] private Sprite sweatEmoji;

    [Header("------Combo------")]
    [SerializeField] private TextMeshPro comboText;

    [Header("------Mirror------")]
    [SerializeField] private GameObject player;
    [SerializeField] private Transform rightLocation;
    [SerializeField] private Transform leftLocation;
    public bool isFacingRight = true;

    [Header("------Audio------")]
    [SerializeField] private AudioClip heartClip;
    [SerializeField] private AudioClip sweatClip;
    [SerializeField] private AudioClip castingClip;
    [SerializeField] private AudioClip reelingClip;

    [Header("------Tutorial------")]
    [SerializeField] private GameObject tryAgainPanel;

    [Header("------Fin------")]
    [SerializeField] private GameObject[] hearts;
    [SerializeField] private GameObject[] sweats;

    private float currTime;
    private float timerTime;
    private bool stopTimer;
    private bool skipAnimation;

    public Animator shirokoAnimator;
    private AudioSource shirokoAudioSource;

    private void Awake()
    {
        shirokoAnimator = GetComponent<Animator>();
        shirokoAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        // resetPhase() without SadFishing()
        /*
        QTECam.SetActive(false);
        DBDCam.SetActive(false);
        TURBOCam.SetActive(false);
        */

        resetAnimBool();
        shirokoPhase = fishingPhase.BEFORECASTING;
        //shirokoPhase = fishingPhase.TALKING;
        shirokoAnimator.SetBool("isIdle", true);

        gachaRate = SetGachaRate();
        currTime = 0;
        resetTimer();

        skipAnimation = false;
    }

    private void Update()
    {
        if (shirokoPhase == fishingPhase.WAITING)
        {
            currTime += Time.deltaTime;

            if (currTime >= gachaRate)
            {
                Debug.Log("!!!");
                surpriseEmoji.SetActive(true);

                shirokoPhase = fishingPhase.CATCHING;
                shirokoAnimator.SetBool("isReeling", true);
                shirokoAudioSource.clip = reelingClip;
                shirokoAudioSource.loop = true;
                shirokoAudioSource.Play();

                StartCoroutine(Timer());
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(shirokoPhase == fishingPhase.BLOCKCASTING)
            {
                // Block Input
            }
            else if (shirokoPhase == fishingPhase.BEFORECASTING && !skipAnimation)
            {
                shirokoAnimator.SetBool("isCasting", true);
                shirokoPhase = fishingPhase.WAITING;
                shirokoAnimator.SetBool("isIdle", false);
                shirokoAnimator.SetBool("isWaiting", true);

                shirokoAudioSource.clip = castingClip;
                shirokoAudioSource.Play();
            }
            else if (shirokoPhase == fishingPhase.WAITING)
            {
                resetPhase();
            }
            else if (shirokoPhase == fishingPhase.CATCHING)
            {
                Debug.Log("Catch!");
                surpriseEmoji.SetActive(false);

                stopTimer = true;
                shirokoPhase = fishingPhase.ENTERGACHA;
            }
            else if (shirokoPhase == fishingPhase.TALKING)
            {
                if (SceneManager.GetActiveScene().name == "Tutorial")
                {
                    // Now playing Tutorial
                    tryAgainPanel.SetActive(false);

                    shirokoPhase = fishingPhase.BEFORECASTING;
                }
            }
        }

        if(SceneManager.GetActiveScene().name == "Ice")
        {
            if(shirokoPhase == fishingPhase.BLOCKCASTING || shirokoPhase == fishingPhase.TALKING)
            {
                // Block Input
            }
            else if(shirokoPhase == fishingPhase.BEFORECASTING && !skipAnimation)
            {
                if(Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    isFacingRight = false;
                    player.transform.localScale = new Vector3(-1, 1, 0);
                    player.transform.position = leftLocation.position;
                }
                else if(Input.GetKeyDown(KeyCode.RightArrow))
                {
                    isFacingRight = true;
                    player.transform.localScale = new Vector3(1, 1, 0);
                    player.transform.position = rightLocation.position;
                }
            }
        }

        if(SceneManager.GetActiveScene().name == "Ice")
        {
            if(penguinSpawner.penguins[penguinSpawner.penguinIdx].GetComponent<Penguin>().isAngry)
            {
                penguinSpawner.SpawnPenguin();
            }
        }
    }

    public float SetGachaRate()
    {
        return Random.Range(minGachaRate, maxGachaRate);
    }

    public void resetPhase()
    {
        if(isFacingRight)
        {
            QTECam.SetActive(false);
            DBDCam.SetActive(false);
            TURBOCam.SetActive(false);
        }
        else
        {
            QTECam_.SetActive(false);
            DBDCam_.SetActive(false);
            TURBOCam_.SetActive(false);
        }

        resetAnimBool();
        shirokoPhase = fishingPhase.BEFORECASTING;
        shirokoAnimator.SetBool("isIdle", true);

        gachaRate = SetGachaRate();
        currTime = 0;
        resetTimer();

        StartCoroutine(SadFishing());
    }

    public void resetAnimBool()
    {
        shirokoAnimator.SetBool("isCasting", false);
        shirokoAnimator.SetBool("isWaiting", false);
        shirokoAnimator.SetBool("isReeling", false);
        shirokoAnimator.SetBool("isJumping", false);
        shirokoAnimator.SetBool("isCatching", false);
    }

    public IEnumerator HappyFishing()
    {
        if(isFacingRight)
        {
            QTECam.SetActive(false);
            DBDCam.SetActive(false);
            TURBOCam.SetActive(false);
        }
        else
        {
            QTECam_.SetActive(false);
            DBDCam_.SetActive(false);
            TURBOCam_.SetActive(false);
        }

        resetAnimBool();
        shirokoPhase = fishingPhase.BEFORECASTING;
        shirokoAnimator.SetBool("isIdle", true);

        gachaRate = SetGachaRate();
        currTime = 0;
        resetTimer();

        if (gachaManager.comboSys)
        {
            gachaManager.ComboUp();
            comboText.text = gachaManager.comboCnt.ToString() + " Combo!";
            comboText.gameObject.SetActive(true);
        }

        if(SceneManager.GetActiveScene().name == "Ice")
        {
            if ((isFacingRight && penguinSpawner.isPenguin) || (!isFacingRight && !penguinSpawner.isPenguin))
            {
                StartCoroutine(penguinSpawner.penguins[penguinSpawner.penguinIdx].GetComponent<Penguin>().HappyOut());
                penguinSpawner.SpawnPenguin();
            }
            else
            {
                StartCoroutine(penguinSpawner.penguins[penguinSpawner.penguinIdx].GetComponent<Penguin>().AngryOut());
            }
        }

        emojiAboveLocation.GetComponent<SpriteRenderer>().sprite = heartEmoji;
        emojiAboveLocation.SetActive(true);

        if(SceneManager.GetActiveScene().name == "Fin")
        {
            for(int i = 0; i < 3; i++)
            {
                hearts[i].SetActive(true);
            }
        }

        shirokoAudioSource.clip = heartClip;
        shirokoAudioSource.Play();

        skipAnimation = true;

        yield return new WaitForSeconds(0.8f);

        skipAnimation = false;

        emojiAboveLocation.SetActive(false);

        if(SceneManager.GetActiveScene().name == "Fin")
        {
            for(int i = 0; i < 3; i++)
            {
                hearts[i].SetActive(false);
            }
        }

        if (gachaManager.comboSys)
        {
            comboText.gameObject.SetActive(false);
        }
    }

    public IEnumerator SadFishing()
    {
        if (gachaManager.comboSys)
        {
            gachaManager.ComboDown();
        }

        emojiBelowLocation.GetComponent<SpriteRenderer>().sprite = sweatEmoji;
        emojiBelowLocation.SetActive(true);

        if(SceneManager.GetActiveScene().name == "Fin")
        {
            for(int i = 0; i < 3; i++)
            {
                sweats[i].SetActive(true);
            }
        }

        shirokoAudioSource.clip = sweatClip;
        shirokoAudioSource.loop = false;
        shirokoAudioSource.Play();

        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            // Now playing Tutorial
            tryAgainPanel.SetActive(true);

            shirokoPhase = fishingPhase.TALKING;
        }

        skipAnimation = true;

        yield return new WaitForSeconds(0.8f);

        skipAnimation = false;

        emojiBelowLocation.SetActive(false);

        if(SceneManager.GetActiveScene().name == "Fin")
        {
            for(int i = 0; i < 3; i++)
            {
                sweats[i].SetActive(false);
            }
        }
    }

    private IEnumerator Timer()
    {
        while (!stopTimer)
        {
            timerTime -= Time.deltaTime;
            yield return new WaitForSeconds(Unity.Mathematics.math.EPSILON);

            if (timerTime <= 0)
            {
                stopTimer = true;
            }
        }

        if (stopTimer && timerTime <= 0)
        {
            Debug.Log("TIME OUT!");
            surpriseEmoji.SetActive(false);
            resetPhase();
        }
    }

    private void resetTimer()
    {
        timerTime = reactionTime;
        stopTimer = false;
    }
}
