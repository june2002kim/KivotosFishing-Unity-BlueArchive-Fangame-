using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class TurboManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GachaManager gachaManager;
    [SerializeField] private int rarity;

    [Header("Timer")]
    [SerializeField] private Slider turboSlider;
    [SerializeField] private float sliderTime = 10f;
    [SerializeField] private float currentTime = 0f;
    [SerializeField] private bool stopTimer;
    [SerializeField] private float plusWeight = 1.2f;
    [SerializeField] private float minusWeight = 1.0f;
    [SerializeField] private float plusValue;
    [SerializeField] private float minusValue;


    // Start is called before the first frame update
    void Start()
    {
        resetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            rarity = gachaManager.fish.fishData.FishRarity.ToString().Length;

            plusValue = plusWeight / rarity;
            minusValue = rarity * minusWeight;
            
            resetTimer();

            StartCoroutine(Timer());
        }

        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentTime += plusValue;
        }
    }

    private IEnumerator Timer()
    {
        while(!stopTimer)
        {
            currentTime -= Time.deltaTime * minusValue;
            yield return new WaitForSeconds(math.EPSILON);

            if(currentTime <= 0 || currentTime >= sliderTime)
            {
                stopTimer = true;
            }

            if(!stopTimer)
            {
                turboSlider.value = currentTime;
            }
        }

        if(stopTimer)
        {
            if(currentTime <= 0)
            {
                Debug.Log("MISS!");
            }
            else{
                Debug.Log("HIT!");
            }
        }
    }

    private void resetTimer()
    {
        turboSlider.maxValue = sliderTime;
        turboSlider.value = sliderTime / 2;

        currentTime = sliderTime / 2;

        stopTimer = false;
    }
}