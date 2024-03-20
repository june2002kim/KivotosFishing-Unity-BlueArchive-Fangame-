using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("------References------")]
    [SerializeField] private GachaManager gachaManager;
    [SerializeField] private Animator shirokoAnimator;
    [SerializeField] private FisingManager fishingManager;
    [SerializeField] private AudioSource shirokoAudioSource;
    [SerializeField] private GameObject fish;
    [SerializeField] private GameObject fish_;
    [SerializeField] private ParticleSystem particleSystem;

    [Header("------Audios------")]
    [SerializeField] private AudioClip starClip;

     [Header("------Variables------")]
    public int totalCnt;
    public int totalScore;
    private int addScore;
    public string fishName;

    void Awake()
    {
        totalCnt = 0;
        totalScore = 0;
        addScore = 0;
        fishName = "";
    }
    
    private void Update()
    {
        if(fishingManager.shirokoPhase == fishingPhase.ENTERRECORD)
        {
            fishingManager.shirokoPhase = fishingPhase.BLOCKRECORD;
            shirokoAnimator.SetBool("isCatching", true);

            shirokoAudioSource.loop = false;
            shirokoAudioSource.Stop();

            if(fishingManager.isFacingRight)
            {
                fish.GetComponent<SpriteRenderer>().sprite = gachaManager.fish.fishData.FishImage;
                fish.SetActive(true);
            }
            else
            {
                fish_.GetComponent<SpriteRenderer>().sprite = gachaManager.fish.fishData.FishImage;
                fish_.SetActive(true);
            }

            shirokoAudioSource.clip = starClip;
            shirokoAudioSource.PlayDelayed(1f);

            if(gachaManager.ceilingSys)
            {
                CountingCeilingStack();
            }

            setParticleColor();
            writeJournal();
            calScore();
        }

        if(Input.GetKeyDown(KeyCode.R) && fishingManager.shirokoPhase == fishingPhase.BLOCKRECORD)
        {
            if(fishingManager.isFacingRight)
            {
                fish.SetActive(false);
            }
            else
            {
                fish_.SetActive(false);
            }
            StartCoroutine(fishingManager.HappyFishing());
        }
    }

    private void CountingCeilingStack()
    {
        if(gachaManager.fish.fishData.FishRarity == fishrarity.SSR)
        {
            gachaManager.ceilingStack = 0;
        }
        else
        {
            gachaManager.ceilingStack++;
        }
    }

    private void setParticleColor()
    {
        var col = particleSystem.colorOverLifetime;

        Gradient grad = new Gradient();
        
        if(gachaManager.fish.fishData.FishRarity == fishrarity.SSR)
        {
            grad.SetKeys( new GradientColorKey[] { new GradientColorKey(Color.magenta, 0.0f), 
                                                new GradientColorKey(Color.red, 1.0f) }, 
                        new GradientAlphaKey[] { new GradientAlphaKey(0.8f, 0.0f), 
                                                new GradientAlphaKey(0.4f, 1.0f) } );
        }
        else if(gachaManager.fish.fishData.FishRarity == fishrarity.SR)
        {
            grad.SetKeys( new GradientColorKey[] { new GradientColorKey(Color.yellow, 0.0f), 
                                                new GradientColorKey(Color.green, 1.0f) }, 
                        new GradientAlphaKey[] { new GradientAlphaKey(0.8f, 0.0f), 
                                                new GradientAlphaKey(0.3f, 1.0f) } );
        }
        else if(gachaManager.fish.fishData.FishRarity == fishrarity.R)
        {
            grad.SetKeys( new GradientColorKey[] { new GradientColorKey(Color.blue, 0.0f), 
                                                new GradientColorKey(Color.cyan, 1.0f) }, 
                        new GradientAlphaKey[] { new GradientAlphaKey(0.8f, 0.0f), 
                                                new GradientAlphaKey(0.4f, 1.0f) } );
        }

        col.color = grad;
    }

    private void writeJournal()
    {
        fishName = gachaManager.fish.fishData.FishName;

        int fishCnt = PlayerPrefs.GetInt(fishName);
        PlayerPrefs.SetInt(fishName, ++fishCnt);
        

        Debug.Log("Caught " + PlayerPrefs.GetInt(fishName) + " " + fishName + ".");
    }

    private void calScore()
    {
        totalCnt++;

        if(gachaManager.fish.fishData.FishRarity == fishrarity.SSR)
        {
            addScore = 1000;
        }
        else if(gachaManager.fish.fishData.FishRarity == fishrarity.SR)
        {
            addScore = 500;
        }
        else if(gachaManager.fish.fishData.FishRarity == fishrarity.R)
        {
            addScore = 100;
        }

        totalScore += addScore;

        Debug.Log("total score : " + addScore + "(" + totalCnt + ")");
    }
}
