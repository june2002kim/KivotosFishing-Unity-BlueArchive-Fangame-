using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

public class MiyuScoreMan : MonoBehaviour
{
    [Header("------References------")]
    [SerializeField] private MiyuGachaMan miyuGachaManager;
    [SerializeField] private Animator miyuAnimator;
    [SerializeField] private GameObject fish;
    [SerializeField] private ParticleSystem particleSystem;

    public int totalCnt;
    public int totalScore;
    private int addScore;

    void Awake()
    {
        totalCnt = 0;
        totalScore = 0;
        addScore = 0;
    }

    public void CountingCeilingStack()
    {
        if(miyuGachaManager.fish.fishData.FishRarity == fishrarity.SSR)
        {
            miyuGachaManager.ceilingStack = 0;
        }
        else
        {
            miyuGachaManager.ceilingStack++;
        }
    }

    public void setParticleColor()
    {
        var col = particleSystem.colorOverLifetime;

        Gradient grad = new Gradient();
        
        if(miyuGachaManager.fish.fishData.FishRarity == fishrarity.SSR)
        {
            grad.SetKeys( new GradientColorKey[] { new GradientColorKey(Color.magenta, 0.0f), 
                                                new GradientColorKey(Color.red, 1.0f) }, 
                        new GradientAlphaKey[] { new GradientAlphaKey(0.8f, 0.0f), 
                                                new GradientAlphaKey(0.4f, 1.0f) } );
        }
        else if(miyuGachaManager.fish.fishData.FishRarity == fishrarity.SR)
        {
            grad.SetKeys( new GradientColorKey[] { new GradientColorKey(Color.yellow, 0.0f), 
                                                new GradientColorKey(Color.green, 1.0f) }, 
                        new GradientAlphaKey[] { new GradientAlphaKey(0.8f, 0.0f), 
                                                new GradientAlphaKey(0.3f, 1.0f) } );
        }
        else if(miyuGachaManager.fish.fishData.FishRarity == fishrarity.R)
        {
            grad.SetKeys( new GradientColorKey[] { new GradientColorKey(Color.blue, 0.0f), 
                                                new GradientColorKey(Color.cyan, 1.0f) }, 
                        new GradientAlphaKey[] { new GradientAlphaKey(0.8f, 0.0f), 
                                                new GradientAlphaKey(0.4f, 1.0f) } );
        }

        col.color = grad;
    }

    public int calScore()
    {
        totalCnt++;

        if(miyuGachaManager.fish.fishData.FishRarity == fishrarity.SSR)
        {
            addScore = 1000;
        }
        else if(miyuGachaManager.fish.fishData.FishRarity == fishrarity.SR)
        {
            addScore = 500;
        }
        else if(miyuGachaManager.fish.fishData.FishRarity == fishrarity.R)
        {
            addScore = 100;
        }

        totalScore += addScore;
        Debug.Log("Miyu total score : " + addScore + "(" + totalCnt + ")");

        return totalScore;
    }
}
