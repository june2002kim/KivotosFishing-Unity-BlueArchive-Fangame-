using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GachaManager gachaManager;
    [SerializeField] private Animator shirokoAnimator;
    [SerializeField] private FisingManager fishingManager;

    private int totalScore = 0;
    private int addScore;
    private bool gotoNextFish;

    void Start()
    {
        gotoNextFish = false;
    }
    
    private void Update()
    {
        if(fishingManager.shirokoPhase == fishingPhase.ENTERRECORD)
        {
            fishingManager.shirokoPhase = fishingPhase.BLOCKRECORD;
            shirokoAnimator.SetBool("isCatching", true);

            writeJournal();
            calScore();
            StartCoroutine(showFish());
        }

        if(Input.GetKeyDown(KeyCode.Space) && fishingManager.shirokoPhase == fishingPhase.BLOCKRECORD)
        {
            gotoNextFish = true;
        }
    }

    private IEnumerator showFish()
    {
        while(!gotoNextFish)
        {
            yield return new WaitForSeconds(math.EPSILON);
        }

        fishingManager.resetPhase();
        gotoNextFish = false;
    }

    private void writeJournal()
    {
        int fishCnt = PlayerPrefs.GetInt(gachaManager.fish.fishData.FishName);
        PlayerPrefs.SetInt(gachaManager.fish.fishData.FishName, ++fishCnt);

        Debug.Log("Caught " + PlayerPrefs.GetInt(gachaManager.fish.fishData.FishName) + " " + gachaManager.fish.fishData.FishName + ".");
    }

    private void calScore()
    {
        if(gachaManager.fish.fishData.FishRarity == fishrarity.SSR)
        {
            addScore = 100;
        }
        else if(gachaManager.fish.fishData.FishRarity == fishrarity.SR)
        {
            addScore = 50;
        }
        else if(gachaManager.fish.fishData.FishRarity == fishrarity.R)
        {
            addScore = 10;
        }

        totalScore += addScore;

        Debug.Log("total score : " + addScore);
    }
}
