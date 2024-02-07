using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField] public FishData fishData;
    public FishData FishData{set{fishData = value;}}

    public void WatchFishInfo()
    {
        Debug.Log("Fish name : " + fishData.FishName);
        Debug.Log("Fish rarity : " + fishData.FishRarity);
        Debug.Log("Fish area : " + fishData.FishArea);
        Debug.Log("Fish description : " + fishData.FishDescription);
    }
}
