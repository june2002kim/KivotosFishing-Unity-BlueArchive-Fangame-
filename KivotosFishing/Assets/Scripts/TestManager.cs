using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    [SerializeField] FishData[] cheatFish;

    public void ControlPlayerPrefs()
    {
        resetPlayerPrefs();
        
        for(int i = 0; i < cheatFish.Length; i++)
        {
            PlayerPrefs.SetInt(cheatFish[i].FishName, 1);
        }
    }

    private void resetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
