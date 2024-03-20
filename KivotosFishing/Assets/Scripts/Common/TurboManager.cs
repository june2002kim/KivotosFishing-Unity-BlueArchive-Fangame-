using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TurboManager : MonoBehaviour
{
    [Header("------Reference------")]
    [SerializeField] private GachaManager gachaManager;
    [SerializeField] private FisingManager fishingManager;
    [SerializeField] public GameObject turboCanvas;
    [SerializeField] private int rarity;

    [Header("------UGUI------")]
    [SerializeField] private GameObject TURBOCam;

    [Header("------MIRROR------")]
    [SerializeField] private GameObject TURBOCam_;
    [SerializeField] private RectTransform sliderTransform_;

    [Header("------Timer------")]
    [SerializeField] private Slider turboSlider;
    [SerializeField] private float sliderTime = 10f;
    [SerializeField] private float currentTime = 0f;
    [SerializeField] private bool stopTimer;
    /*
    [SerializeField] private float plusWeight = 1.2f;
    [SerializeField] private float minusWeight = 1.0f;
    */
    [SerializeField] private float plusValue;
    [SerializeField] private float minusValue;

    private AudioSource turboAudioSource;


    // Start is called before the first frame update
    void Start()
    {
        resetTimer();
        turboAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(fishingManager.shirokoPhase == fishingPhase.ENTERTURBO)
        {
            if(SceneManager.GetActiveScene().name == "Ice")
            {
                if(fishingManager.isFacingRight)
                {
                    TURBOCam.SetActive(true);
                    
                    sliderTransform_.anchoredPosition = new Vector2(500, 0);
                }
                else
                {
                    TURBOCam_.SetActive(true);

                    sliderTransform_.anchoredPosition = new Vector2(-500, 0);
                }
            }
            else
            {
                TURBOCam.SetActive(true);
            }

            turboCanvas.SetActive(true);
            rarity = gachaManager.fish.fishData.FishRarity.ToString().Length;

            // [Difficulty]
            /*
            plusValue = plusWeight / rarity;
            minusValue = rarity * minusWeight;
            */
            if(rarity == 1)
            {
                // R
                plusValue = 1.0f;
                minusValue = 1.2f;
            }
            else if(rarity == 2)
            {
                // SR
                plusValue = 0.5f;
                minusValue = 2.1f;
            }
            else
            {
                // SSR
                plusValue = 0.4f;
                minusValue = 2.8f;
            }
            
            resetTimer();
            StartCoroutine(Timer());

            fishingManager.shirokoPhase = fishingPhase.BLOCKTURBO;
        }

        if(Input.GetKeyDown(KeyCode.Space) && fishingManager.shirokoPhase == fishingPhase.BLOCKTURBO)
        {
            currentTime += plusValue;
            turboAudioSource.Play();
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
                fishingManager.resetPhase();
            }
            else{
                Debug.Log("HIT!");
                fishingManager.shirokoPhase = fishingPhase.ENTERRECORD;
            }
            turboCanvas.SetActive(false);
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
