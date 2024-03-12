using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class QTEManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GachaManager gachaManager;
    [SerializeField] private QTEAlignment qteAlignment;
    [SerializeField] private FisingManager fishingManager;
    [SerializeField] private GameObject qteCanvas;

    [Header("variables")]
    [SerializeField] public int[] arrowDirection = new int[5];
    [SerializeField] private bool isWatingQTE;
    [SerializeField] private int keyCount;
    [SerializeField] private int leftRight;
    [SerializeField] private int index;
    [SerializeField] private bool isFailed;

    [Header("UGUI")]
    [SerializeField] private GameObject QTECanvas;
    [SerializeField] private GameObject KeyPanel;
    [SerializeField] private GameObject TimerPanel;

    [Header("Timer")]
    [SerializeField] private Slider timerSlider;
    [SerializeField] private float sliderTime = 5f;
    [SerializeField] private float currentTime = 0f;
    [SerializeField] private bool stopTimer;



    void Start()
    {
        qteAlignment = KeyPanel.GetComponent<QTEAlignment>();

        isWatingQTE = false;
        isFailed = false;
        index = 0;
    }

    void Update()
    {
        if (!isWatingQTE && fishingManager.shirokoPhase == fishingPhase.ENTERQTE)
        {
            qteCanvas.SetActive(true);
            StartCoroutine(StartQTE());
            fishingManager.shirokoPhase = fishingPhase.BLOCKQTE;
        }

        if (isWatingQTE && Input.anyKeyDown)
        {
            if (leftRight == 0)
            {
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    isFailed = false;
                }
                else
                {
                    isFailed = true;
                }

                //StartCoroutine(Play());
            }
            else
            {
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    isFailed = false;
                }
                else if (!Input.GetKey(KeyCode.LeftArrow))
                {
                    isFailed = true;
                }

                //StartCoroutine(Play());
            }
            StartCoroutine(PlayQTE());
        }
    }

    private int Setting()
    {
        string rarity;
        int keyCount = 3;
        int leftRight;

        rarity = gachaManager.fish.fishData.FishRarity.ToString();
        Debug.Log("Rarity : " + rarity);

        keyCount = rarity.Length + 2;
        Debug.Log(keyCount);

        QTEAlignment keyPanelCode = KeyPanel.GetComponent<QTEAlignment>();

        keyPanelCode.gridAlignment(keyCount);

        for (int i = 0; i < keyCount; i++)
        {
            leftRight = UnityEngine.Random.Range(0, 2);
            if (leftRight != 0)
            {
                leftRight++;
            }
            arrowDirection[i] = leftRight;

            keyPanelCode.QTEKeys[i].changeKey(leftRight);
        }

        return keyCount;
    }

    private IEnumerator StartQTE()
    {
        isWatingQTE = true;

        keyCount = Setting();

        resetTimer();
        StartCoroutine(Timer());

        leftRight = arrowDirection[index];

        qteAlignment.QTEKeys[index].changeKey(leftRight + 1);

        yield return null;
    }

    private IEnumerator PlayQTE()
    {
        if (!isFailed)
        {
            qteAlignment.QTEKeys[index].changeKey(leftRight);
            qteAlignment.QTEKeys[index].changeColor();

            index++;
            if (index == qteAlignment.QTEKeys.Count)
            {
                Debug.Log("PASS!");
                
                resetQTE();

                qteCanvas.SetActive(false);
                fishingManager.shirokoPhase = fishingPhase.ENTERDBD;

                yield break;
            }
            leftRight = arrowDirection[index];

            qteAlignment.QTEKeys[index].changeKey(leftRight + 1);
        }
        else
        {
            Debug.Log("FAIL!");
            
            resetQTE();
            fishingManager.resetPhase();
            qteCanvas.SetActive(false);
        }
    }

    private void resetQTE()
    {
        qteAlignment.gridReset();

        isWatingQTE = false;
        index = 0;

        stopTimer = true;
    }

    private IEnumerator Timer()
    {
        while(!stopTimer)
        {
            currentTime -= Time.deltaTime;
            yield return new WaitForSeconds(math.EPSILON);

            if(currentTime <= 0)
            {
                stopTimer = true;
            }

            if(!stopTimer)
            {
                timerSlider.value = currentTime;
            }
        }

        if(stopTimer && currentTime<=0)
        {
            Debug.Log("TIME OUT!");

            resetQTE();
            fishingManager.resetPhase();
        }
    }

    private void resetTimer()
    {
        timerSlider.maxValue = sliderTime;
        timerSlider.value = sliderTime;

        currentTime = sliderTime;

        stopTimer = false;
    }
}
