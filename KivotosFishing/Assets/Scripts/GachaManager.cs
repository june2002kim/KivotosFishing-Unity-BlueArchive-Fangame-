using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class GachaManager : MonoBehaviour
{
    [Header("Fish Pool")]
    [SerializeField] List<FishData> R_Fish;
    [SerializeField] List<FishData> SR_Fish;
    [SerializeField] List<FishData> SSR_Fish;
    [SerializeField] GameObject fishPrefab;

    [Header("Rarity probability")]
    [SerializeField] private int Rare = 755;
    [SerializeField] private int SuperRare = 185;
    [SerializeField] private int SuperSuperRare = 6;

    [Header("Fish")]
    [SerializeField] public Fish fish;

    void Update()
    {
        test();
    }

    private void test()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameObject spawnedFish = GameObject.FindGameObjectWithTag("Fish");
            if(spawnedFish != null)
            {
                Destroy(spawnedFish);
            }

            fish = SpawnFish();
            fish.WatchFishInfo();
        }
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

        if(rarityPick <= SuperSuperRare)
        {
            // SSR
            fishPick = Random.Range(0, SSR_Fish.Count);
            rtnData = SSR_Fish[fishPick];
        }
        else if(rarityPick <= SuperSuperRare + SuperRare)
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
}
