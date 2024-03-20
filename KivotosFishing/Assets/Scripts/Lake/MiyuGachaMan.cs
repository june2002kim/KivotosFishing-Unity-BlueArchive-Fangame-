using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class MiyuGachaMan : MonoBehaviour
{
    [Header("------Fish Pool------")]
    [SerializeField] List<FishData> R_Fish;
    [SerializeField] List<FishData> SR_Fish;
    [SerializeField] List<FishData> SSR_Fish;
    [SerializeField] GameObject fishPrefab;

    [Header("------Rarity probability------")]
    [SerializeField] private int Rare = 775;
    [SerializeField] private int SuperRare = 165;
    [SerializeField] private int SuperSuperRare = 60;

    [Header("------Fish------")]
    [SerializeField] public Fish fish;

    [Header("------System------")]
    [SerializeField] public bool ceilingSys;
    [SerializeField] private int ceilingMax;
    [SerializeField] public int ceilingStack;
    [SerializeField] public bool comboSys;
    [SerializeField] public int comboCnt;
    [SerializeField] private int Rset = 775;
    [SerializeField] private int SRset = 165;
    [SerializeField] private int SSRset = 60;

    void Start()
    {
        ResetGacha();
    }

    public void startGacha()
    {
        GameObject spawnedFish = GameObject.FindGameObjectWithTag("Fish");
        if (spawnedFish != null)
        {
            Destroy(spawnedFish);
        }

        fish = SpawnFish();
        fish.WatchFishInfo();
    }

    public Fish SpawnFish()
    {
        var newFish = Instantiate(fishPrefab).GetComponent<Fish>();
        newFish.FishData = GachaFish();
        return newFish;
    }

    public FishData GachaFish()
    {
        int total = Rare + SuperRare + SuperSuperRare;
        int rarityPick = Random.Range(1, total + 1);
        int fishPick;
        FishData rtnData;

        if (ceilingSys && ceilingStack >= ceilingMax)
        {
            // SSR
            fishPick = Random.Range(0, SSR_Fish.Count);
            rtnData = SSR_Fish[fishPick];

            return rtnData;
        }

        if (rarityPick <= SuperSuperRare)
        {
            // SSR
            fishPick = Random.Range(0, SSR_Fish.Count);
            rtnData = SSR_Fish[fishPick];
        }
        else if (rarityPick <= SuperSuperRare + SuperRare)
        {
            // SR
            fishPick = Random.Range(0, SR_Fish.Count);
            rtnData = SR_Fish[fishPick];
        }
        else
        {
            // R
            fishPick = Random.Range(0, R_Fish.Count);
            rtnData = R_Fish[fishPick];
        }

        return rtnData;
    }

    public void ResetGacha()
    {
        Rare = Rset;
        SuperRare = SRset;
        SuperSuperRare = SSRset;

        comboCnt = 0;

        ceilingMax = 8;
        ceilingStack = 0;
    }

    public void ComboUp()
    {
        if (Rare > SSRset && SuperSuperRare < Rset)
        {
            comboCnt++;

            Rare--;
            SuperSuperRare++;
        }
    }
}