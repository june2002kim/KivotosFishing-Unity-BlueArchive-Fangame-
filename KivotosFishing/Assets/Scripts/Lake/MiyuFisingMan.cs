using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class MiyuFisingMan : MonoBehaviour
{
    [Header("------References------")]
    [SerializeField] private MiyuGachaMan miyuGachaManager;
    [SerializeField] private MiyuScoreMan miyuScoreManager;
    [SerializeField] private LakeManager lakeManager;

    [Header("------Variables------")]
    [SerializeField] private float gachaRate;
    public float GachaRate { get { return gachaRate; } }

    [SerializeField] private float minGachaRate;
    public float MinGachaRate { get { return minGachaRate; } }

    [SerializeField] private float maxGachaRate;
    public float MaxGachaRate { get { return maxGachaRate; } }

    [SerializeField] private int miyuScore;
    public int MiyuScore { get { return miyuScore; } }

    [Header("------Emoji------")]
    [SerializeField] public GameObject emojiAboveLocation;
    [SerializeField] public Sprite heartEmoji;
    [SerializeField] public Sprite dizzyEmoji;
    [SerializeField] private GameObject particles;

    [Header("------Audio------")]
    [SerializeField] private AudioClip miyuCatchClip;

    private bool skipAnimation;
    private bool isMiyuFishing;
    private bool stopInfLoop = false;

    public Animator miyuAnimator;
    private AudioSource miyuAudioSource;

    private void Awake()
    {
        miyuAnimator = GetComponent<Animator>();
        miyuAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        resetAnimBool();

        miyuScore = 0;

        skipAnimation = false;
        isMiyuFishing = false;
    }

    private void Update()
    {
        if (!lakeManager.stopFishing)
        {
            if (!isMiyuFishing && !skipAnimation)
            {
                gachaRate = SetGachaRate();

                StartCoroutine(MiyuFishing());

                isMiyuFishing = true;
            }
        }
        else
        {
            if(!stopInfLoop)
            {
                StopAllCoroutines();
                particles.SetActive(false);
                emojiAboveLocation.SetActive(false);
                stopInfLoop = true;
            }
        }
    }

    private IEnumerator MiyuFishing()
    {
        miyuAnimator.SetBool("isCasting", true);
        miyuAnimator.SetBool("isIdle", false);
        miyuAnimator.SetBool("isWaiting", true);

        yield return new WaitForSeconds(gachaRate);

        miyuGachaManager.startGacha();

        miyuAnimator.SetBool("isCatching", true);

        if (miyuGachaManager.ceilingSys)
        {
            miyuScoreManager.CountingCeilingStack();
        }
        if (miyuGachaManager.comboSys)
        {
            miyuGachaManager.ComboUp();
        }

        miyuScoreManager.setParticleColor();
        particles.SetActive(true);

        miyuScore = miyuScoreManager.calScore();

        emojiAboveLocation.GetComponent<SpriteRenderer>().sprite = heartEmoji;
        emojiAboveLocation.SetActive(true);
        miyuAudioSource.clip = miyuCatchClip;
        miyuAudioSource.Play();

        skipAnimation = true;

        yield return new WaitForSeconds(0.5f);
        resetAnimBool();
        yield return new WaitForSeconds(0.8f);
        
        skipAnimation = false;

        particles.SetActive(false);
        emojiAboveLocation.SetActive(false);

        isMiyuFishing = false;
    }

    public float SetGachaRate()
    {
        return Random.Range(minGachaRate, maxGachaRate);
    }

    public void resetAnimBool()
    {
        miyuAnimator.SetBool("isIdle", true);
        miyuAnimator.SetBool("isCasting", false);
        miyuAnimator.SetBool("isWaiting", false);
        miyuAnimator.SetBool("isCatching", false);
    }
}
